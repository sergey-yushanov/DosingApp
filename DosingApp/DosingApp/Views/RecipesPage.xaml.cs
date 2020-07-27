using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class RecipesPage : ContentPage
    {
        string dbPath;

        public RecipesPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        protected override void OnAppearing()
        {
            /*using (AppDbContext db = new AppDbContext(dbPath))
            {
                itemsList.ItemsSource = db.Recipes.ToList();
            }
            base.OnAppearing();*/
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Recipe selectedRecipe = (Recipe)e.SelectedItem;
            RecipePage recipePage = new RecipePage();
            recipePage.BindingContext = selectedRecipe;
            await Navigation.PushAsync(recipePage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Recipe recipe = new Recipe();
            RecipePage recipePage = new RecipePage();
            recipePage.BindingContext = recipe;
            await Navigation.PushAsync(recipePage);
        }
    }
}