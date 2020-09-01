using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public static class Water
    {
        public const string Name = "Вода";
        public const string Consistency = ComponentConsistency.Liquid;
        public const double Density = 1.0;

        public static Manufacturer GetManufacturer()
        {
            Manufacturer manufacturer = new Manufacturer();
            manufacturer.Name = Name;
            return manufacturer;
        }

        public static Component GetComponent()
        {
            Component component = new Component();
            component.Name = Name;
            component.Consistency = Consistency;
            component.Density = Density;
            component.Manufacturer = GetManufacturer();
            return component;
        }
    }

    public static class ComponentConsistency
    {
        public const string Dry = "Сухой";
        public const string Liquid = "Жидкий";

        public static List<string> GetList()
        {
            return new List<string>() { Liquid, Dry };
        }
    }

    public static class VolumeUnit
    {
        public const string Dry = "кг";
        public const string Liquid = "л";

        public static List<string> GetList()
        {
            return new List<string>() { Liquid, Dry };
        }
    }

    public static class VolumeRateUnit
    {
        public const string Dry = "кг/га";
        public const string Liquid = "л/га";

        public static List<string> GetList()
        {
            return new List<string>() { Liquid, Dry };
        }
    }

    public class Component
    {
        public int ComponentId { get; set; }

        public int? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        public string Name { get; set; }
        public string Consistency { get; set; }
        public double? Density { get; set; }
        public string Packing { get; set; }

        public bool IsLiquid()
        {
            return String.Equals(Consistency, ComponentConsistency.Liquid);
        }

        public override string ToString()
        {
            return Name;
        }

        public string Icon { get { return "chevron_right.png"; } }
    }
}
