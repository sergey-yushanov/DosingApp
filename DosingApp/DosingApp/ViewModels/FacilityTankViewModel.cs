using DosingApp.Models;

namespace DosingApp.ViewModels
{
    public class FacilityTankViewModel : BaseViewModel
    {
        #region Attributes
        FacilityViewModel facilityViewModel;
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
        public FacilityViewModel FacilityViewModel
        {
            get { return facilityViewModel; }
            set { SetProperty(ref facilityViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Объект: " + FacilityViewModel.Facility.Name + ((FacilityTank.FacilityTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + FacilityTank.Name);
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

        public double? Volume
        {
            get { return FacilityTank.Volume; }
            set
            {
                if (FacilityTank.Volume != value)
                {
                    FacilityTank.Volume = value;
                    OnPropertyChanged(nameof(Volume));
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

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}
