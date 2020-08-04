using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class ApplicatorViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Applicator> dataServiceApplicators;
        #endregion Services

        #region Attributes
        ApplicatorsViewModel applicatorsViewModel;
        public Applicator Applicator { get; private set; }
        #endregion Attributes

        #region Constructor
        public ApplicatorViewModel(Applicator applicator)
        {
            Applicator = applicator;
        }
        #endregion Constructor

        #region Properties
        public ApplicatorsViewModel ApplicatorsViewModel
        {
            get { return applicatorsViewModel; }
            set { SetProperty(ref applicatorsViewModel, value); }
        }

        public string Name
        {
            get { return Applicator.Name; }
            set
            {
                if (Applicator.Name != value)
                {
                    Applicator.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Tank
        {
            get { return Applicator.Tank; }
            set
            {
                if (Applicator.Tank != value)
                {
                    Applicator.Tank = value;
                    OnPropertyChanged(nameof(Tank));
                }
            }
        }

        public float Volume
        {
            get { return Applicator.Volume; }
            set
            {
                if (Applicator.Volume != value)
                {
                    Applicator.Volume = value;
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
        #endregion Properties
    }
}
