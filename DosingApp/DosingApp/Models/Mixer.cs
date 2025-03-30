using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DosingApp.Models
{
    public static class DispenserSuffix
    {
        public const string Collector = "К";
        public const string Volume = "ОбД";
        public const string Single = "ОД";
        public const string ThreeWay = "ТХК";
        public const string Carrier = "Носитель";
        public const string Dry = "Загр. вручную";
        public const string Powder = "ПД";
        public const string Valve = "Кл";
    }

    public class Mixer
    {
        public int MixerId { get; set; }
        public string Name { get; set; }

        public int? Collector { get; set; }
        public int? Volume { get; set; }
        public int? Single { get; set; }
        public int? ThreeWay { get; set; }
        public int? Powder { get; set; }

        public bool IsUsedMixer { get; set; }
        public string Url { get; set; }

        public bool IsAirTemperatureSensor { get; set; }

        // ограничение по количеству дозаторов
        public const int MaxCollectors = 4;
        public const int MaxVolumes = 1;
        public const int MaxPowders = 1;

        public List<string> GetDispensers()
        {
            var dispensers = new List<string>();
            dispensers.AddRange(GetCollectorDispensers());
            //dispensers.AddRange(GetSingleDispensers());
            dispensers.AddRange(GetVolumeDispensers());
            //dispensers.AddRange(GetThreeWayDispensers());
            dispensers.AddRange(GetPowderDispensers());
            return dispensers;
        }

        public List<string> GetCollectorDispensers()
        {
            var dispensers = new List<string>();
            if (Collector != null)
            {
                for (int c = 1; c <= Collector; c++)
                for (int v = 1; v <= 4; v++) // в коллекторе 4 клапана
                {
                    dispensers.Add(c.ToString() + DispenserSuffix.Collector + v.ToString() + DispenserSuffix.Valve);
                }
            }
            return dispensers;
        }

        //public List<string> GetSingleDispensers()
        //{
        //    var dispensers = new List<string>();
        //    if (Single != null)
        //    {
        //        for (int s = 1; s <= Single; s++)
        //        {
        //            dispensers.Add(DispenserSuffix.Single + s.ToString());
        //        }
        //    }
        //    return dispensers;
        //}

        public List<string> GetVolumeDispensers()
        {
            var dispensers = new List<string>();
            if (Volume != null)
            {
                for (int s = 1; s <= Volume; s++)
                {
                    dispensers.Add(DispenserSuffix.Volume + s.ToString());
                }
            }
            return dispensers;
        }

        //public List<string> GetThreeWayDispensers()
        //{
        //    var dispensers = new List<string>();
        //    if (ThreeWay != null)
        //    {
        //        for (int t = 1; t <= ThreeWay; t++)
        //        {
        //            dispensers.Add(t.ToString() + DispenserSuffix.ThreeWay + "1"); // у клапана 2 хода
        //            dispensers.Add(t.ToString() + DispenserSuffix.ThreeWay + "2");
        //        }
        //    }
        //    return dispensers;
        //}

        public List<string> GetPowderDispensers()
        {
            var dispensers = new List<string>();
            if (Powder != null)
            {
                for (int s = 1; s <= Powder; s++)
                {
                    dispensers.Add(DispenserSuffix.Powder + s.ToString());
                }
            }
            return dispensers;
        }
    }
}
