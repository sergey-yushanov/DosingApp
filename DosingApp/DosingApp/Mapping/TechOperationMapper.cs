using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services.Import;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DosingApp.Mapping
{
    public class TechOperationMapper : IEntityMapper<TechOperationDto, TechOperation, Guid>
    {
        public Guid GetKeyFromDto(TechOperationDto dto) => dto.TechoperationGuid;

        public Expression<Func<TechOperation, Guid>> KeySelector => e => e.TechOperationGuid;

        public bool IsValid(TechOperationDto dto) => dto.TechoperationGuid != Guid.Empty;

        public TechOperation CreateEntity(TechOperationDto dto) => new TechOperation
        {
            TechOperationGuid = dto.TechoperationGuid,
            FieldGuid = dto.FieldGuid,
            StructureGuid = dto.StructureGuid,
            PlanAreaHectare = dto.PlanAreaHectare,
            FactAreaHectare = dto.FactAreaHectare,
            SeqId = dto.SeqId,
            SeasonId = dto.SeasonId
        };

        public void UpdateEntity(TechOperation entity, TechOperationDto dto)
        {
            entity.FieldGuid = dto.FieldGuid;
            entity.StructureGuid = dto.StructureGuid;
            entity.PlanAreaHectare = dto.PlanAreaHectare;
            entity.FactAreaHectare = dto.FactAreaHectare;
            entity.SeqId = dto.SeqId;
            entity.SeasonId = dto.SeasonId;
        }
    }
}
