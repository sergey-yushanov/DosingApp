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
    public partial class ApplicatorTankPage : ContentPage
    {
        public ApplicatorTankViewModel ViewModel { get; private set; }
        public ApplicatorTankPage(ApplicatorTankViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void OnVolumeTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var applicatorTankViewModel = (ApplicatorTankViewModel)BindingContext;
                applicatorTankViewModel.Volume = null;
            }
        }
    }
}