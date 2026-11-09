using DosingApp.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class SyncService
    {
        public SyncService()
        {
        }

        public async Task<SyncResult> SyncAllAsync(ApiService api, IProgress<SyncProgress> progress = null, CancellationToken ct = default)
        {
            var result = new SyncResult();

            // один контекст на всю последовательную синхронизацию
            using (var db = App.GetContext())
            {
                // ── 1. Units ──
                var units = new UnitImportService(api, db);
                progress?.Report(new SyncProgress { Stage = "Загрузка справочника 'Единицы измерения'" });
                result.UnitsSaved = await units.LoadAllAsync(null, ct).ConfigureAwait(false);

                // ── 2. Pesticides ──
                var pesticides = new PesticideImportService(api, db);
                progress?.Report(new SyncProgress { Stage = "Загрузка справочника 'Пестициды'" });
                var pesticidesDto = await pesticides.LoadAllAsync(null, ct).ConfigureAwait(false);

                // ── 3. Один производитель для всех ──
                progress?.Report(new SyncProgress { Stage = "Создание производителя" });
                var manufacturerProvider = new ManufacturerProvider(db);
                var manufacturerId = await manufacturerProvider.GetOrCreateAsync(ManufacturerDefaults.Name, ct).ConfigureAwait(false);

                // ── 4. Components — привязка к пестицидам под этим производителем ──
                progress?.Report(new SyncProgress { Stage = "Обновление справочника 'Компоненты'" });
                var unitNameResolver = BuildUnitNameResolver(db);
                var componentImport = new ComponentImportService(db, manufacturerId, unitNameResolver);
                result.ComponentsSaved = await componentImport.SaveAsync(pesticidesDto, ct).ConfigureAwait(false);

                // ── 5. Techoperations ──
                var techOperations = new TechOperationImportService(api, db);
                progress?.Report(new SyncProgress { Stage = "Загрузка списка техопераций (план на день)" });
                var techDto = await techOperations.LoadAllAsync(null, ct).ConfigureAwait(false);

                // ── 6. Recipes (из TechOperation.PlanInventories) ──
                var recipeImport = new RecipeImportService(db);
                progress?.Report(new SyncProgress { Stage = "Формирование рецептов" });
                var recipesCount = await recipeImport.SaveFromTechOperationsAsync(techDto, ct);
            }

            progress?.Report(new SyncProgress { Stage = "Загрузка завершена" });
            return result;
        }

        private static Func<int?, string> BuildUnitNameResolver(AppDbContext db)
        {
            var map = db.Units.AsNoTracking().ToDictionary(u => u.UnitId, u => u.Name);
            return unitId => unitId.HasValue && map.TryGetValue(unitId.Value, out var n) ? n : null;
        }
    }

    public class SyncResult
    {
        public int UnitsSaved { get; set; }
        public int PesticidesSaved { get; set; }
        public int ComponentsSaved { get; set; }
        public int TechoperationsSaved { get; set; }
    }

    public class SyncProgress
    {
        public string Stage { get; set; }
        public int Current { get; set; }
        public int Total { get; set; }
    }
}
