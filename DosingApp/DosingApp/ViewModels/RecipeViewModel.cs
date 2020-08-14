using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        #region Attributes
        RecipesViewModel recipesViewModel;
        public Recipe Recipe { get; private set; }
        private bool isBack;
        private string title;

        private ObservableCollection<Crop> crops;
        private ObservableCollection<ProcessingType> processingTypes;
        private ObservableCollection<Component> carriers;

        private ObservableCollection<RecipeComponent> recipeComponents;
        private RecipeComponent selectedRecipeComponent;

        public ICommand CreateRecipeComponentCommand { get; protected set; }
        public ICommand DeleteRecipeComponentCommand { get; protected set; }
        public ICommand SaveRecipeComponentCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public RecipeViewModel(Recipe recipe)
        {
            Recipe = recipe;
            LoadItems();
            InitSelectedItems();

            CreateRecipeComponentCommand = new Command(CreateRecipeComponent);
            DeleteRecipeComponentCommand = new Command(DeleteRecipeComponent);
            SaveRecipeComponentCommand = new Command(SaveRecipeComponent);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public RecipesViewModel RecipesViewModel
        {
            get { return recipesViewModel; }
            set { SetProperty(ref recipesViewModel, value); }
        }

        public string Name
        {
            get 
            {
                Title = (Recipe.RecipeId == 0) ? "Новый рецепт" : "Рецепт: " + Recipe.Name;
                return Recipe.Name; 
            }
            set
            {
                if (Recipe.Name != value)
                {
                    Recipe.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public Crop Crop
        {
            get { return Recipe.Crop; }
            set
            {
                if (Recipe.Crop != value)
                {
                    Recipe.Crop = value;
                    OnPropertyChanged(nameof(Crop));
                }
            }
        }

        public ObservableCollection<Crop> Crops
        {
            get { return crops; }
            set { SetProperty(ref crops, value); }
        }

        public ProcessingType ProcessingType
        {
            get { return Recipe.ProcessingType; }
            set
            {
                if (Recipe.ProcessingType != value)
                {
                    Recipe.ProcessingType = value;
                    OnPropertyChanged(nameof(ProcessingType));
                }
            }
        }

        public ObservableCollection<ProcessingType> ProcessingTypes
        {
            get { return processingTypes; }
            set { SetProperty(ref processingTypes, value); }
        }

        public Component Carrier
        {
            get { return Recipe.Carrier; }
            set
            {
                if (Recipe.Carrier != value)
                {
                    Recipe.Carrier = value;
                    OnPropertyChanged(nameof(Carrier));
                }
            }
        }

        public ObservableCollection<Component> Carriers
        {
            get { return carriers; }
            set { SetProperty(ref carriers, value); }
        }

        public string Note
        {
            get { return Recipe.Note; }
            set
            {
                if (Recipe.Note != value)
                {
                    Recipe.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        public string CarrierReserve
        {
            get
            {
                if (Recipe.CarrierReserve == null)
                {
                    return "";
                }
                else
                {
                    return Recipe.CarrierReserve.ToString();
                }
            }
            set
            {
                try
                {
                    Recipe.CarrierReserve = float.Parse(value);
                    Recipe.CarrierReserve = PercentLimits(Recipe.CarrierReserve);
                }
                catch
                {
                    Recipe.CarrierReserve = null;
                }
                OnPropertyChanged(nameof(CarrierReserve));
            }
        }

        public ObservableCollection<RecipeComponent> RecipeComponents
        {
            get { return recipeComponents; }
            set { SetProperty(ref recipeComponents, value); }
        }

        public RecipeComponent SelectedRecipeComponent
        {
            get { return selectedRecipeComponent; }
            set
            {
                if (selectedRecipeComponent != value)
                {
                    RecipeComponentViewModel tempRecipeComponent = new RecipeComponentViewModel(value) { RecipeViewModel = this };
                    selectedRecipeComponent = null;
                    OnPropertyChanged(nameof(SelectedRecipeComponent));
                    Application.Current.MainPage.Navigation.PushAsync(new RecipeComponentPage(tempRecipeComponent));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
        }

        public bool IsBack
        {
            get { return isBack; }
            set { SetProperty(ref isBack, value); }
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

        private async void CreateRecipeComponent()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя рецепта", "Ok");
                return;
            }

            if (Recipe.RecipeId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для добавления компонента необходимо сохранить рецепт. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    RecipesViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Recipe.RecipeId != 0)
            {
                RecipeComponent newRecipeComponent = new RecipeComponent() { Recipe = this.Recipe };
                await Application.Current.MainPage.Navigation.PushAsync(new RecipeComponentPage(new RecipeComponentViewModel(newRecipeComponent) { RecipeViewModel = this }));
            }
        }

        private void DeleteRecipeComponent(object recipeComponentInstance)
        {
            RecipeComponentViewModel recipeComponentViewModel = recipeComponentInstance as RecipeComponentViewModel;
            if (recipeComponentViewModel.RecipeComponent != null && recipeComponentViewModel.RecipeComponent.RecipeComponentId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.RecipeComponents.Remove(recipeComponentViewModel.RecipeComponent);
                    db.SaveChanges();
                }
                ReorderComponents(recipeComponentViewModel.RecipeComponent.Order);
            }
            Back();
        }

        private void SaveRecipeComponent(object recipeComponentInstance)
        {
            RecipeComponentViewModel recipeComponentViewModel = recipeComponentInstance as RecipeComponentViewModel;
            if (recipeComponentViewModel.RecipeComponent != null && recipeComponentViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (recipeComponentViewModel.RecipeComponent.RecipeComponentId == 0)
                    {
                        recipeComponentViewModel.RecipeComponent.Order = RecipeComponents.Count + 1;
                        db.Entry(recipeComponentViewModel.RecipeComponent).State = EntityState.Added;
                    }
                    else
                    {
                        db.RecipeComponents.Update(recipeComponentViewModel.RecipeComponent);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadItems()
        {
            using (AppDbContext db = App.GetContext())
            {
                Crops = new ObservableCollection<Crop>(db.Crops.ToList());
                ProcessingTypes = new ObservableCollection<ProcessingType>(db.ProcessingTypes.ToList());
                Carriers = new ObservableCollection<Component>(db.Components.ToList());
                // water must be on top in the list
                WaterOnTop();
            }
        }

        private void WaterOnTop()
        {
            var water = Carriers.FirstOrDefault(c => c.Name == Water.Name);
            var index = Carriers.IndexOf(water);
            if (index > 0)
                Carriers.Move(index, 0);
        }

        public void InitSelectedItems()
        {
            Crop = Crops.FirstOrDefault(c => c.CropId == Recipe.CropId);
            ProcessingType = ProcessingTypes.FirstOrDefault(pt => pt.ProcessingTypeId == Recipe.ProcessingTypeId);
            Carrier = Carriers.FirstOrDefault(c => c.ComponentId == Recipe.CarrierId);
        }

        private float? PercentLimits(float? percents)
        {
            return percents > 100.0 ? (float?)100.0 : percents < 0.0 ? (float?)0.0 : percents;
        }

        public void LoadRecipeComponents()
        {
            using (AppDbContext db = App.GetContext())
            {
                var recipeComponentsDB = db.RecipeComponents.Where(rc => rc.RecipeId == Recipe.RecipeId).ToList();
                RecipeComponents = new ObservableCollection<RecipeComponent>(recipeComponentsDB);
                RecipeComponents.ForEach(rc => rc.Component = db.Components.FirstOrDefault(c => c.ComponentId == rc.ComponentId));
            }
        }

        public void ReorderComponents(int? startOrder)
        {
            using (AppDbContext db = App.GetContext())
            {
                var recipeComponentsDB = db.RecipeComponents.Where(rc => rc.Order > startOrder).ToList();
                recipeComponentsDB.ForEach(rc => rc.Order--);
                db.SaveChanges();
            }
        }
        #endregion Methods
    }
}
