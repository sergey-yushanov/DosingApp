using DosingApp.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class GenericUpsertService<TDto, TEntity, TKey>
            where TEntity : class
    {
        private readonly AppDbContext _db;
        private readonly IEntityMapper<TDto, TEntity, TKey> _mapper;

        public int LoadChunkSize { get; set; } = 500;
        public int SaveChunkSize { get; set; } = 1000;

        public GenericUpsertService(AppDbContext db, IEntityMapper<TDto, TEntity, TKey> mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> SaveAsync(IEnumerable<TDto> dtos, CancellationToken ct = default)
        {
            if (dtos == null) return 0;

            var list = dtos.ToList();
            if (list.Count == 0) return 0;

            // 1. Уникальные по ключу, валидные
            var incoming = list
                .Where(_mapper.IsValid)
                .GroupBy(_mapper.GetKeyFromDto)
                .Select(g => g.Last())
                .ToList();

            if (incoming.Count == 0) return 0;

            // 2. Загружаем существующие чанками
            var existing = new Dictionary<TKey, TEntity>();
            var keySelectorCompiled = _mapper.KeySelector.Compile();

            for (int i = 0; i < incoming.Count; i += LoadChunkSize)
            {
                var chunkKeys = incoming
                    .Skip(i).Take(LoadChunkSize)
                    .Select(_mapper.GetKeyFromDto)
                    .ToList();

                var dict = await _db.Set<TEntity>()
                    .Where(BuildContains(_mapper.KeySelector, chunkKeys))
                    .ToDictionaryAsync(keySelectorCompiled, ct)
                    .ConfigureAwait(false);

                foreach (var kv in dict) existing[kv.Key] = kv.Value;
            }

            // 3. Split insert/update
            var toInsert = new List<TEntity>();
            var toUpdate = new List<TEntity>();

            foreach (var dto in incoming)
            {
                var key = _mapper.GetKeyFromDto(dto);
                if (existing.TryGetValue(key, out var entity))
                {
                    _mapper.UpdateEntity(entity, dto);
                    toUpdate.Add(entity);
                }
                else
                {
                    toInsert.Add(_mapper.CreateEntity(dto));
                }
            }

            // 4. Сохраняем чанками, в транзакции, с отключённым DetectChanges
            var oldAutoDetect = _db.ChangeTracker.AutoDetectChangesEnabled;
            _db.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                if (toInsert.Count > 0)
                {
                    using (var tx = await _db.Database.BeginTransactionAsync(ct).ConfigureAwait(false))
                    {
                        for (int i = 0; i < toInsert.Count; i += SaveChunkSize)
                        {
                            var batch = toInsert.GetRange(i, Math.Min(SaveChunkSize, toInsert.Count - i));
                            await _db.Set<TEntity>().AddRangeAsync(batch, ct).ConfigureAwait(false);
                            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
                            ClearTracker();
                        }
                        await tx.CommitAsync(ct).ConfigureAwait(false);
                    }
                }

                if (toUpdate.Count > 0)
                {
                    using (var tx = await _db.Database.BeginTransactionAsync(ct).ConfigureAwait(false))
                    {
                        for (int i = 0; i < toUpdate.Count; i += SaveChunkSize)
                        {
                            var batch = toUpdate.GetRange(i, Math.Min(SaveChunkSize, toUpdate.Count - i));
                            _db.Set<TEntity>().UpdateRange(batch);
                            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
                            ClearTracker();
                        }
                        await tx.CommitAsync(ct).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                _db.ChangeTracker.AutoDetectChangesEnabled = oldAutoDetect;
            }

            return incoming.Count;
        }

        private void ClearTracker()
        {
            foreach (var entry in _db.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }

        /// Собираем Expression: e => keys.Contains(e.Key)
        private static System.Linq.Expressions.Expression<Func<TEntity, bool>> BuildContains(
            System.Linq.Expressions.Expression<Func<TEntity, TKey>> keySelector,
            List<TKey> keys)
        {
            var parameter = keySelector.Parameters[0];
            var body = keySelector.Body;
            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TKey));
            var call = System.Linq.Expressions.Expression.Call(
                containsMethod,
                System.Linq.Expressions.Expression.Constant(keys),
                body);
            return System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(call, parameter);
        }
    }
}
