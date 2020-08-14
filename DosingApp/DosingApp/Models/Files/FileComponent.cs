using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Files
{
    public class FileComponent
    {
        [Index(0)]
        public string Name { get; set; }

        [Index(1)]
        public string Density { get; set; }
    }
}
