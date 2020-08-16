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
    public partial class MixturePage : ContentPage
    {
        public MixtureViewModel ViewModel { get; private set; }
        public MixturePage(MixtureViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var mixtureViewModel = (MixtureViewModel)BindingContext;
            mixtureViewModel.LoadItems();
            mixtureViewModel.InitSelectedItems();
            base.OnAppearing();
        }

        private void OnVolumeRateTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var mixtureViewModel = (MixtureViewModel)BindingContext;
                mixtureViewModel.VolumeRate = null;
            }
        }
    }
}