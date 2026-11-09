using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models.AgroAssist.Data
{
    public class PlanInventory
    {
        [Key]
        public Guid PlanInventoryGuid { get; set; } = Guid.NewGuid();
        public Guid? TechOperationGuid { get; set; }
        public Guid? EntityGuid { get; set; }
        public int? EntityTypeId { get; set; }
        public double? PlanAreaHectare { get; set; }
        public double? Total { get; set; }
        public int? UnitId { get; set; }
    }
}
