using DosingApp.DataContext;
using DosingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public static class ManufacturerDefaults
    {
        public const string Name = "История поля (agroassist.ru)";
    }

    public class ManufacturerProvider
    {
        private readonly AppDbContext _db;

        public ManufacturerProvider(AppDbContext db) => _db = db;

        /// Возвращает Id производителя по имени, создаёт при отсутствии.
        public async Task<int> GetOrCreateAsync(string name, CancellationToken ct = default)
        {
            name = (name ?? ManufacturerDefaults.Name).Trim();

            var existing = await _db.Manufacturers
                .Where(m => m.Name == name)
                .Select(m => (int?)m.ManufacturerId)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

            if (existing.HasValue) return existing.Value;

            var entity = new Manufacturer { Name = name };
            _db.Manufacturers.Add(entity);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return entity.ManufacturerId;
        }
    }
}
