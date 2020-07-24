using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        //public int CropId { get; set; }
        public Crop Crop { get; set; }
        
        public ProcessingType ProcessingType { get; set; }
        public Component Carrier { get; set; }
        public string Note { get; set; }
        public float CarrierInit { get; set; }
        public float CarrierMin { get; set; }
    }
}
