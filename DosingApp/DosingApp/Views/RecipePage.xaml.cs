using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class RecipePage : TabbedPage
    {
        string dbPath;

        public RecipePage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                cropsList.ItemsSource = db.Crops.ToList() as List<Crop>;
                processingTypesList.ItemsSource = db.ProcessingTypes.ToList() as List<ProcessingType>;
                carriersList.ItemsSource = db.Components.ToList() as List<Component>;
            }
        }

        protected override void OnAppearing()
        {
            var recipe = (Recipe)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                cropsList.SelectedIndex = (cropsList.ItemsSource as List<Crop>).FindIndex(a => a.CropId == recipe.CropId);
                processingTypesList.SelectedIndex = (processingTypesList.ItemsSource as List<ProcessingType>).FindIndex(a => a.ProcessingTypeId == recipe.ProcessingTypeId);
                carriersList.SelectedIndex = (carriersList.ItemsSource as List<Component>).FindIndex(a => a.ComponentId == recipe.CarrierId);

                recipeComponentsList.ItemsSource = db.RecipeComponents.Where(rc => rc.RecipeId == recipe.RecipeId).OrderBy(rc => rc.Order).ToList();
                //db.RecipeComponents.Find().ToList();
            }
            base.OnAppearing();
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            if (!String.IsNullOrEmpty(recipe.Name))
            {
                using (AppDbContext db = new AppDbContext(dbPath))
                {
                    if (recipe.RecipeId == 0)
                    {
                        db.Entry(recipe).State = EntityState.Added;
                    }
                    else
                    {
                        db.Recipes.Attach(recipe);
                        db.Recipes.Update(recipe);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Recipes.Attach(recipe);
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
            Back();
        }

/*        // добавление компонента
        private void AddComponent(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            recipe.RecipeComponents.Add(new RecipeComponent { });
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Recipes.Attach(recipe);
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
            Back();
        }

        // удаление компонента
        private void RemoveComponent(object sender, EventArgs e)
        {
            //Phone phone = phonesList.SelectedItem as Phone;
            if (phone != null)
            {
                Phones.Remove(phone);
                phonesList.SelectedItem = null;
            }
        }*/
    }
}