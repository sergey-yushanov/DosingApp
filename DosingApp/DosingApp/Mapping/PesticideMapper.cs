using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services.Import;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DosingApp.Mapping
{
    public class PesticideMapper : IEntityMapper<PesticideDto, Pesticide, Guid>
    {
        public Guid GetKeyFromDto(PesticideDto dto) => dto.PesticideGuid;

        public Expression<Func<Pesticide, Guid>> KeySelector => e => e.PesticideGuid;

        public bool IsValid(PesticideDto dto) => dto.PesticideGuid != Guid.Empty;

        public Pesticide CreateEntity(PesticideDto dto) => new Pesticide
        {
            PesticideGuid = dto.PesticideGuid,
            Name = dto.Name ?? string.Empty,
            Description = dto.Description,
            UnitId = dto.UnitId,
        };

        public void UpdateEntity(Pesticide entity, PesticideDto dto)
        {
            entity.Name = dto.Name ?? string.Empty;
            entity.Description = dto.Description;
            entity.UnitId = dto.UnitId;
        }
    }
}
