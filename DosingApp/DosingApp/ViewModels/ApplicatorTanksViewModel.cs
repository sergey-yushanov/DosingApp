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
    public class ApplicatorTanksViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        private ObservableCollection<ApplicatorTank> applicatorTanks;
        private ApplicatorTank selectedApplicatorTank;
        private string title;

        public Applicator Applicator { get; private set; }

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ApplicatorTanksViewModel(Applicator applicator)
        {
            Applicator = applicator;
            LoadApplicatorTanks();
            Title = "Аппликатор: " + Applicator.Name + "\nСписок емкостей";

            CreateCommand = new Command(CreateApplicatorTank);
            DeleteCommand = new Command(DeleteApplicatorTank);
            SaveCommand = new Command(SaveApplicatorTank);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<ApplicatorTank> ApplicatorTanks
        {
            get { return applicatorTanks; }
            set { SetProperty(ref applicatorTanks, value); }
        }

        public ApplicatorTank SelectedApplicatorTank
        {
            get { return selectedApplicatorTank; }
            set
            {
                if (selectedApplicatorTank != value)
                {
                    ApplicatorTankViewModel tempApplicatorTank = new ApplicatorTankViewModel(value) { ApplicatorTanksViewModel = this };
                    selectedApplicatorTank = null;
                    OnPropertyChanged(nameof(SelectedApplicatorTank));
                    Application.Current.MainPage.Navigation.PushAsync(new ApplicatorTankPage(tempApplicatorTank));
                }
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateApplicatorTank()
        {
            ApplicatorTank newApplicatorTank = new ApplicatorTank
            {
                Applicator = this.Applicator,
                IsUsedTank = false
            };
            Application.Current.MainPage.Navigation.PushAsync(new ApplicatorTankPage(new ApplicatorTankViewModel(newApplicatorTank) { ApplicatorTanksViewModel = this }));
        }

        private void DeleteApplicatorTank(object applicatorTankInstance)
        {
            ApplicatorTankViewModel applicatorTankViewModel = applicatorTankInstance as ApplicatorTankViewModel;
            if (applicatorTankViewModel.ApplicatorTank.ApplicatorTankId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.ApplicatorTanks.Remove(applicatorTankViewModel.ApplicatorTank);
                    db.SaveChanges();
                }
            }
            LoadApplicatorTanks();
            Back();
        }

        private void SaveApplicatorTank(object applicatorTankInstance)
        {
            ApplicatorTankViewModel applicatorTankViewModel = applicatorTankInstance as ApplicatorTankViewModel;
            if (applicatorTankViewModel.ApplicatorTank != null && applicatorTankViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (applicatorTankViewModel.ApplicatorTank.ApplicatorTankId == 0)
                    {
                        db.Entry(applicatorTankViewModel.ApplicatorTank).State = EntityState.Added;
                    }
                    else
                    {
                        db.ApplicatorTanks.Update(applicatorTankViewModel.ApplicatorTank);
                    }
                    db.SaveChanges();
                }
            }
            LoadApplicatorTanks();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadApplicatorTanks()
        {
            using (AppDbContext db = App.GetContext())
            {
                var applicatorTanksDB = db.ApplicatorTanks.Where(ft => ft.ApplicatorId == Applicator.ApplicatorId).ToList();
                ApplicatorTanks = new ObservableCollection<ApplicatorTank>(applicatorTanksDB);
            }
        }
        #endregion Methods

    }
}
