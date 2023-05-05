using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public static class DispenserSuffix
    {
        public const string Collector = "Кол";
        public const string Volume = "ОД";
        public const string Single = "ОД";
        public const string ThreeWay = "ТХК";
        public const string Carrier = "Носитель";
        public const string Dry = "Загр. вручную";
    }

    public class Mixer
    {
        public int MixerId { get; set; }
        public string Name { get; set; }

        public int? Collector { get; set; }
        public int? Single { get; set; }
        public int? ThreeWay { get; set; }

        public bool IsUsedMixer { get; set; }
        //public string UUID { get; set; }

        public List<string> GetDispensers()
        {
            var dispensers = new List<string>();
            dispensers.AddRange(GetCollectorDispensers());
            //dispensers.AddRange(GetSingleDispensers());
            dispensers.AddRange(GetVolumeDispensers());
            dispensers.AddRange(GetThreeWayDispensers());
            return dispensers;
        }

        public List<string> GetCollectorDispensers()
        {
            var dispensers = new List<string>();
            if (Collector != null)
            {
                for (int c = 1; c <= Collector; c++)
                for (int v = 1; v <= 3; v++) // в коллекторе 3 клапана
                {
                    dispensers.Add(c.ToString() + DispenserSuffix.Collector + v.ToString());
                }
            }
            return dispensers;
        }

        public List<string> GetSingleDispensers()
        {
            var dispensers = new List<string>();
            if (Single != null)
            {
                for (int s = 1; s <= Single; s++)
                {
                    dispensers.Add(DispenserSuffix.Single + s.ToString());
                }
            }
            return dispensers;
        }

        public List<string> GetVolumeDispensers()
        {
            var dispensers = new List<string>();
            if (Single != null)
            {
                for (int s = 1; s <= Single; s++)
                {
                    dispensers.Add(DispenserSuffix.Volume + s.ToString());
                }
            }
            return dispensers;
        }

        public List<string> GetThreeWayDispensers()
        {
            var dispensers = new List<string>();
            if (ThreeWay != null)
            {
                for (int t = 1; t <= ThreeWay; t++)
                {
                    dispensers.Add(t.ToString() + DispenserSuffix.ThreeWay + "1"); // у клапана 2 хода
                    dispensers.Add(t.ToString() + DispenserSuffix.ThreeWay + "2");
                }
            }
            return dispensers;
        }
    }
}
