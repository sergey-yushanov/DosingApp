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
    public class RecipesViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Recipe> recipes;
        private Recipe selectedRecipe;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public RecipesViewModel()
        {
            CreateCommand = new Command(CreateRecipe);
            DeleteCommand = new Command(DeleteRecipe);
            SaveCommand = new Command(SaveRecipe);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Recipe> Recipes 
        {
            get { return recipes; }
            set { SetProperty(ref recipes, value); }
        }

        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                if (selectedRecipe != value)
                {
                    RecipeViewModel tempRecipe = new RecipeViewModel(value) { RecipesViewModel = this };
                    selectedRecipe = null;
                    OnPropertyChanged(nameof(SelectedRecipe));
                    Application.Current.MainPage.Navigation.PushAsync(new RecipePage(tempRecipe));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateRecipe()
        {
            Recipe newRecipe = new Recipe()
            {
                CarrierReserve = (double?)20.0,
                CarrierId = GetWaterCarrier().ComponentId
            };
            Application.Current.MainPage.Navigation.PushAsync(new RecipePage(new RecipeViewModel(newRecipe) { RecipesViewModel = this }));
        }

        private void DeleteRecipe(object recipeInstance)
        {
            RecipeViewModel recipeViewModel = recipeInstance as RecipeViewModel;
            if (recipeViewModel.Recipe != null && recipeViewModel.Recipe.RecipeId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.RecipeComponents.RemoveRange(recipeViewModel.RecipeComponents);
                    db.Recipes.Remove(recipeViewModel.Recipe);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveRecipe(object recipeInstance)
        {
            RecipeViewModel recipeViewModel = recipeInstance as RecipeViewModel;
            if (recipeViewModel.Recipe != null)
            {
                if (!recipeViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название рецепта", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (recipeViewModel.Recipe.RecipeId == 0)
                    {
                        db.Entry(recipeViewModel.Recipe).State = EntityState.Added;
                    }
                    else
                    {
                        db.Recipes.Update(recipeViewModel.Recipe);
                    }
                    db.SaveChanges();
                }
            }
            if (recipeViewModel.IsBack)
            {
                Back();
            }
        }
        #endregion Commands

        #region Methods
        public void LoadRecipes()
        {
            using (AppDbContext db = App.GetContext())
            {
                Recipes = new ObservableCollection<Recipe>(db.Recipes.ToList());
            }
        }

        private Component GetWaterCarrier()
        {
            using (AppDbContext db = App.GetContext())
            {
                return db.Components.FirstOrDefault(c => c.Name == Water.Name);
            }
        }
        #endregion Methods
    }
}
