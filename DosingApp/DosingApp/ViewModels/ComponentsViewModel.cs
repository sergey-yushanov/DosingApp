using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class ComponentsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Component> components;
        private Component selectedComponent;
        private string title;

        public Manufacturer Manufacturer { get; private set; }

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ComponentsViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            Title = "Производитель: " + Manufacturer.Name + "\nКомпоненты";

            CreateCommand = new Command(CreateComponent);
            DeleteCommand = new Command(DeleteComponent);
            SaveCommand = new Command(SaveComponent);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Component> Components
        {
            get { return components; }
            set { SetProperty(ref components, value); }
        }

        public Component SelectedComponent
        {
            get { return selectedComponent; }
            set
            {
                if (selectedComponent != value)
                {
                    ComponentViewModel tempComponent = new ComponentViewModel(value) { ComponentsViewModel = this };
                    selectedComponent = null;
                    OnPropertyChanged(nameof(SelectedComponent));
                    Application.Current.MainPage.Navigation.PushAsync(new ComponentPage(tempComponent));
                }
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateComponent()
        {
            Component newComponent = new Component
            {
                Manufacturer = this.Manufacturer
            };
            Application.Current.MainPage.Navigation.PushAsync(new ComponentPage(new ComponentViewModel(newComponent) { ComponentsViewModel = this }));
        }

        private void DeleteComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null && componentViewModel.Component.ComponentId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Components.Remove(componentViewModel.Component);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null && componentViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (componentViewModel.Component.ComponentId == 0)
                    {
                        db.Entry(componentViewModel.Component).State = EntityState.Added;
                    }
                    else
                    {
                        db.Components.Update(componentViewModel.Component);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

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
