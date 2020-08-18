using DosingApp.Models;

namespace DosingApp.ViewModels
{
    public class ApplicatorTankViewModel : BaseViewModel
    {
        #region Attributes
        ApplicatorViewModel applicatorViewModel;
        public ApplicatorTank ApplicatorTank { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public ApplicatorTankViewModel(ApplicatorTank applicatorTank)
        {
            ApplicatorTank = applicatorTank;
        }
        #endregion Constructor

        #region Properties
        public ApplicatorViewModel ApplicatorViewModel
        {
            get { return applicatorViewModel; }
            set { SetProperty(ref applicatorViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Аппликатор: " + ApplicatorViewModel.Applicator.Name + ((ApplicatorTank.ApplicatorTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + ApplicatorTank.Name);
                return ApplicatorTank.Name;
            }
            set
            {
                if (ApplicatorTank.Name != value)
                {
                    ApplicatorTank.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public double? Volume
        {
            get { return ApplicatorTank.Volume; }
            set
            {
                if (ApplicatorTank.Volume != value)
                {
                    ApplicatorTank.Volume = value;
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
