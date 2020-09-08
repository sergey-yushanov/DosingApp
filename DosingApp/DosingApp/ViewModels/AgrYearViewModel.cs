using DosingApp.Models;

namespace DosingApp.ViewModels
{
    public class AgrYearViewModel : BaseViewModel
    {
        #region Attributes
        AgrYearsViewModel agrYearsViewModel;
        public AgrYear AgrYear { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public AgrYearViewModel(AgrYear agrYear)
        {
            AgrYear = agrYear;
        }
        #endregion Constructor

        #region Properties
        public AgrYearsViewModel AgrYearsViewModel
        {
            get { return agrYearsViewModel; }
            set { SetProperty(ref agrYearsViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (AgrYear.AgrYearId == 0) ? "Новый сельхоз. год" : "Сельхоз. год: " + AgrYear.Name;
                return AgrYear.Name; 
            }
            set
            {
                if (AgrYear.Name != value)
                {
                    AgrYear.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string FinishDate
        {
            get { return AgrYear.FinishDate; }
            set
            {
                if (AgrYear.FinishDate != value)
                {
                    AgrYear.FinishDate = value;
                    OnPropertyChanged(nameof(FinishDate));
                }
            }
        }

        public bool IsValid
        {
            get { return (!string.IsNullOrEmpty(Name)); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties
    }
}
