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
    public partial class AssignmentsPage : ContentPage
    {
        public AssignmentsPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentsViewModel();
        }

        protected override void OnAppearing()
        {
            var assignmentsViewModel = (AssignmentsViewModel)BindingContext;
            assignmentsViewModel.LoadAssignments();
            base.OnAppearing();
        }
    }
}