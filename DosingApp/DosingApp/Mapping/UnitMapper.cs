using DosingApp.Models.AgroAssist.Data;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services.Import;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DosingApp.Mapping
{
    public class UnitMapper : IEntityMapper<UnitDto, Unit, int>
    {
        public int GetKeyFromDto(UnitDto dto) => dto.Id;

        public Expression<Func<Unit, int>> KeySelector => e => e.UnitId;

        public bool IsValid(UnitDto dto) => dto.Id > 0;

        public Unit CreateEntity(UnitDto dto) => new Unit {
            UnitId = dto.Id,
            GroupId = dto.GroupId,
            Name = dto.Name 
        };

        public void UpdateEntity(Unit entity, UnitDto dto) {
            entity.GroupId = dto.GroupId;
            entity.Name = dto.Name;
        }
    }
}
