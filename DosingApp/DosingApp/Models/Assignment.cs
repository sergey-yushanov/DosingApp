using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Recipe Recipe { get; set; }
        public string SourceTank { get; set; }
        public string DestTank { get; set; }
        public AgrYear AgrYear { get; set; }
        public Field Field { get; set; }
        public Object Object { get; set; }
        public float VolumeRate { get; set; }
    }
}
