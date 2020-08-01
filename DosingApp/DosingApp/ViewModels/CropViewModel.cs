using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class CropViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Crop> dataServiceCrops;
        #endregion Services

        #region Attributes
        CropsViewModel cropsViewModel;
        public Crop Crop { get; private set; }
        #endregion Attributes

        #region Constructor
        public CropViewModel(Crop crop)
        {
            Crop = crop;
        }
        #endregion Constructor

        #region Properties
        public CropsViewModel CropsViewModel
        {
            get { return cropsViewModel; }
            set { SetProperty(ref cropsViewModel, value); }
        }

        public string Name
        {
            get { return Crop.Name; }
            set
            {
                if (Crop.Name != value)
                {
                    Crop.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Code
        {
            get { return Crop.Code; }
            set
            {
                if (Crop.Code != value)
                {
                    Crop.Code = value;
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
