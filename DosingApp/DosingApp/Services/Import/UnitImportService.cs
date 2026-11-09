using DosingApp.DataContext;
using DosingApp.Mapping;
using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class UnitImportService
    {
        private readonly ApiService _api;
        private readonly GenericUpsertService<UnitDto, Unit, int> _upsert;

        public UnitImportService(ApiService api, AppDbContext db)
        {
            _api = api;
            _upsert = new GenericUpsertService<UnitDto, Unit, int>(db, new UnitMapper());
        }

        public async Task<int> LoadAllAsync(IProgress<int> progress = null, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var dtos = await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=1").ConfigureAwait(false);
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=2").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=3").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=4").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=5").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=6").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=7").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=8").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=9").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=10").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=11").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=12").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=13").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=14").ConfigureAwait(false));
            dtos.AddRange(await _api.GetAsync<UnitsListResponseDto>("dictionaries/units?group_id=15").ConfigureAwait(false));

            if (dtos == null || dtos.Count == 0) return 0;

            var saved = await _upsert.SaveAsync(dtos, ct).ConfigureAwait(false);
            progress?.Report(saved);
            return saved;
        }
    }
}
