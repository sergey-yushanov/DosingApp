using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class ApplicatorTankViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        ApplicatorTanksViewModel applicatorTanksViewModel;
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
        public ApplicatorTanksViewModel ApplicatorTanksViewModel
        {
            get { return applicatorTanksViewModel; }
            set { SetProperty(ref applicatorTanksViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = "Аппликатор: " + ApplicatorTanksViewModel.Applicator.Name + ((ApplicatorTank.ApplicatorTankId == 0) ? "\nНовая емкость" : "\nЕмкость: " + ApplicatorTank.Name);
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

        public float? Volume
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
