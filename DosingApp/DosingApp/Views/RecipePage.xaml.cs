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
        }

        protected override void OnAppearing()
        {
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                pickerCrop.ItemsSource = db.Crops.ToList();
                pickerProcessingType.ItemsSource = db.ProcessingTypes.ToList();
                pickerComponent.ItemsSource = db.Components.ToList();
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

                    db.ChangeTracker.TrackGraph(recipe, r => {
                        if (r.Entry.IsKeySet)
                        {
                            r.Entry.State = EntityState.Modified;
                        }
                        else
                        {
                            r.Entry.State = EntityState.Added;
                        }
                    });


                    //var crop = db.Crops.Find(recipe.Crop.Id);
                    //recipe.Crop = crop;

                    //if (recipe.Id == 0)
                    //{
                    //db.Recipes.Add(recipe);
                    //}
                    //else
                    //{
                    //db.Recipes.Update(recipe);
                    //}
                    db.SaveChanges();
                    var recipes = db.Recipes.ToList();
                }
            }
            Back();
        }

        private void DeleteButton(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            using (AppDbContext db = new AppDbContext(dbPath))
            {
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
            Back();
        }

    }
}