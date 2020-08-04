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
    public class ComponentsViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<Component> dataServiceComponents;
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        private ObservableCollection<Component> components;
        private Component selectedComponent;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ComponentsViewModel()
        {
            db = App.GetContext();
            LoadComponents();
            //CreateComponents();

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
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateComponent()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ComponentPage(new ComponentViewModel(new Component()) { ComponentsViewModel = this }));
        }

        private void DeleteComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null && componentViewModel.Component.ComponentId != 0)
            {
                db.Components.Attach(componentViewModel.Component);
                db.Components.Remove(componentViewModel.Component);
                db.SaveChanges();
            }
            LoadComponents();
            Back();
        }

        private void SaveComponent(object componentInstance)
        {
            ComponentViewModel componentViewModel = componentInstance as ComponentViewModel;
            if (componentViewModel.Component != null && componentViewModel.IsValid)
            {
                if (componentViewModel.Component.ComponentId == 0)
                {
                    db.Entry(componentViewModel.Component).State = EntityState.Added;
                }
                else
                {
                    db.Components.Attach(componentViewModel.Component);
                    db.Components.Update(componentViewModel.Component);
                }
                db.SaveChanges();
            }
            LoadComponents();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadComponents()
        {
            Components = new ObservableCollection<Component>(db.Components.ToList());
        }

/*        private void CreateComponents()
        {
            var components = new List<Component>()
            {
                new Component { Name = "Component 1", Code = "f1" },
                new Component { Name = "Component 2", Code = "f2" },
                new Component { Name = "Component 3", Code = "f3" }
            };

            db.Components.AddRange(components);
            db.SaveChanges();
        }*/
        #endregion Methods

    }
}
