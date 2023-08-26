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
    public partial class RequirementInvoicesPage : ContentPage
    {
        public RequirementInvoicesPage()
        {
            InitializeComponent();
            BindingContext = new RequirementInvoicesViewModel();
        }

        protected override void OnAppearing()
        {
            var requirementInvoicesViewModel = (RequirementInvoicesViewModel)BindingContext;
            requirementInvoicesViewModel.LoadReports();
            base.OnAppearing();
        }
    }
}