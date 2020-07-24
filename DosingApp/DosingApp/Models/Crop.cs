using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Crop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
