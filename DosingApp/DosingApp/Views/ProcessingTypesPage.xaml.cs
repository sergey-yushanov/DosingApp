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
    public partial class ProcessingTypesPage : ContentPage
    {
        public ProcessingTypesPage()
        {
            InitializeComponent();
            BindingContext = new ProcessingTypesViewModel();
        }

        protected override void OnAppearing()
        {
            var ProcessingTypesViewModel = (ProcessingTypesViewModel)BindingContext;
            using (AppDbContext db = App.GetContext())
            {
                ProcessingTypesViewModel.LoadProcessingTypes();
                processingTypesList.ItemsSource = ProcessingTypesViewModel.ProcessingTypes;
            }
            base.OnAppearing();
        }
    }
}