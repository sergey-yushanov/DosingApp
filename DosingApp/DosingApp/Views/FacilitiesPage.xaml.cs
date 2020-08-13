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
    public partial class FacilitiesPage : ContentPage
    {
        public FacilitiesPage()
        {
            InitializeComponent();
            BindingContext = new FacilitiesViewModel();
        }

        protected override void OnAppearing()
        {
            var facilitiesViewModel = (FacilitiesViewModel)BindingContext;
            facilitiesViewModel.LoadFacilities();
            base.OnAppearing();
        }
    }
}