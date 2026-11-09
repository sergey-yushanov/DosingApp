using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services.Import;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DosingApp.Mapping
{
    public class PlanInventoryMapper : IEntityMapper<PlanInventoryDto, PlanInventory, Guid>
    {
        private readonly Guid _parentTechOperationGuid;

        public PlanInventoryMapper(Guid parentTechOperationGuid)
            => _parentTechOperationGuid = parentTechOperationGuid;

        public Guid GetKeyFromDto(PlanInventoryDto dto) => dto.PlanInventoryGuid;

        public Expression<Func<PlanInventory, Guid>> KeySelector => e => e.PlanInventoryGuid;

        public bool IsValid(PlanInventoryDto dto) => dto.PlanInventoryGuid != Guid.Empty;

        public PlanInventory CreateEntity(PlanInventoryDto dto) => new PlanInventory
        {
            PlanInventoryGuid = dto.PlanInventoryGuid,
            TechOperationGuid = _parentTechOperationGuid,
            EntityGuid = dto.EntityGuid,
            EntityTypeId = dto.EntityTypeId,
            PlanAreaHectare = dto.PlanAreaHectare,
            Total = dto.Total,
            UnitId = dto.UnitId,
        };

        public void UpdateEntity(PlanInventory entity, PlanInventoryDto dto)
        {
            entity.EntityGuid = dto.EntityGuid;
            entity.EntityTypeId = dto.EntityTypeId;
            entity.PlanAreaHectare = dto.PlanAreaHectare;
            entity.Total = dto.Total;
            entity.UnitId = dto.UnitId;
        }
    }
}
