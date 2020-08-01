using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models
{
    public class Field
    {
        public int FieldId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
