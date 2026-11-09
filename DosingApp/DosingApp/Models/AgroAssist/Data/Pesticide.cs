using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models.AgroAssist.Data
{
    public class Pesticide
    {
        [Key]
        public Guid PesticideGuid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
