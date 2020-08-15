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
    public partial class MixersPage : ContentPage
    {
        public MixersPage()
        {
            InitializeComponent();
            BindingContext = new MixersViewModel();
        }

        protected override void OnAppearing()
        {
            var mixersViewModel = (MixersViewModel)BindingContext;
            mixersViewModel.LoadMixers();
            base.OnAppearing();
        }
    }
}