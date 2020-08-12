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
    public partial class ManufacturersPage : ContentPage
    {
        public ManufacturersPage()
        {
            InitializeComponent();
            BindingContext = new ManufacturersViewModel();
        }

        protected override void OnAppearing()
        {
            var manufacturersViewModel = (ManufacturersViewModel)BindingContext;
            manufacturersViewModel.LoadManufacturers();
            base.OnAppearing();
        }
    }
}