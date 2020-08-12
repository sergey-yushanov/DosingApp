using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class ComponentPage : ContentPage
    {
        public ComponentViewModel ViewModel { get; private set; }
        public ComponentPage(ComponentViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void OnDensityTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var componentViewModel = (ComponentViewModel)BindingContext;
                componentViewModel.Density = null;
            }
        }
    }
}