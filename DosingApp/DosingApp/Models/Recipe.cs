using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        
        public int? CropId { get; set; }
        public virtual Crop Crop { get; set; }
        
        public int? ProcessingTypeId { get; set; }
        public virtual ProcessingType ProcessingType { get; set; }

        public int? CarrierId { get; set; }
        public virtual Component Carrier { get; set; }

        public string Note { get; set; }
        public float ValueInit { get; set; }
        public float ValueMin { get; set; }

        public ICollection<RecipeComponent> RecipeComponents { get; set; }
    }
}
