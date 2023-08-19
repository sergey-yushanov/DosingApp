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
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
            BindingContext = new ReportsViewModel();
        }

        protected override void OnAppearing()
        {
            var reportsViewModel = (ReportsViewModel)BindingContext;
            reportsViewModel.LoadReports();
            base.OnAppearing();
        }
    }
}