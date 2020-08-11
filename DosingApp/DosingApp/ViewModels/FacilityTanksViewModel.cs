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
    public class FacilityTanksViewModel : BaseViewModel
    {
        #region Services
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        private ObservableCollection<FacilityTank> facilityTanks;
        private FacilityTank selectedFacilityTank;
        private string title;

        public Facility Facility { get; private set; }

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public FacilityTanksViewModel(Facility facility)
        {
            db = App.GetContext();
            Facility = facility;
            LoadFacilityTanks();
            Title = "Объект: " + Facility.Name + "\nСписок емкостей";

            CreateCommand = new Command(CreateFacilityTank);
            DeleteCommand = new Command(DeleteFacilityTank);
            SaveCommand = new Command(SaveFacilityTank);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<FacilityTank> FacilityTanks
        {
            get { return facilityTanks; }
            set { SetProperty(ref facilityTanks, value); }
        }

        public FacilityTank SelectedFacilityTank
        {
            get { return selectedFacilityTank; }
            set
            {
                if (selectedFacilityTank != value)
                {
                    FacilityTankViewModel tempFacilityTank = new FacilityTankViewModel(value) { FacilityTanksViewModel = this };
                    selectedFacilityTank = null;
                    OnPropertyChanged(nameof(SelectedFacilityTank));
                    Application.Current.MainPage.Navigation.PushAsync(new FacilityTankPage(tempFacilityTank));
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

        private void CreateFacilityTank()
        {
            FacilityTank newFacilityTank = new FacilityTank
            {
                Facility = this.Facility,
                IsUsedTank = false
            };
            Application.Current.MainPage.Navigation.PushAsync(new FacilityTankPage(new FacilityTankViewModel(newFacilityTank) { FacilityTanksViewModel = this }));
        }

        private void DeleteFacilityTank(object facilityTankInstance)
        {
            FacilityTankViewModel facilityTankViewModel = facilityTankInstance as FacilityTankViewModel;
            if (facilityTankViewModel.FacilityTank.Facility != null && facilityTankViewModel.FacilityTank.FacilityId != 0)
            {
                db.FacilityTanks.Attach(facilityTankViewModel.FacilityTank);
                db.FacilityTanks.Remove(facilityTankViewModel.FacilityTank);
                db.SaveChanges();
            }
            LoadFacilityTanks();
            Back();
        }

        private void SaveFacilityTank(object facilityTankInstance)
        {
            FacilityTankViewModel facilityTankViewModel = facilityTankInstance as FacilityTankViewModel;
            if (facilityTankViewModel.FacilityTank != null && facilityTankViewModel.IsValid)
            {
                if (facilityTankViewModel.FacilityTank.FacilityTankId == 0)
                {
                    db.Entry(facilityTankViewModel.FacilityTank).State = EntityState.Added;
                }
                else
                {
                    db.FacilityTanks.Attach(facilityTankViewModel.FacilityTank);
                    db.FacilityTanks.Update(facilityTankViewModel.FacilityTank);
                }
                db.SaveChanges();
            }
            LoadFacilityTanks();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFacilityTanks()
        {
            var facilityTanksDB = db.FacilityTanks.Where(ft => ft.FacilityId == Facility.FacilityId).ToList();
            FacilityTanks = new ObservableCollection<FacilityTank>(facilityTanksDB);
        }
        #endregion Methods

    }
}
