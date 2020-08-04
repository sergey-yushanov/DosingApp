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
        //private readonly DataService<Applicator> dataServiceApplicators;
        public readonly AppDbContext db;
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
            db = App.GetContext();
            LoadApplicators();
            //CreateApplicators();

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
                db.Applicators.Attach(applicatorViewModel.Applicator);
                db.Applicators.Remove(applicatorViewModel.Applicator);
                db.SaveChanges();
            }
            LoadApplicators();
            Back();
        }

        private void SaveApplicator(object applicatorInstance)
        {
            ApplicatorViewModel applicatorViewModel = applicatorInstance as ApplicatorViewModel;
            if (applicatorViewModel.Applicator != null && applicatorViewModel.IsValid)
            {
                if (applicatorViewModel.Applicator.ApplicatorId == 0)
                {
                    db.Entry(applicatorViewModel.Applicator).State = EntityState.Added;
                }
                else
                {
                    db.Applicators.Attach(applicatorViewModel.Applicator);
                    db.Applicators.Update(applicatorViewModel.Applicator);
                }
                db.SaveChanges();
            }
            LoadApplicators();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadApplicators()
        {
            Applicators = new ObservableCollection<Applicator>(db.Applicators.ToList());
        }

/*        private void CreateApplicators()
        {
            var applicators = new List<Applicator>()
            {
                new Applicator { Name = "Applicator 1", Code = "f1" },
                new Applicator { Name = "Applicator 2", Code = "f2" },
                new Applicator { Name = "Applicator 3", Code = "f3" }
            };

            db.Applicators.AddRange(applicators);
            db.SaveChanges();
        }*/
        #endregion Methods

    }
}
