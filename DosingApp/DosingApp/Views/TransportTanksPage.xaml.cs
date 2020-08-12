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
    public partial class TransportTanksPage : ContentPage
    {
        public TransportTanksViewModel ViewModel { get; private set; }
        public TransportTanksPage(TransportTanksViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var transportTanksViewModel = (TransportTanksViewModel)BindingContext;
            transportTanksViewModel.LoadTransportTanks();
            base.OnAppearing();
        }
    }
}