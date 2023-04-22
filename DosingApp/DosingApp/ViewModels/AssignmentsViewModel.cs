using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class AssignmentsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Assignment> assignments;
        private Assignment selectedAssignment;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand NullifyCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public AssignmentsViewModel()
        {
            CreateCommand = new Command(CreateAssignment);
            DeleteCommand = new Command(DeleteAssignment);
            SaveCommand = new Command(SaveAssignment);
            NullifyCommand = new Command(NullifyAssignment);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Assignment> Assignments 
        {
            get { return assignments; }
            set { SetProperty(ref assignments, value); }
        }

        public Assignment SelectedAssignment
        {
            get { return selectedAssignment; }
            set
            {
                if (selectedAssignment != value)
                {
                    AssignmentViewModel tempAssignment = new AssignmentViewModel(value) { AssignmentsViewModel = this };
                    selectedAssignment = null;
                    OnPropertyChanged(nameof(SelectedAssignment));
                    Application.Current.MainPage.Navigation.PushAsync(new AssignmentPage(tempAssignment));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateAssignment()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AssignmentPage(new AssignmentViewModel(new Assignment()) { AssignmentsViewModel = this }));
        }

        private void DeleteAssignment(object assignmentInstance)
        {
            AssignmentViewModel assignmentViewModel = assignmentInstance as AssignmentViewModel;
            if (assignmentViewModel.Assignment != null && assignmentViewModel.Assignment.AssignmentId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.Assignments.Remove(assignmentViewModel.Assignment);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveAssignment(object assignmentInstance)
        {
            AssignmentViewModel assignmentViewModel = assignmentInstance as AssignmentViewModel;
            if (assignmentViewModel.Assignment != null)
            {
                if (!assignmentViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название задания, выберите рецепт, введите норму вылива и размер партии смеси", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (assignmentViewModel.Assignment.AssignmentId == 0)
                    {
                        db.Entry(assignmentViewModel.Assignment).State = EntityState.Added;
                    }
                    else
                    {
                        db.Assignments.Update(assignmentViewModel.Assignment);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void NullifyAssignment()
        {

        }
        #endregion Commands

        #region Methods
        public void LoadAssignments()
        {
            using (AppDbContext db = App.GetContext())
            {
                Assignments = new ObservableCollection<Assignment>(db.Assignments.ToList());
            }
        }
        #endregion Methods
    }
}
