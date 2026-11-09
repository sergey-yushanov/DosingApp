using DosingApp.DataContext;
using DosingApp.Mapping;
using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
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
    public class PesticideImportService
    {
        private readonly ApiService _api;
        private readonly GenericUpsertService<PesticideDto, Pesticide, Guid> _upsert;

        public PesticideImportService(ApiService api, AppDbContext db)
        {
            _api = api;
            _upsert = new GenericUpsertService<PesticideDto, Pesticide, Guid>(db, new PesticideMapper());
        }

        public async Task<PesticidesListResponseDto> LoadAllAsync(IProgress<int> progress = null, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var dtos = await _api.GetAsync<PesticidesListResponseDto>("dictionaries/pesticides/list?is_default=false").ConfigureAwait(false);
            dtos.AddRange(await _api.GetAsync<PesticidesListResponseDto>("dictionaries/pesticides/list?is_default=true").ConfigureAwait(false));

            if (dtos == null || dtos.Count == 0) return new PesticidesListResponseDto();
            await _upsert.SaveAsync(dtos, ct).ConfigureAwait(false);
            return dtos;
        }
    }
}
