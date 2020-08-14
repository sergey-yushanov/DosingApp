using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public static class Water
    {
        public const string Name = "Вода";
        public const string Consistency = ComponentConsistency.Liquid;
        public const float Density = 1.0F;
    }

    public static class ComponentConsistency
    {
        public const string Dry = "Сухой";
        public const string Liquid = "Жидкий";
    }

    public class Component
    {
        public int ComponentId { get; set; }

        public int? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        public string Name { get; set; }
        public string Consistency { get; set; }
        public float? Density { get; set; }
        public string Packing { get; set; }
    }
}
