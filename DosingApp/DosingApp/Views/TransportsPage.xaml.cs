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
    public partial class TransportsPage : ContentPage
    {
        public TransportsPage()
        {
            InitializeComponent();
            BindingContext = new TransportsViewModel();
        }

        protected override void OnAppearing()
        {
            var TransportsViewModel = (TransportsViewModel)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                TransportsViewModel.LoadTransports();
                transportsList.ItemsSource = TransportsViewModel.Transports;
            }
            base.OnAppearing();
        }
    }
}