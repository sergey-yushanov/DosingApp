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
    public partial class GroupedComponentsPage : ContentPage
    {
        public GroupedComponentsPage()
        {
            InitializeComponent();
            BindingContext = new GroupedComponentsViewModel();
        }

        protected override void OnAppearing()
        {
            var groupedComponentsViewModel = (GroupedComponentsViewModel)BindingContext;
            groupedComponentsViewModel.LoadManufacturers();
            base.OnAppearing();
        }
    }
}