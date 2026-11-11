using DosingApp.DataContext;
using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Export
{
    public class WithdrawalPayloadBuilder
    {
        private readonly AppDbContext _db;

        public WithdrawalPayloadBuilder(AppDbContext db) => _db = db;

        public async Task<WithdrawalBulkRequestDto> BuildAsync(CancellationToken ct = default)
        {
            var payload = new WithdrawalBulkRequestDto();
            int instance = 0;

            // ── to_be_created ──
            var toCreate = await _db.Withdrawals
                .Where(w => w.SyncState == WithdrawalSyncState.New)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            payload.ToBeCreated = toCreate.Select(w => new WithdrawalCreateDto
            {
                Instance = ++instance,
                WithdrawalExtId = w.WithdrawalExtId,
                TechoperationGuid = w.TechoperationGuid,
                StructureGuid = w.StructureGuid,
                FieldGuid = w.FieldGuid,
                EntityGuid = w.EntityGuid.GetValueOrDefault(),
                EntityTypeId = w.EntityTypeId.GetValueOrDefault(),
                Total = w.Total.GetValueOrDefault(),
                FactAreaHectare = w.FactAreaHectare.GetValueOrDefault(),
                UnitId = w.UnitId.GetValueOrDefault(),
            }).ToList();

            // ── to_be_updated ──
            var toUpdate = await _db.Withdrawals
                .Where(w => w.SyncState == WithdrawalSyncState.Modified)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            payload.ToBeUpdated = toUpdate.Select(w => new WithdrawalUpdateDto
            {
                Instance = ++instance,
                WithdrawalGuid = w.WithdrawalGuid.GetValueOrDefault(),
                WithdrawalExtId = w.WithdrawalExtId,
                EntityGuid = w.EntityGuid.GetValueOrDefault(),
                EntityTypeId = w.EntityTypeId.GetValueOrDefault(),
                FactAreaHectare = w.FactAreaHectare.GetValueOrDefault(),
                Total = w.Total.GetValueOrDefault(),
                UnitId = w.UnitId.GetValueOrDefault(),
            }).ToList();

            // ── to_be_deleted ──
            var toDelete = await _db.Withdrawals
                .Where(w => w.SyncState == WithdrawalSyncState.Deleted)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            payload.ToBeDeleted = toDelete.Select(w => new WithdrawalDeleteDto
            {
                Instance = ++instance,
                WithdrawalGuid = w.WithdrawalGuid.GetValueOrDefault(),
            }).ToList();

            return payload;
        }
    }
}
