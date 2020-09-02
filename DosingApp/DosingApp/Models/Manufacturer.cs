using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual List<Component> Components { get; set; }

        public string Icon
        { 
            get { return String.Equals(Name, Water.Name) ? "nav_right.png" : "folder.png"; } 
        }
    }
}
