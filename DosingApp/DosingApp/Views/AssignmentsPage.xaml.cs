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
        }

        protected override void OnAppearing()
        {
            using (AppDbContext db = App.GetContext())
            {
                assignmentsList.ItemsSource = db.Assignments.ToList();
            }
            base.OnAppearing();
        }

        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Assignment selectedAssignment = (Assignment)e.SelectedItem;
            AssignmentPage assignmentPage = new AssignmentPage();
            assignmentPage.BindingContext = selectedAssignment;
            await Navigation.PushAsync(assignmentPage);
        }

        // обработка нажатия кнопки добавления
        private async void CreateButton(object sender, EventArgs e)
        {
            Assignment assignment = new Assignment();
            AssignmentPage assignmentPage = new AssignmentPage();
            assignmentPage.BindingContext = assignment;
            await Navigation.PushAsync(assignmentPage);
        }
    }
}