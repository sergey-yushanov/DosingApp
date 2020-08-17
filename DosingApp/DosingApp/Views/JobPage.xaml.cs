using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class JobPage : ContentPage
    {
        public JobViewModel ViewModel { get; private set; }
        public JobPage(JobViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var jobViewModel = (JobViewModel)BindingContext;
            jobViewModel.LoadItems();
            jobViewModel.InitSelectedItems();
            base.OnAppearing();
        }
    }
}