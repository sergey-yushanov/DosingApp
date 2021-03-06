﻿using DosingApp.Models;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.ViewModels
{
    public class CropViewModel : BaseViewModel
    {
        #region Attributes
        CropsViewModel cropsViewModel;
        public Crop Crop { get; private set; }
        private string title;
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
            get 
            {
                Title = (Crop.CropId == 0) ? "Новая культура" : "Культура: " + Crop.Name;
                return Crop.Name; 
            }
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
