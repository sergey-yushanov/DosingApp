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
    public partial class RecipeComponentPage : ContentPage
    {
        public RecipeComponentViewModel ViewModel { get; private set; }
        public RecipeComponentPage(RecipeComponentViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void OnVolumeRateTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var recipeComponentViewModel = (RecipeComponentViewModel)BindingContext;
                recipeComponentViewModel.VolumeRate = null;
            }
        }
    }
}