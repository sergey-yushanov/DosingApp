using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public static class RecipeComponentUnit
    {
        public const string Dry = "кг/га";
        public const string Liquid = "л/га";
    }

    public class RecipeComponent
    {
        public int RecipeComponentId { get; set; }
        
        public int? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public int? ComponentId { get; set; }
        public virtual Component Component { get; set; }

        public int? Order { get; set; }
        public double? VolumeRate { get; set; }
        public string Unit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get { return Component?.Name; } }
    }
}
