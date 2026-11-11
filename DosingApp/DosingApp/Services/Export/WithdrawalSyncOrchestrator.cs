using DosingApp.DataContext;
using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services.Import;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Export
{
    public class WithdrawalSyncOrchestrator
    {
        private readonly AppDbContext _db;
        private readonly WithdrawalPayloadBuilder _builder;
        private readonly WithdrawalSyncService _sync;

        public WithdrawalSyncOrchestrator(AppDbContext db, WithdrawalPayloadBuilder builder, WithdrawalSyncService sync)
        {
            _db = db;
            _builder = builder;
            _sync = sync;
        }

        public async Task<WithdrawalBulkSummaryDto> PushAsync(IProgress<SyncProgress> progress = null, CancellationToken ct = default)
        {
            var payload = await _builder.BuildAsync(ct).ConfigureAwait(false);
            progress?.Report(new SyncProgress { Stage = "Отправка данных по выполненным заданиям" });
            var response = await _sync.SendAsync(payload, ct).ConfigureAwait(false);

            if (response?.Results == null)
                return response?.Summary ?? new WithdrawalBulkSummaryDto();

            // сопоставление по instance (уникален внутри каждого списка)
            var createByInstance = payload.ToBeCreated.ToDictionary(x => x.Instance);
            var updateByInstance = payload.ToBeUpdated.ToDictionary(x => x.Instance);
            var deleteByInstance = payload.ToBeDeleted.ToDictionary(x => x.Instance);

            foreach (var result in response.Results)
            {
                ct.ThrowIfCancellationRequested();

                switch (result.Status)
                {
                    case WithdrawalBulkResultStatus.Created:
                        progress?.Report(new SyncProgress { Stage = "Обработка созданных записей" });
                        await HandleCreatedAsync(result, createByInstance, ct).ConfigureAwait(false);
                        break;

                    case WithdrawalBulkResultStatus.Updated:
                        progress?.Report(new SyncProgress { Stage = "Обработка обновленных записей" });
                        await HandleUpdatedAsync(result, updateByInstance, ct).ConfigureAwait(false);
                        break;

                    case WithdrawalBulkResultStatus.Deleted:
                        progress?.Report(new SyncProgress { Stage = "Обработка удаленных записей" });
                        await HandleDeletedAsync(result, deleteByInstance, ct).ConfigureAwait(false);
                        break;

                    case WithdrawalBulkResultStatus.Failed:
                        // не трогаем — повторится в следующей синхронизации
                        progress?.Report(new SyncProgress { Stage = "Обработка неудавшихся записей" });
                        System.Diagnostics.Debug.WriteLine($"[Withdrawal] failed instance={result.Instance}: {result.Error}");
                        break;
                }
            }

            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return response.Summary;
        }

        private async Task HandleCreatedAsync(WithdrawalBulkResultDto result, Dictionary<int, WithdrawalCreateDto> createByInstance, CancellationToken ct)
        {
            if (!createByInstance.TryGetValue(result.Instance, out var dto))
                return;

            var local = await _db.Withdrawals.FirstOrDefaultAsync(w => w.WithdrawalExtId == dto.WithdrawalExtId, ct).ConfigureAwait(false);

            if (local == null) return;

            if (result.Data?.WithdrawalGuid is Guid g && g != Guid.Empty)
                local.WithdrawalGuid = g;

            local.SyncState = WithdrawalSyncState.Synced;
        }

        private async Task HandleUpdatedAsync(WithdrawalBulkResultDto result, Dictionary<int, WithdrawalUpdateDto> updateByInstance, CancellationToken ct)
        {
            Guid guid;

            if (result.Data?.WithdrawalGuid is Guid g && g != Guid.Empty)
            {
                guid = g;
            }
            else if (updateByInstance.TryGetValue(result.Instance, out var dto))
            {
                guid = dto.WithdrawalGuid;
            }
            else return;

            var entity = await _db.Withdrawals.FirstOrDefaultAsync(w => w.WithdrawalGuid == guid, ct).ConfigureAwait(false);

            if (entity != null)
                entity.SyncState = WithdrawalSyncState.Synced;
        }

        private async Task HandleDeletedAsync(WithdrawalBulkResultDto result, Dictionary<int, WithdrawalDeleteDto> deleteByInstance, CancellationToken ct)
        {
            Guid guid;

            if (result.Data?.WithdrawalGuid is Guid g && g != Guid.Empty)
            {
                guid = g;
            }
            else if (deleteByInstance.TryGetValue(result.Instance, out var dto))
            {
                guid = dto.WithdrawalGuid;
            }
            else return;

            var entity = await _db.Withdrawals.FirstOrDefaultAsync(w => w.WithdrawalGuid == guid, ct).ConfigureAwait(false);

            if (entity != null)
                _db.Withdrawals.Remove(entity);
        }
    }
}
