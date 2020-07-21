using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class RecipeComponent
    {
        public int Id { get; set; }
        public Recipe Recipe { get; set; }
        public Component Component { get; set; }
        public int Order { get; set; }
        public float VolumeRate { get; set; }
        public string Unit { get; set; }
        public int Valve { get; set; }
        public string DispenserName { get; set; }
    }
}
