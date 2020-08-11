using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }

        public virtual ObservableCollection<FacilityTank> FacilityTanks { get; set; }
        //public virtual FacilityTank SelectedFacilityTank { get; set; }
    }
}
