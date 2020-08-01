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
    public partial class FieldsPage : ContentPage
    {
        public FieldsPage()
        {
            InitializeComponent();
            BindingContext = new FieldsViewModel();
        }

        protected override void OnAppearing()
        {
            var FieldsViewModel = (FieldsViewModel)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                FieldsViewModel.LoadFields();
                fieldsList.ItemsSource = FieldsViewModel.Fields;
            }
            base.OnAppearing();
        }
    }
}