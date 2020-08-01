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
    public partial class CropsPage : ContentPage
    {
        public CropsPage()
        {
            InitializeComponent();
            BindingContext = new CropsViewModel();
        }

        protected override void OnAppearing()
        {
            var CropsViewModel = (CropsViewModel)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                CropsViewModel.LoadCrops();
                cropsList.ItemsSource = CropsViewModel.Crops;
            }
            base.OnAppearing();
        }
    }
}