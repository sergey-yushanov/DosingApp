using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
    public class RecipeComponent
    {
        public int RecipeComponentId { get; set; }
        
        public int? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public int? ComponentId { get; set; }
        public virtual Component Component { get; set; }

        public int? Order { get; set; }
        public double? VolumeRate { get; set; }
        public string VolumeRateUnit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get { return Component?.Name; } }

        // проверяем, является ли клапан четвертым в коллекторе - он всегда вода
        public bool IsFourthValve()
        {
            string fourthValve = DispenserSuffix.Collector + "4";
            int index = Dispenser != null ? Dispenser.IndexOf(fourthValve) : -1;
            return index != -1;
        }

        public string DryComponentDispenser { get { return "Загр. вручную"; } }
    }
}
