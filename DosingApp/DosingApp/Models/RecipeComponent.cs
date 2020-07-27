using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class RecipeComponent
    {
        public int RecipeComponentId { get; set; }
        
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int ComponentId { get; set; }
        public Component Component { get; set; }

        public int Order { get; set; }
        public float VolumeRate { get; set; }
        public string Unit { get; set; }
        public int Valve { get; set; }
        public string DispenserName { get; set; }
    }
}
