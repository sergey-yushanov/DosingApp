using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class FacilityTankViewModel : BaseViewModel
    {
        #region Services
        
        #endregion Services

        #region Attributes
        FacilityTanksViewModel facilityTanksViewModel;
        public FacilityTank FacilityTank { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public FacilityTankViewModel(FacilityTank facilityTank)
        {
            FacilityTank = facilityTank;
        }
        #endregion Constructor

        #region Properties
        public FacilityTanksViewModel FacilityTanksViewModel
        {
            get { return facilityTanksViewModel; }
            set { SetProperty(ref facilityTanksViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Объект: " + FacilityTanksViewModel.Facility.Name + ((FacilityTank.FacilityTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + FacilityTank.Name);
                return FacilityTank.Name;
            }
            set
            {
                if (FacilityTank.Name != value)
                {
                    FacilityTank.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Volume
        {
            get
            {
                if (FacilityTank.Volume == null)
                {
                    return "";
                }
                else
                {
                    return FacilityTank.Volume.ToString();
                }
            }
            set
            {
                try
                {
                    FacilityTank.Volume = float.Parse(value);
                }
                catch
                {
                    FacilityTank.Volume = null;
                }
                OnPropertyChanged(nameof(Volume));
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}
