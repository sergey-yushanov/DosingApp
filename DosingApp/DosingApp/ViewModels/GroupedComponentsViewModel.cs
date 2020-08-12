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

        public ICommand EditManufacturersCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public GroupedComponentsViewModel()
        {
            LoadManufacturers();
            EditManufacturersCommand = new Command(EditManufacturers);
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
                    ComponentsViewModel tempComponentsViewModel = new ComponentsViewModel(value);
                    selectedManufacturer = null;
                    OnPropertyChanged(nameof(SelectedManufacturer));
                    Application.Current.MainPage.Navigation.PushAsync(new ComponentsPage(tempComponentsViewModel));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void EditManufacturers()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManufacturersPage());
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
