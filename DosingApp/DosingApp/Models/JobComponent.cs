using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace DosingApp.Models
{
    public class JobComponent
    {
        public int JobComponentId { get; set; }
        
        public int? JobId { get; set; }
        public virtual Job Job { get; set; }

        public int? ComponentId { get; set; }
        public virtual Component Component { get; set; }

        public int? Order { get; set; }
        public double? Volume { get; set; }
        public double? VolumeRate { get; set; }
        public string VolumeUnit { get; set; }
        public string VolumeRateUnit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get { return Component?.Name; } }
        //public string VolumeInfo { get { return ((double)Volume).ToString("N", CultureInfo.CreateSpecificCulture("ru-RU")) + " " + VolumeUnit; } }
        public string VolumeInfo { get { return String.Format("{0,12:N2} {1}", (double)Volume, VolumeUnit); } }
        public string VolumeRateInfo { get { return "Норма расхода: " + ((double)VolumeRate).ToString("N2", CultureInfo.CreateSpecificCulture("ru-RU")) + " " + VolumeRateUnit; } }
        public string ConsistencyInfo { get { return "Форма: " + Component?.Consistency; } }
        public string DispenserInfo { get { return "Дозатор: " + Dispenser; } }

        // new
        //public double? DosedVolume { get; set; }
        //public double? ErrorVolume { get; set; }
        //public string DosedVolumeInfo { get { return ((double)DosedVolume).ToString("N", CultureInfo.CreateSpecificCulture("ru-RU")) + " " + VolumeUnit; } }

        public int GetCollectorNumber()
        {
            return 1;
        }

        public int GetDispenserNumber()
        {
            char number = Dispenser[Dispenser.Length - 1];
            return Int32.Parse(number.ToString());
        }
    }
}
