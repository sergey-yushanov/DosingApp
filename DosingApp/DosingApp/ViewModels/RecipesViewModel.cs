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
                CarrierId = GetWaterCarrier().ComponentId,
                FillMotherLiquorVolume = (double?)30.0
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
                    List<Assignment> assignments = db.Assignments.Where(a => a.RecipeId == recipeViewModel.Recipe.RecipeId).ToList();
                    if (assignments.Count > 0)
                    {
                        string assignmentsNames = string.Join("\n", assignments.Select(a => a.Name));
                        Application.Current.MainPage.DisplayAlert("Предупреждение", "Невозможно удалить рецепт пока он используется в следующих заданиях:\n\n" + assignmentsNames, "Ok");
                        return;
                    }

                    var componentIds = recipeViewModel.RecipeComponents.Select(rc => rc.ComponentId).ToList();
                    var jobs = db.Jobs.Where(j => j.RecipeId == recipeViewModel.Recipe.RecipeId).Distinct().ToList();
                    var jobsIds = jobs.Select(j => j.JobId).ToList();
                    var jobComponents = db.JobComponents.Where(jc => jobsIds.Contains((int)jc.JobId)).ToList();
                    db.JobComponents.RemoveRange(jobComponents);
                    db.Jobs.RemoveRange(jobs);
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
                if (!recipeViewModel.IsAllValid)
                {
                    string message = recipeViewModel.IsMotherLiquor ? "Задайте название рецепта, носитель смеси и объём предварительной подачи носителя" : "Задайте название рецепта и носитель смеси";
                    Application.Current.MainPage.DisplayAlert("Предупреждение", message, "Ok");
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
