using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
    public static class FacilityType
    {
        public const string Storage = "Склад";
        public const string Shipment = "Отгрузка";
        public const string Own = "Свой вариант";

        public static List<string> GetList()
        {
            return new List<string>() { Storage, Shipment, Own };
        }

        public static List<string> GetNotOwnList()
        {
            return new List<string>() { Storage, Shipment };
        }
    }

    public class Facility
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }

        public virtual List<FacilityTank> FacilityTanks { get; set; }
    }
}
