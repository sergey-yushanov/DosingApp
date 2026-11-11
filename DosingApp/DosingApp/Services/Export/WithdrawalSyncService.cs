using DosingApp.Models.AgroAssist.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Export
{
    public class WithdrawalSyncService
    {
        private readonly ApiService _api;

        public WithdrawalSyncService(ApiService api) => _api = api;

        public async Task<WithdrawalBulkResponseDto> SendAsync(WithdrawalBulkRequestDto payload, CancellationToken ct = default)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            bool empty = (payload.ToBeCreated?.Count ?? 0) == 0
                      && (payload.ToBeUpdated?.Count ?? 0) == 0
                      && (payload.ToBeDeleted?.Count ?? 0) == 0;

            if (empty) return new WithdrawalBulkResponseDto();

            return await _api.PatchAsync<WithdrawalBulkRequestDto, WithdrawalBulkResponseDto>("warehouse/withdrawals/bulk", payload).ConfigureAwait(false);
        }
    }
}
