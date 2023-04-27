using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class ManualControlPage : ContentPage
    {
        public ManualControlPage()
        {
            InitializeComponent();
            BindingContext = new ManualControlViewModel();
        }

        protected override void OnDisappearing()
        {
            var manualControlViewModel = (ManualControlViewModel)BindingContext;
            manualControlViewModel.ModbusService.MasterDispose();
            base.OnDisappearing();
        }
    }
}