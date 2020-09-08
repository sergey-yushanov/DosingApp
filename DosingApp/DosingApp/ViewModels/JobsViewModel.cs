using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
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
                    db.Entry(jobViewModel.Job).State = EntityState.Added;
                    db.SaveChanges();

                    var jobComponents = GetJobComponents(jobViewModel.Job);
                    jobComponents.ForEach(jc => db.Entry(jc).State = EntityState.Added);
                    db.SaveChanges();

                    Application.Current.MainPage.Navigation.PushAsync(new JobComponentsPage(new JobComponentsViewModel(jobViewModel.Job, jobComponents)));
                }
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

        public Component LoadRecipeCarrier(Job job)
        {
            using (AppDbContext db = App.GetContext())
            {
                var recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == job.RecipeId);
                var recipeCarrier = db.Components.FirstOrDefault(c => c.ComponentId == recipe.CarrierId);
                return recipeCarrier;
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
                JobId = job.JobId,
                Job = job,
                ComponentId = rc.ComponentId,
                Component = rc.Component,
                Order = rc.Order,
                Volume = GetVolume(rc, job.PartySize),
                VolumeRate = rc.VolumeRate,
                VolumeUnit = GetVolumeUnit(rc),
                VolumeRateUnit = rc.VolumeRateUnit,
                Dispenser = GetDispenser(rc)
            }));

            var carrierVolume = GetCarrierVolume(job, jobComponents);
            var carrierVolumeRate = GetCarrierVolumeRate(carrierVolume, job.PartySize);
            var recipeCarrier = LoadRecipeCarrier(job);

            jobComponents.Insert(0, new JobComponent()
            {
                JobId = job.JobId,
                Job = job,
                ComponentId = recipeCarrier.ComponentId,
                Component = recipeCarrier,
                Order = 0,
                Volume = carrierVolume,
                VolumeRate = carrierVolumeRate,
                VolumeUnit = VolumeUnit.Liquid,
                VolumeRateUnit = VolumeRateUnit.Liquid,
                Dispenser = "Носитель"
            });

            return jobComponents;
        }

        private double? GetVolume(RecipeComponent recipeComponent, double? square)
        {
            double? doubleValue = 0.0;

            if (String.Equals(recipeComponent.VolumeRateUnit, VolumeRateUnit.Dry))
            {
                doubleValue = recipeComponent.Component.Density != 0 ? recipeComponent.VolumeRate / recipeComponent.Component.Density * square : 0.0;
            }
            else
            {
                doubleValue = recipeComponent.VolumeRate * square;
            }

            var decimalValue = (decimal)doubleValue;
            return (double)Math.Round(decimalValue, 2);
        }

        private double? GetCarrierVolume(Job job, List<JobComponent> jobComponents)
        {
            double? jobComponentsVolume = 0.0;
            jobComponents.ForEach(jc => jobComponentsVolume += jc.Volume);
            
            double? doubleValue = job.PartyVolume - jobComponentsVolume;
            var decimalValue = (decimal)doubleValue;
            return (double)Math.Round(decimalValue, 2);
        }

        private double? GetCarrierVolumeRate(double? carrierVolume, double? square)
        {
            double? carrierVolumeRate = square != 0 ? carrierVolume / square : 0.0;

            double? doubleValue = carrierVolumeRate;
            var decimalValue = (decimal)doubleValue;
            return (double)Math.Round(decimalValue, 2);
        }

        private string GetVolumeUnit(RecipeComponent recipeComponent)
        {
            return String.Equals(recipeComponent.Component.Consistency, ComponentConsistency.Dry) ? VolumeUnit.Dry : VolumeUnit.Liquid;
        }

        private string GetDispenser(RecipeComponent recipeComponent)
        {
            return String.Equals(recipeComponent.Component.Consistency, ComponentConsistency.Dry) ? "Загр. вручную" : recipeComponent.Dispenser;
        }
        #endregion Methods
    }
}
