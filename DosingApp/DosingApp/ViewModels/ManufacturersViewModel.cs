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
    public class ManufacturersViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        private ObservableCollection<Manufacturer> manufacturers;
        private Manufacturer selectedManufacturer;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ManufacturersViewModel()
        {
            LoadManufacturers();

            CreateCommand = new Command(CreateManufacturer);
            DeleteCommand = new Command(DeleteManufacturer);
            SaveCommand = new Command(SaveManufacturer);
            BackCommand = new Command(Back);
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
                    ManufacturerViewModel tempManufacturer = new ManufacturerViewModel(value) { ManufacturersViewModel = this };
                    selectedManufacturer = null;
                    OnPropertyChanged(nameof(SelectedManufacturer));
                    Application.Current.MainPage.Navigation.PushAsync(new ManufacturerPage(tempManufacturer));
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
            Application.Current.MainPage.Navigation.PushAsync(new ManufacturerPage(new ManufacturerViewModel(new Manufacturer()) { ManufacturersViewModel = this }));
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
            LoadManufacturers();
            Back();
        }

        private void SaveManufacturer(object manufacturerInstance)
        {
            ManufacturerViewModel manufacturerViewModel = manufacturerInstance as ManufacturerViewModel;
            if (manufacturerViewModel.Manufacturer != null && manufacturerViewModel.IsValid)
            {
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
            LoadManufacturers();
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
        #endregion Methods
    }
}
