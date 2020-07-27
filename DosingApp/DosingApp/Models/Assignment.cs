using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        
        public string SourceTank { get; set; }
        public string DestTank { get; set; }
        
        public int AgrYearId { get; set; }
        public AgrYear AgrYear { get; set; }

        public int FieldId { get; set; }
        public Field Field { get; set; }

        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        public float VolumeRate { get; set; }
    }
}
