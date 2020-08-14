using DosingApp.DataContext;
using DosingApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace DosingApp.ViewModels
{
    public class RecipeComponentViewModel : BaseViewModel
    {
        #region Attributes
        RecipeViewModel recipeViewModel;
        public RecipeComponent RecipeComponent { get; private set; }

        private ObservableCollection<Component> components;
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

        public float? VolumeRate
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
                    RecipeComponent.Unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public ObservableCollection<string> UnitList
        {
            get { return new ObservableCollection<string>() { RecipeComponentUnit.Liquid, RecipeComponentUnit.Dry }; }
        }

        public string Valve
        {
            get { return RecipeComponent.Valve; }
            set
            {
                if (RecipeComponent.Valve != value)
                {
                    RecipeComponent.Valve = value;
                    OnPropertyChanged(nameof(Valve));
                }
            }
        }

        public string DispenserName
        {
            get { return RecipeComponent.DispenserName; }
            set
            {
                if (RecipeComponent.DispenserName != value)
                {
                    RecipeComponent.DispenserName = value;
                    OnPropertyChanged(nameof(DispenserName));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (Component != null);
            }
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
        #endregion Methods
    }
}
