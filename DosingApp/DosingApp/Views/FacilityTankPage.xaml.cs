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
    public partial class FacilityTankPage : ContentPage
    {
        public FacilityTankViewModel ViewModel { get; private set; }
        public FacilityTankPage(FacilityTankViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void OnVolumeTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var facilityTankViewModel = (FacilityTankViewModel)BindingContext;
                facilityTankViewModel.Volume = null;
            }
        }
    }
}