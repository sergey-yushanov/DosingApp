using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DosingApp.ViewModels
{
    public class ApplicatorsViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        private ObservableCollection<Applicator> applicators;
        private Applicator selectedApplicator;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ApplicatorsViewModel()
        {
            LoadApplicators();

            CreateCommand = new Command(CreateApplicator);
            DeleteCommand = new Command(DeleteApplicator);
            SaveCommand = new Command(SaveApplicator);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Applicator> Applicators
        {
            get { return applicators; }
            set { SetProperty(ref applicators, value); }
        }

        public Applicator SelectedApplicator
        {
            get { return selectedApplicator; }
            set
            {
                if (selectedApplicator != value)
                {
                    ApplicatorViewModel tempApplicator = new ApplicatorViewModel(value) { ApplicatorsViewModel = this };
                    selectedApplicator = null;
                    OnPropertyChanged(nameof(SelectedApplicator));
                    Application.Current.MainPage.Navigation.PushAsync(new ApplicatorPage(tempApplicator));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateApplicator()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ApplicatorPage(new ApplicatorViewModel(new Applicator()) { ApplicatorsViewModel = this }));
        }

        private void DeleteApplicator(object applicatorInstance)
        {
            ApplicatorViewModel applicatorViewModel = applicatorInstance as ApplicatorViewModel;
            if (applicatorViewModel.Applicator != null && applicatorViewModel.Applicator.ApplicatorId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.ApplicatorTanks.RemoveRange(applicatorViewModel.ApplicatorTanks);
                    db.Applicators.Remove(applicatorViewModel.Applicator);
                    db.SaveChanges();
                }
            }
            LoadApplicators();
            Back();
        }

        private void SaveApplicator(object applicatorInstance)
        {
            ApplicatorViewModel applicatorViewModel = applicatorInstance as ApplicatorViewModel;
            if (applicatorViewModel.Applicator != null && applicatorViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (applicatorViewModel.Applicator.ApplicatorId == 0)
                    {
                        db.Entry(applicatorViewModel.Applicator).State = EntityState.Added;
                    }
                    else
                    {
                        db.Applicators.Update(applicatorViewModel.Applicator);
                    }
                    db.SaveChanges();
                }

                SetSelectedApplicatorTank(applicatorViewModel);
            }
            LoadApplicators();
            if (applicatorViewModel.IsBack)
            {
                Back();
            }
        }
        #endregion Commands

        #region Methods
        public void LoadApplicators()
        {
            using (AppDbContext db = App.GetContext())
            {
                Applicators = new ObservableCollection<Applicator>(db.Applicators.ToList());
            }
        }

        private void SetSelectedApplicatorTank(ApplicatorViewModel applicatorViewModel)
        {
            if (applicatorViewModel.SelectedApplicatorTank != null)
            {
                applicatorViewModel.ApplicatorTanks.ForEach(ft => ft.IsUsedTank = false);
                applicatorViewModel.ApplicatorTanks.FirstOrDefault(ft => ft.ApplicatorTankId == applicatorViewModel.SelectedApplicatorTank.ApplicatorTankId).IsUsedTank = true;

                using (AppDbContext db = App.GetContext())
                {
                    db.ApplicatorTanks.UpdateRange(applicatorViewModel.ApplicatorTanks);
                    db.SaveChanges();
                }
            }
        }
        #endregion Methods
    }
}
