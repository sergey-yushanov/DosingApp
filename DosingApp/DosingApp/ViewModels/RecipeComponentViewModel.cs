using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class RecipeComponentViewModel : BaseViewModel
    {
        #region Attributes
        RecipeViewModel recipeViewModel;
        public RecipeComponent RecipeComponent { get; private set; }

        private ObservableCollection<Component> components;
        private ObservableCollection<string> dispensers;
        private bool isComponentEnabled;
        private bool isUnitEnabled;
        private bool isDispenserVisible;

        public ICommand SelectComponentCommand { get; protected set; }
        public ICommand ClearDispenserCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public RecipeComponentViewModel(RecipeComponent recipeComponent)
        {
            RecipeComponent = recipeComponent;
            LoadComponents();
            InitSelectedComponent();
            LoadDispensers();

            SelectComponentCommand = new Command(SelectComponent);
            ClearDispenserCommand = new Command(ClearDispenser);
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
                    CheckDryComponent(value);
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
            get { return RecipeComponent.VolumeRateUnit; }
            set
            {
                if (RecipeComponent.VolumeRateUnit != value)
                {
                    DensityError(Component, value);
                    RecipeComponent.VolumeRateUnit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public ObservableCollection<string> UnitList
        {
            get { return new ObservableCollection<string>(VolumeRateUnit.GetList()); }
        }

        public bool IsUnitEnabled
        {
            get { return isUnitEnabled; }
            set { SetProperty(ref isUnitEnabled, value); }
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
            get { return dispensers; }
            set { SetProperty(ref dispensers, value); }
        }

        public bool IsValid
        {
            get { return (Component != null); }
        }

        public bool IsComponentEnabled
        {
            get { return isComponentEnabled; }
            set { SetProperty(ref isComponentEnabled, value); }
        }

        public bool IsDispenserVisible
        {
            get { return isDispenserVisible; }
            set { SetProperty(ref isDispenserVisible, value); }
        }

        public string Title
        {
            get { return "Рецепт: " + RecipeViewModel.Name; ; }
        }
        #endregion Properties

        #region Commands
        private void SelectComponent()
        {
            if (IsComponentEnabled)
            {
                Application.Current.MainPage.Navigation.PushAsync(new GroupedComponentsPage(false, null, this));
            }
        }

        private void ClearDispenser()
        {
            Dispenser = null;
        }
        #endregion Commands

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
            if (component != null && component.IsLiquid() && component.Density == null && String.Equals(unit, VolumeRateUnit.Dry))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", " У выбранного компонента не указана плотность!", "Ok");
            }
        }

        public void CheckDryComponent(Component component)
        {
            IsUnitEnabled = component.IsLiquid();
            IsDispenserVisible = component.IsLiquid();

            if (!component.IsLiquid())
            {
                Unit = VolumeRateUnit.Dry;
                Dispenser = RecipeComponent.DryComponentDispenser;
            }
            else
            {
                if (String.Equals(Dispenser, RecipeComponent.DryComponentDispenser))
                {
                    Dispenser = null;
                }
            }
        }

        public void LoadDispensers()
        {
            if (App.GetUsedMixer() == null)
            {
                Dispensers = null;
            }

            using (AppDbContext db = App.GetContext())
            {
                var recipeId = RecipeComponent.Recipe != null ? RecipeComponent.Recipe.RecipeId : RecipeComponent.RecipeId;
                var recipeComponents = db.RecipeComponents.Where(rc => rc.RecipeId == recipeId && rc.RecipeComponentId != RecipeComponent.RecipeComponentId).ToList();
                Dispensers = new ObservableCollection<string>(App.GetUsedMixer().GetDispensers());
                recipeComponents.ForEach(rc => Dispensers.Remove(rc.Dispenser));
            }
        }
        #endregion Methods
    }
}
