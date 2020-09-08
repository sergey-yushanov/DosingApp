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
    public class GroupedComponentsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Manufacturer> manufacturers;
        private Manufacturer selectedManufacturer;

        public ICommand CreateManufacturerCommand { get; protected set; }
        public ICommand DeleteManufacturerCommand { get; protected set; }
        public ICommand SaveManufacturerCommand { get; protected set; }

        public bool IsEditMode { get; protected set; }
        RecipeViewModel RecipeViewModel;
        RecipeComponentViewModel RecipeComponentViewModel;
        #endregion Attributes

        #region Constructor
        public GroupedComponentsViewModel(bool isEditMode, RecipeViewModel recipeViewModel, RecipeComponentViewModel recipeComponentViewModel)
        {
            IsEditMode = isEditMode;
            RecipeViewModel = recipeViewModel;
            RecipeComponentViewModel = recipeComponentViewModel;

            LoadManufacturers();
            CreateManufacturerCommand = new Command(CreateManufacturer);
            DeleteManufacturerCommand = new Command(DeleteManufacturer);
            SaveManufacturerCommand = new Command(SaveManufacturer);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set { SetProperty(ref manufacturers, value); }
        }

        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                if (selectedManufacturer != value)
                {
                    if (!String.Equals(value.Name, Water.Name))
                    {
                        ComponentsViewModel tempComponentsViewModel = new ComponentsViewModel(value, IsEditMode, RecipeViewModel, RecipeComponentViewModel);
                        selectedManufacturer = null;
                        OnPropertyChanged(nameof(SelectedManufacturer));
                        Application.Current.MainPage.Navigation.PushAsync(new ComponentsPage(tempComponentsViewModel));
                    }
                    else
                    {
                        if (IsEditMode)
                        {
                            selectedManufacturer = null;
                        }
                        else
                        { 
                            if (RecipeViewModel != null)
                            {
                                RecipeViewModel.Carrier = GetWaterComponent();
                            }
                            if (RecipeComponentViewModel != null)
                            {
                                RecipeComponentViewModel.Component = GetWaterComponent();
                            }
                            Back();
                        }
                    }
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateManufacturer()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManufacturerPage(new ManufacturerViewModel(new Manufacturer()) { GroupedComponentsViewModel = this }));
        }

        private void DeleteManufacturer(object manufacturerInstance)
        {
            ManufacturerViewModel manufacturerViewModel = manufacturerInstance as ManufacturerViewModel;
            if (manufacturerViewModel.Manufacturer != null && manufacturerViewModel.Manufacturer.ManufacturerId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Components.RemoveRange(manufacturerViewModel.Components);
                    db.Manufacturers.Remove(manufacturerViewModel.Manufacturer);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveManufacturer(object manufacturerInstance)
        {
            ManufacturerViewModel manufacturerViewModel = manufacturerInstance as ManufacturerViewModel;
            if (manufacturerViewModel.Manufacturer != null)
            {
                if (!manufacturerViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название производителя", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (manufacturerViewModel.Manufacturer.ManufacturerId == 0)
                    {
                        db.Entry(manufacturerViewModel.Manufacturer).State = EntityState.Added;
                    }
                    else
                    {
                        db.Manufacturers.Update(manufacturerViewModel.Manufacturer);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadManufacturers()
        {
            using (AppDbContext db = App.GetContext())
            {
                Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.ToList());
            }
        }

        private Component GetWaterComponent()
        {
            using (AppDbContext db = App.GetContext())
            {
                return db.Components.FirstOrDefault(c => c.Name == Water.Name);
            }
        }
        #endregion Methods
    }
}
