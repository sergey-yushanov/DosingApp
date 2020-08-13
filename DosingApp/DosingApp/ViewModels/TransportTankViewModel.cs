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
        TransportTanksViewModel transportTanksViewModel;
        public TransportTank TransportTank { get; private set; }
        private string title;
        #endregion Attributes

        #region Constructor
        public TransportTankViewModel(TransportTank transportTank)
        {
            TransportTank = transportTank;
        }
        #endregion Constructor

        #region Properties
        public TransportTanksViewModel TransportTanksViewModel
        {
            get { return transportTanksViewModel; }
            set { SetProperty(ref transportTanksViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Транспорт: " + TransportTanksViewModel.Transport.Name + ((TransportTank.TransportTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + TransportTank.Name);
                return TransportTank.Name;
            }
            set
            {
                if (TransportTank.Name != value)
                {
                    TransportTank.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public float? Volume
        {
            get { return TransportTank.Volume; }
            set
            {
                if (TransportTank.Volume != value)
                {
                    TransportTank.Volume = value;
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
