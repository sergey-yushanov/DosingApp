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

namespace DosingApp.ViewModels
{
    public class ManufacturerViewModel : BaseViewModel
    {
        #region Attributes
        ManufacturersViewModel manufacturersViewModel;
        public Manufacturer Manufacturer { get; private set; }

        private ObservableCollection<Component> components;

        private string title;
        #endregion Attributes

        #region Constructor
        public ManufacturerViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            LoadComponents();
        }
        #endregion Constructor

        #region Properties
        public ManufacturersViewModel ManufacturersViewModel
        {
            get { return manufacturersViewModel; }
            set { SetProperty(ref manufacturersViewModel, value); }
        }

        public ObservableCollection<Component> Components
        {
            get { return components; }
            set { SetProperty(ref components, value); }
        }

        public string Name
        {
            get
            {
                Title = (Manufacturer.ManufacturerId == 0) ? "Новый производитель" : "Производитель: " + Manufacturer.Name;
                return Manufacturer.Name;
            }
            set
            {
                if (Manufacturer.Name != value)
                {
                    Manufacturer.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsValid
        {
            get { return (!String.IsNullOrEmpty(Name)); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Methods
        public void LoadComponents()
        {
            using (AppDbContext db = App.GetContext())
            {
                var componentsDB = db.Components.Where(c => c.ManufacturerId == Manufacturer.ManufacturerId).ToList();
                Components = new ObservableCollection<Component>(componentsDB);
            }
        }
        #endregion Methods
    }
}
