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
    public partial class TransportPage : TabbedPage
    {
        public TransportViewModel ViewModel { get; private set; }
        public TransportPage(TransportViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var transportViewModel = (TransportViewModel)BindingContext;
            transportViewModel.LoadTransportTanks();
            base.OnAppearing();
        }
    }
}