using DosingApp.DataContext;
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
    public partial class GroupedComponentsPage : ContentPage
    {
        public GroupedComponentsPage(bool isEditMode, RecipeViewModel recipeViewModel, RecipeComponentViewModel recipeComponentViewModel)
        {
            InitializeComponent();
            BindingContext = new GroupedComponentsViewModel(isEditMode, recipeViewModel, recipeComponentViewModel);
        }

        protected override void OnAppearing()
        {
            var groupedComponentsViewModel = (GroupedComponentsViewModel)BindingContext;
            groupedComponentsViewModel.LoadManufacturers();
            base.OnAppearing();
        }
    }
}