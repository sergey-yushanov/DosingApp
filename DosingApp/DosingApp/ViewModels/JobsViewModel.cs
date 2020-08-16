using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class JobsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Assignment> assignments;
        private Assignment selectedAssignment;

        public ICommand SaveCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public JobsViewModel()
        {
            SaveCommand = new Command(SaveJob);
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
                    Job newJob = new Job() { Assignment = value };
                    JobViewModel tempJob = new JobViewModel(newJob) { JobsViewModel = this };
                    selectedAssignment = null;
                    OnPropertyChanged(nameof(SelectedAssignment));
                    Application.Current.MainPage.Navigation.PushAsync(new JobPage(tempJob));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void SaveJob(object jobInstance)
        {
            JobViewModel jobViewModel = jobInstance as JobViewModel;
            if (jobViewModel.Job != null && jobViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (jobViewModel.Job.JobId == 0)
                    {
                        db.Entry(jobViewModel.Job).State = EntityState.Added;
                    }
                    //else
                    //{
                        //db.Recipes.Update(recipeViewModel.Recipe);
                    //}
                    //db.JobComponents.AddRange(GetJobComponents(jobViewModel.Job));
                    db.SaveChanges();
                }
                Application.Current.MainPage.Navigation.PushAsync(new JobComponentsPage(new JobComponentsViewModel(jobViewModel.Job)));
            }
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

        public List<RecipeComponent> LoadRecipeComponents(Job job)
        {
            using (AppDbContext db = App.GetContext())
            {
                var recipeComponents = db.RecipeComponents.Where(rc => rc.RecipeId == job.RecipeId).ToList();
                recipeComponents.ForEach(rc => rc.Component = db.Components.FirstOrDefault(c => c.ComponentId == rc.ComponentId));
                return recipeComponents;
            }
        }

        public List<JobComponent> GetJobComponents(Job job)
        {
            var jobComponents = new List<JobComponent>();
            var recipeComponents = LoadRecipeComponents(job);
            recipeComponents.ForEach(rc => jobComponents.Add(new JobComponent() 
                { 
                    Job = job,
                    Component = rc.Component,
                    Order = rc.Order,
                    Volume = GetVolume(rc.VolumeRate, job.Square),
                    VolumeRate = rc.VolumeRate,
                    Unit = rc.Unit,
                    Dispenser = rc.Dispenser
                }
            ));
            return jobComponents;
        }

        private double? GetVolume(double? volumeRate, double? square)
        {
            return volumeRate * square;
        }

        #endregion Methods
    }
}
