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
                if (jobViewModel.Job.PartyVolume == null)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте требуемый размер партии смеси", "Ok");
                    return;
                }
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
                var recipeComponents = db.RecipeComponents.Where(rc => rc.RecipeId == job.RecipeId).OrderBy(rc => rc.Order).ToList();
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
                Volume = GetVolume(rc, job),
                VolumeRate = rc.VolumeRate,
                VolumeUnit = GetVolumeUnit(rc),
                VolumeRateUnit = rc.VolumeRateUnit,
                Dispenser = rc.Dispenser
            }));

            var carrierVolume = GetCarrierVolume(job, jobComponents);
            var carrierVolumeRate = GetCarrierVolumeRate(carrierVolume, job.PartyVolume, job.VolumeRate);
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
                Dispenser = DispenserSuffix.Carrier
            });

            return jobComponents;
        }

        private double? GetVolume(RecipeComponent recipeComponent, Job job)
        {
            if (job.VolumeRate != 0.0)
            {
                double? doubleValue = job.PartyVolume * recipeComponent.VolumeRate / job.VolumeRate;

                if (String.Equals(recipeComponent.VolumeRateUnit, VolumeRateUnit.Dry))
                {
                    doubleValue = recipeComponent.Component.Density != 0 ? doubleValue / recipeComponent.Component.Density : doubleValue;
                }
                var decimalValue = (decimal)doubleValue;
                return (double)Math.Round(decimalValue, 2);
            }
            else
            {
                return 0.0;
            }
        }

        private double? GetCarrierVolume(Job job, List<JobComponent> jobComponents)
        {
            double? jobComponentsVolume = 0.0;
            jobComponents.ForEach(jc => jobComponentsVolume += jc.Volume);
            
            double? doubleValue = job.PartyVolume - jobComponentsVolume;
            var decimalValue = (decimal)doubleValue;
            return (double)Math.Round(decimalValue, 2);
        }

        private double? GetCarrierVolumeRate(double? carrierVolume, double? partyVolume, double? volumeRate)
        {
            double? carrierVolumeRate = partyVolume != 0 ? carrierVolume / partyVolume * volumeRate : 0.0;

            double? doubleValue = carrierVolumeRate;
            var decimalValue = (decimal)doubleValue;
            return (double)Math.Round(decimalValue, 2);
        }

        private string GetVolumeUnit(RecipeComponent recipeComponent)
        {
            return String.Equals(recipeComponent.Component.Consistency, ComponentConsistency.Dry) ? VolumeUnit.Dry : VolumeUnit.Liquid;
        }
        #endregion Methods
    }
}
