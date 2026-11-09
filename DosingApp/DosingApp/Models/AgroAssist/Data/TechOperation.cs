using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models.AgroAssist.Data
{
    public class TechOperation
    {
        [Key]
        public Guid TechOperationGuid { get; set; } = Guid.NewGuid();
        public Guid? FieldGuid { get; set; }
        public Guid? StructureGuid { get; set; }
        public double? PlanAreaHectare { get; set; }
        public double? FactAreaHectare { get; set; }
        public int? SeqId { get; set; }
        public int? SeasonId { get; set; }
    }
}
