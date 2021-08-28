using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DosingApp.Models.Screen
{
    public class JobComponentScreen : BaseViewModel
    {
        public double? Volume { get; set; }
        public double? VolumeRate { get; set; }
        public string VolumeUnit { get; set; }
        public string VolumeRateUnit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get; set; }
        public string VolumeInfo { get; set; }
        public string VolumeRateInfo { get; set; }
        public string ConsistencyInfo { get; set; }
        public string DispenserInfo { get; set; }

        public double? DosedVolume { get; set; }
        public double? DosedVolumeError { get; set; }

        public string DosedVolumeInfo { get { return String.Format("{0,12:N2} {1}", Convert.ToDouble(DosedVolume), VolumeUnit); } }
        public string DosedVolumeErrorInfo { get { return String.Format("{0,12:P2}", Convert.ToDouble(DosedVolumeError)); } }

        public bool IsNotDry { get { return Dispenser != DispenserSuffix.Dry; } }

        public JobComponentScreen(JobComponent jobComponent)
        {
            Volume = jobComponent.Volume;
            VolumeRate = jobComponent.VolumeRate;
            VolumeUnit = jobComponent.VolumeUnit;
            VolumeRateUnit = jobComponent.VolumeRateUnit;
            Dispenser = jobComponent.Dispenser;

            Name = jobComponent.Name;
            VolumeInfo = jobComponent.VolumeInfo;
            VolumeRateInfo = jobComponent.VolumeRateInfo;
            ConsistencyInfo = jobComponent.ConsistencyInfo;
            DispenserInfo = jobComponent.DispenserInfo;
        }

        public void Update(CommonScreen common, CollectorScreen collector)
        {
            if (Dispenser == DispenserSuffix.Dry)
            {
                DosedVolume = 0;
                return;
            }

            if (Dispenser == DispenserSuffix.Carrier)
                DosedVolume = common.CarrierDosedVolume;
            else
                DosedVolume = collector.DosedVolumes[GetDispenserNumber() - 1];

            DosedVolumeError = (Volume - DosedVolume) / Volume;
        }

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
