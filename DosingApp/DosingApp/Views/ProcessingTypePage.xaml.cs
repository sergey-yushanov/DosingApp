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
    public partial class ProcessingTypePage : ContentPage
    {
        public ProcessingTypeViewModel ViewModel { get; private set; }
        public ProcessingTypePage(ProcessingTypeViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
    }
}