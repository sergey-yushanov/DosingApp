using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class AgrYearViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<AgrYear> dataServiceAgrYears;
        #endregion Services

        #region Attributes
        AgrYearsViewModel agrYearsViewModel;
        public AgrYear AgrYear { get; private set; }
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
            get { return AgrYear.Name; }
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
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }
        #endregion Properties
    }
}
