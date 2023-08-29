using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DosingApp.Models.Screen
{
    public class JobComponentScreen : BaseViewModel
    {
        private double? dosedVolume;
        private double? dosedVolumeError;
        private string dosedVolumeInfo;
        private string dosedVolumeErrorInfo;

        private bool isDosedVolumeNull;
        private bool isDosedVolumeNotNull;
        private bool isDosedVolumeGood;
        private bool isDosedVolumeNotGood;

        public double? DosedVolume
        {
            get { return dosedVolume; }
            set
            {
                IsDosedVolumeNull = (value == 0);
                IsDosedVolumeNotNull = (value != 0);
                DosedVolumeInfo = String.Format("{0,12:N2} {1}", Convert.ToDouble(value), VolumeUnit);
                SetProperty(ref dosedVolume, value);
            }
        }

        public double? DosedVolumeError
        {
            get { return dosedVolumeError; }
            set 
            {
                IsDosedVolumeGood = (float)Math.Abs((decimal)value) < 0.05;
                IsDosedVolumeNotGood = (float)Math.Abs((decimal)value) >= 0.05 && (float)Math.Abs((decimal)value) < 1;
                DosedVolumeErrorInfo = String.Format("{0,12:P2}", Convert.ToDouble(value));
                SetProperty(ref dosedVolumeError, value); 
            }
        }

        public string DosedVolumeInfo
        {
            get { return dosedVolumeInfo; }
            set { SetProperty(ref dosedVolumeInfo, value); }
        }

        public string DosedVolumeErrorInfo
        {
            get { return dosedVolumeErrorInfo; }
            set { SetProperty(ref dosedVolumeErrorInfo, value); }
        }

        public bool IsDosedVolumeNull
        {
            get { return isDosedVolumeNull; }
            set { SetProperty(ref isDosedVolumeNull, value); }
        }

        public bool IsDosedVolumeNotNull
        {
            get { return isDosedVolumeNotNull; }
            set { SetProperty(ref isDosedVolumeNotNull, value); }
        }

        public bool IsDosedVolumeGood
        {
            get { return isDosedVolumeGood; }
            set { SetProperty(ref isDosedVolumeGood, value); }
        }

        public bool IsDosedVolumeNotGood
        {
            get { return isDosedVolumeNotGood; }
            set { SetProperty(ref isDosedVolumeNotGood, value); }
        }

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

        //public string DosedVolumeInfo; // { get { return String.Format("{0,12:N2} {1}", Convert.ToDouble(DosedVolume), VolumeUnit); } }
        //public string DosedVolumeErrorInfo { get { return String.Format("{0,12:P2}", Convert.ToDouble(DosedVolumeError)); } }

        public bool IsNotDry { get { return Dispenser != DispenserSuffix.Dry; } }
        public bool IsVisible { get; set; }

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

            if ((Dispenser == DispenserSuffix.Carrier) || (Dispenser == DispenserSuffix.Dry))
            {
                //IsVisible = false;
                IsVisible = true;
            }
            else
            {
                IsVisible = true;
            }
        }

        public void Update(CommonScreen common, CollectorScreen collector, SingleDosScreen singleDos)
        {
            if (Dispenser == DispenserSuffix.Dry)
            {
                DosedVolume = 0;
                return;
            }

            if (Dispenser == DispenserSuffix.Carrier)
            {
                DosedVolume = (double?)common.CarrierDosedVolume;
            }

            if (Dispenser.IndexOf(DispenserSuffix.Collector) >= 0)
            {
                DosedVolume = collector.DosedVolumes[GetDispenserNumber() - 1];
            }

            if (Dispenser.IndexOf(DispenserSuffix.Single) >= 0)
            {
                DosedVolume = singleDos.DosedVolume;
            }

            DosedVolumeError = (double?)((DosedVolume - Volume) / Volume);
        }

        public void Update(CommonScreen common, List<CollectorScreen> collectors, VolumeDosScreen volumeDos)
        {
            if (Dispenser == DispenserSuffix.Dry)
            {
                DosedVolume = 0;
                return;
            }

            if (Dispenser == DispenserSuffix.Carrier)
            {
                DosedVolume = (double?)common.CarrierDosedVolume;
            }

            if (Dispenser.IndexOf(DispenserSuffix.Collector) >= 0)
            {
                int collectorIndex = (int)Char.GetNumericValue(Dispenser[0]) - 1;
                DosedVolume = collectors[collectorIndex].DosedVolumes[GetDispenserNumber() - 1];
            }

            if (Dispenser.IndexOf(DispenserSuffix.Volume) >= 0)
            {
                DosedVolume = volumeDos.DosedVolume;
            }

            DosedVolumeError = (double?)((DosedVolume - Volume) / Volume);
        }

        //public int GetCollectorNumber()
        //{
        //    return 1;
        //}

        //public int GetSingleNumber()
        //{
        //    return 1;
        //}

        public int GetDispenserNumber()
        {
            char number = Dispenser[Dispenser.Length - 1];
            return Int32.Parse(number.ToString());
        }
    }
}
