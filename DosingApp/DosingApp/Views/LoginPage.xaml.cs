using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class LoginPage : PopupPage
    {
        public LoginViewModel ViewModel { get; private set; }
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var loginViewModel = (LoginViewModel)BindingContext;
            loginViewModel.MainViewModel.SetUserAccess();
            base.OnAppearing();
        }
    }
}