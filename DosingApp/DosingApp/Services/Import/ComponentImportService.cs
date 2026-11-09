using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.AgroAssist.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class ComponentImportService
    {
        private readonly AppDbContext _db;
        private readonly int _manufacturerId;
        private readonly Func<int?, string> _unitNameResolver;

        public ComponentImportService(AppDbContext db, int manufacturerId, Func<int?, string> unitNameResolver = null)
        {
            _db = db;
            _manufacturerId = manufacturerId;
            _unitNameResolver = unitNameResolver;
        }

        public async Task<int> SaveAsync(IEnumerable<PesticideDto> pesticides, CancellationToken ct = default)
        {
            var list = pesticides
                .Where(p => p.PesticideGuid != Guid.Empty)
                .GroupBy(p => p.PesticideGuid)
                .Select(g => g.Last())
                .ToList();

            if (list.Count == 0) return 0;

            var guids = list.Select(p => p.PesticideGuid).ToList();

            // существующие Component по PesticideGuid — чанками
            var existing = new Dictionary<Guid, Component>();
            const int Chunk = 500;
            for (int i = 0; i < guids.Count; i += Chunk)
            {
                var chunk = guids.Skip(i).Take(Chunk).ToList();
                var dict = await _db.Components
                    .Where(c => c.PesticideGuid.HasValue && chunk.Contains(c.PesticideGuid.Value))
                    .ToDictionaryAsync(c => c.PesticideGuid.Value, ct)
                    .ConfigureAwait(false);
                foreach (var kv in dict) existing[kv.Key] = kv.Value;
            }

            var toInsert = new List<Component>();
            var toUpdate = new List<Component>();

            foreach (var dto in list)
            {
                if (existing.TryGetValue(dto.PesticideGuid, out var entity))
                {
                    entity.Name = dto.Name ?? string.Empty;
                    entity.Consistency = DetectConsistency(dto.UnitId);
                    entity.ManufacturerId = _manufacturerId;
                    toUpdate.Add(entity);
                }
                else
                {
                    toInsert.Add(new Component
                    {
                        Name = dto.Name ?? string.Empty,
                        PesticideGuid = dto.PesticideGuid,
                        Consistency = DetectConsistency(dto.UnitId),
                        ManufacturerId = _manufacturerId,
                    });
                }
            }

            var oldAutoDetect = _db.ChangeTracker.AutoDetectChangesEnabled;
            _db.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                if (toInsert.Count > 0)
                {
                    using (var tx = await _db.Database.BeginTransactionAsync(ct).ConfigureAwait(false))
                    {
                        for (int i = 0; i < toInsert.Count; i += 1000)
                        {
                            var batch = toInsert.GetRange(i, Math.Min(1000, toInsert.Count - i));
                            await _db.Components.AddRangeAsync(batch, ct).ConfigureAwait(false);
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
                        for (int i = 0; i < toUpdate.Count; i += 1000)
                        {
                            var batch = toUpdate.GetRange(i, Math.Min(1000, toUpdate.Count - i));
                            _db.Components.UpdateRange(batch);
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

            return list.Count;
        }

        private string DetectConsistency(int? unitId)
        {
            var unitName = _unitNameResolver?.Invoke(unitId);
            if (string.IsNullOrEmpty(unitName)) return ComponentConsistency.Liquid;
            return unitName == VolumeUnit.Dry
                ? ComponentConsistency.Dry
                : ComponentConsistency.Liquid;
        }

        private void ClearTracker()
        {
            foreach (var entry in _db.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }
    }
}
