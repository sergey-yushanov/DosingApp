using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class FacilityViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Facility> dataServiceFacilities;
        #endregion Services

        #region Attributes
        FacilitiesViewModel facilitiesViewModel;
        public Facility Facility { get; private set; }
        #endregion Attributes

        #region Constructor
        public FacilityViewModel(Facility facility)
        {
            Facility = facility;
        }
        #endregion Constructor

        #region Properties
        public FacilitiesViewModel FacilitiesViewModel
        {
            get { return facilitiesViewModel; }
            set { SetProperty(ref facilitiesViewModel, value); }
        }

        public string Name
        {
            get { return Facility.Name; }
            set
            {
                if (Facility.Name != value)
                {
                    Facility.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Type
        {
            get { return Facility.Type; }
            set
            {
                if (Facility.Type != value)
                {
                    Facility.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public string Address
        {
            get { return Facility.Address; }
            set
            {
                if (Facility.Address != value)
                {
                    Facility.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public string Tank
        {
            get { return Facility.Tank; }
            set
            {
                if (Facility.Tank != value)
                {
                    Facility.Tank = value;
                    OnPropertyChanged(nameof(Tank));
                }
            }
        }

        public string Code
        {
            get { return Facility.Code; }
            set
            {
                if (Facility.Code != value)
                {
                    Facility.Code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }
        #endregion Properties
    }
}
