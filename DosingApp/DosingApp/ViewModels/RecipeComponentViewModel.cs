﻿using DosingApp.DataContext;
using DosingApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class RecipeComponentViewModel : BaseViewModel
    {
        #region Attributes
        RecipeViewModel recipeViewModel;
        public RecipeComponent RecipeComponent { get; private set; }

        private ObservableCollection<Component> components;
        private bool isComponentEnabled;
        #endregion Attributes

        #region Constructor
        public RecipeComponentViewModel(RecipeComponent recipeComponent)
        {
            RecipeComponent = recipeComponent;
            LoadComponents();
            InitSelectedComponent();
        }
        #endregion Constructor

        #region Properties
        public RecipeViewModel RecipeViewModel
        {
            get { return recipeViewModel; }
            set { SetProperty(ref recipeViewModel, value); }
        }

        public Component Component
        {
            get { return RecipeComponent.Component; }
            set
            {
                if (RecipeComponent.Component != value)
                {
                    DensityError(value, Unit);
                    RecipeComponent.Component = value;
                    OnPropertyChanged(nameof(Component));
                }
            }
        }

        public ObservableCollection<Component> Components
        {
            get { return components; }
            set { SetProperty(ref components, value); }
        }

        public double? VolumeRate
        {
            get { return RecipeComponent.VolumeRate; }
            set
            {
                if (RecipeComponent.VolumeRate != value)
                {
                    RecipeComponent.VolumeRate = value;
                    OnPropertyChanged(nameof(VolumeRate));
                }
            }
        }

        public string Unit
        {
            get { return RecipeComponent.Unit; }
            set
            {
                if (RecipeComponent.Unit != value)
                {
                    DensityError(Component, value);
                    RecipeComponent.Unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public ObservableCollection<string> UnitList
        {
            get { return new ObservableCollection<string>(RecipeComponentUnit.GetList()); }
        }

        public string Dispenser
        {
            get 
            {
                IsComponentEnabled = !RecipeComponent.IsFourthValve();
                if (RecipeComponent.IsFourthValve())
                {
                    SetFourthValveComponent();
                }
                return RecipeComponent.Dispenser;
            }
            set
            {
                if (RecipeComponent.Dispenser != value)
                {
                    RecipeComponent.Dispenser = value;
                    OnPropertyChanged(nameof(Dispenser));
                }
            }
        }

        public ObservableCollection<string> Dispensers
        {
            get
            {
                if (App.GetUsedMixer() != null)
                {
                    return new ObservableCollection<string>(App.GetUsedMixer().GetDispensers());
                }
                else
                    return null;
            }
        }

        public bool IsValid
        {
            get
            {
                return (Component != null);
            }
        }

        public bool IsComponentEnabled
        {
            get { return isComponentEnabled; }
            set { SetProperty(ref isComponentEnabled, value); }
        }

        public string Title
        {
            get { return "Рецепт: " + RecipeViewModel.Name; ; }
        }
        #endregion Properties

        #region Methods
        public void LoadComponents()
        {
            using (AppDbContext db = App.GetContext())
            {
                Components = new ObservableCollection<Component>(db.Components.ToList());
            }
        }

        public void InitSelectedComponent()
        {
            Component = Components.FirstOrDefault(c => c.ComponentId == RecipeComponent.ComponentId);
        }

        public void SetFourthValveComponent()
        {
            Component = Components.FirstOrDefault(c => c.Name == Water.GetComponent().Name);
        }

        public void DensityError(Component component, string unit)
        {
            if (component != null && component.IsLiquid() && component.Density == null && String.Equals(unit, RecipeComponentUnit.Dry))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", " У выбранного компонента не указана плотность!", "Ok");
            }
        }
        #endregion Methods
    }
}
