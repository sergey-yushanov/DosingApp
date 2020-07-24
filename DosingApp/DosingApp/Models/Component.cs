using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Consistency { get; set; }
        public float Density { get; set; }
        public string Packing { get; set; }
    }
}
