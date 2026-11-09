using System;
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
                var units = new UnitImportService(api, db);
                var pesticides = new PesticideImportService(api, db);
                var techOperations = new TechOperationImportService(api, db);

                // ── 1. Units ──
                progress?.Report(new SyncProgress { Stage = "Загрузка справочника 'Единицы измерения'" });
                result.UnitsSaved = await units.LoadAllAsync(null, ct).ConfigureAwait(false);
                // ── 2. Pesticides ──
                progress?.Report(new SyncProgress { Stage = "Загрузка справочника 'Пестициды'" });
                result.PesticidesSaved = await pesticides.LoadAllAsync(null, ct).ConfigureAwait(false);
                // ── 3. Techoperations ──
                progress?.Report(new SyncProgress { Stage = "Загрузка техопераций" });
                result.TechoperationsSaved = await techOperations.LoadAllAsync(null, ct).ConfigureAwait(false);
            }

            progress?.Report(new SyncProgress { Stage = "Загрузка завершена" });
            return result;
        }
    }

    public class SyncResult
    {
        public int UnitsSaved { get; set; }
        public int PesticidesSaved { get; set; }
        public int TechoperationsSaved { get; set; }
    }

    public class SyncProgress
    {
        public string Stage { get; set; }
        public int Current { get; set; }
        public int Total { get; set; }
    }
}
