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
    public partial class MixturesPage : ContentPage
    {
        public MixturesPage()
        {
            InitializeComponent();
            BindingContext = new MixturesViewModel();
        }

        protected override void OnAppearing()
        {
            var mixturesViewModel = (MixturesViewModel)BindingContext;
            mixturesViewModel.LoadMixtures();
            base.OnAppearing();
        }
    }
}