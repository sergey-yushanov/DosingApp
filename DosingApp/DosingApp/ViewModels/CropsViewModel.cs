using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class CropsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Crop> crops;
        private Crop selectedCrop;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public CropsViewModel()
        {
            //LoadCrops();

            CreateCommand = new Command(CreateCrop);
            DeleteCommand = new Command(DeleteCrop);
            SaveCommand = new Command(SaveCrop);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Crop> Crops 
        {
            get { return crops; }
            set { SetProperty(ref crops, value); }
        }

        public Crop SelectedCrop
        {
            get { return selectedCrop; }
            set
            {
                if (selectedCrop != value)
                {
                    CropViewModel tempCrop = new CropViewModel(value) { CropsViewModel = this };
                    selectedCrop = null;
                    OnPropertyChanged(nameof(SelectedCrop));
                    Application.Current.MainPage.Navigation.PushAsync(new CropPage(tempCrop));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateCrop()
        {
            Application.Current.MainPage.Navigation.PushAsync(new CropPage(new CropViewModel(new Crop()) { CropsViewModel = this }));
        }

        private void DeleteCrop(object cropInstance)
        {
            CropViewModel cropViewModel = cropInstance as CropViewModel;
            if (cropViewModel.Crop != null && cropViewModel.Crop.CropId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Crops.Remove(cropViewModel.Crop);
                    db.SaveChanges();
                }
            }
            //LoadCrops();
            Back();
        }

        private void SaveCrop(object cropInstance)
        {
            CropViewModel cropViewModel = cropInstance as CropViewModel;
            if (cropViewModel.Crop != null && cropViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (cropViewModel.Crop.CropId == 0)
                    {
                        db.Entry(cropViewModel.Crop).State = EntityState.Added;
                    }
                    else
                    {
                        db.Crops.Update(cropViewModel.Crop);
                    }
                    db.SaveChanges();
                }
            }
            //LoadCrops();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadCrops()
        {
            using (AppDbContext db = App.GetContext())
            {
                Crops = new ObservableCollection<Crop>(db.Crops.ToList());
            }
        }
        #endregion Methods

    }
}
