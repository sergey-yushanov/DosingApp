using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class TransportTankViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        TransportViewModel transportViewModel;
        public TransportTank Tank { get; private set; }
        #endregion Attributes

        #region Constructor
        public TransportTankViewModel(TransportTank tank)
        {
            Tank = tank;
        }
        #endregion Constructor

        #region Properties
        public TransportViewModel TransportViewModel
        {
            get { return transportViewModel; }
            set { SetProperty(ref transportViewModel, value); }
        }

        public string Name
        {
            get { return Tank.Name; }
            set
            {
                if (Tank.Name != value)
                {
                    Tank.Name = value;
                    OnPropertyChanged(nameof(Name));
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
