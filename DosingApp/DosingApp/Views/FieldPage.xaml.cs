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
    public partial class FieldPage : ContentPage
    {
        public FieldViewModel ViewModel { get; private set; }
        public FieldPage(FieldViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void OnSizeTextChanged(object sender, TextChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                var fieldViewModel = (FieldViewModel)BindingContext;
                fieldViewModel.Size = null;
            }
        }
    }
}