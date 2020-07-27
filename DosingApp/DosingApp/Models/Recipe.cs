using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        
        public int CropId { get; set; }
        public Crop Crop { get; set; }
        
        public int ProcessingTypeId { get; set; }
        public ProcessingType ProcessingType { get; set; }

        public int CarrierId { get; set; }
        public Component Carrier { get; set; }

        public string Note { get; set; }
        public float ValueInit { get; set; }
        public float ValueMin { get; set; }
    }
}
