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
    public partial class AssignmentPage : ContentPage
    {
        public AssignmentViewModel ViewModel { get; private set; }
        public AssignmentPage(AssignmentViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var assignmentViewModel = (AssignmentViewModel)BindingContext;
            assignmentViewModel.LoadItems();
            assignmentViewModel.InitSelectedItems();
            base.OnAppearing();
        }
    }
}