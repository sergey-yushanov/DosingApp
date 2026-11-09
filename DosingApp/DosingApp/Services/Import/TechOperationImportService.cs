using DosingApp.DataContext;
using DosingApp.Mapping;
using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class TechOperationImportService
    {
        private readonly ApiService _api;
        private readonly AppDbContext _db;
        private readonly GenericUpsertService<TechOperationDto, TechOperation, Guid> techUpsert;

        public TechOperationImportService(ApiService api, AppDbContext db)
        {
            _api = api;
            _db = db;
            techUpsert = new GenericUpsertService<TechOperationDto, TechOperation, Guid>(db, new TechOperationMapper());
        }

        public async Task<int> LoadAllAsync(IProgress<int> progress = null, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            DateTime now = DateTime.Now;
            DateTime startPlanDatetime = now.Date;
            DateTime endPlanDatetime = now.Date.AddDays(1).AddMilliseconds(-1);

            string url = "techoperations/list";
            url = QueryHelpers.AddQueryString(url, "start_plan_datetime", startPlanDatetime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            url = QueryHelpers.AddQueryString(url, "end_plan_datetime", endPlanDatetime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            url = QueryHelpers.AddQueryString(url, "dates_filter_mode", "any");
            url = QueryHelpers.AddQueryString(url, "status_ids", "1");
            url = QueryHelpers.AddQueryString(url, "status_ids", "2");

            var dtos = await _api.GetAsync<TechOperationsListResponseDto>(url).ConfigureAwait(false);

            if (dtos == null || dtos.Count == 0) return 0;

            // 1. Родители
            var techCount = await techUpsert.SaveAsync(dtos, ct).ConfigureAwait(false);

            // 2. Дети + удаление осиротевших
            int invCount = 0;
            foreach (var tech in dtos)
            {
                var incoming = tech.PlanInventories ?? new List<PlanInventoryDto>();

                // 2.1. Upsert пришедших
                if (incoming.Count > 0)
                {
                    var invUpsert = new GenericUpsertService<PlanInventoryDto, PlanInventory, Guid>(_db, new PlanInventoryMapper(tech.TechoperationGuid));
                    invCount += await invUpsert.SaveAsync(incoming, ct).ConfigureAwait(false);
                }

                // 2.2. Удалить тех, кого нет в incoming
                var incomingGuids = incoming
                    .Where(p => p.PlanInventoryGuid != Guid.Empty)
                    .Select(p => p.PlanInventoryGuid)
                    .ToList();

                var toDelete = await _db.PlanInventories
                    .Where(p => p.TechOperationGuid == tech.TechoperationGuid && !incomingGuids.Contains(p.PlanInventoryGuid))
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

                if (toDelete.Count > 0)
                {
                    _db.PlanInventories.RemoveRange(toDelete);
                    await _db.SaveChangesAsync(ct).ConfigureAwait(false);
                }
            }

            progress?.Report(techCount + invCount);
            return techCount + invCount;
        }
    }
}
