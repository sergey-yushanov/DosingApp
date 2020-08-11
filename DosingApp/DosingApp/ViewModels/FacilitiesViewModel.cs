﻿using DosingApp.DataContext;
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
    public class FacilitiesViewModel : BaseViewModel
    {
        #region Services
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        private ObservableCollection<Facility> facilities;
        private Facility selectedFacility;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public FacilitiesViewModel()
        {
            db = App.GetContext();
            LoadFacilities();

            CreateCommand = new Command(CreateFacility);
            DeleteCommand = new Command(DeleteFacility);
            SaveCommand = new Command(SaveFacility);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Facility> Facilities
        {
            get { return facilities; }
            set { SetProperty(ref facilities, value); }
        }

        public Facility SelectedFacility
        {
            get { return selectedFacility; }
            set
            {
                if (selectedFacility != value)
                {
                    FacilityViewModel tempFacility = new FacilityViewModel(value) { FacilitiesViewModel = this };
                    selectedFacility = null;
                    OnPropertyChanged(nameof(SelectedFacility));
                    Application.Current.MainPage.Navigation.PushAsync(new FacilityPage(tempFacility));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateFacility()
        {
            Application.Current.MainPage.Navigation.PushAsync(new FacilityPage(new FacilityViewModel(new Facility()) { FacilitiesViewModel = this }));
        }

        private void DeleteFacility(object facilityInstance)
        {
            FacilityViewModel facilityViewModel = facilityInstance as FacilityViewModel;
            if (facilityViewModel.Facility != null && facilityViewModel.Facility.FacilityId != 0)
            {
                db.Facilities.Attach(facilityViewModel.Facility);
                db.Facilities.Remove(facilityViewModel.Facility);
                db.SaveChanges();
            }
            LoadFacilities();
            Back();
        }

        private void SaveFacility(object facilityInstance)
        {
            FacilityViewModel facilityViewModel = facilityInstance as FacilityViewModel;
            if (facilityViewModel.Facility != null && facilityViewModel.IsValid)
            {
                if (facilityViewModel.Facility.FacilityId == 0)
                {
                    db.Entry(facilityViewModel.Facility).State = EntityState.Added;
                }
                else
                {
                    db.Facilities.Attach(facilityViewModel.Facility);
                    db.Facilities.Update(facilityViewModel.Facility);
                }
                db.SaveChanges();

                SetSelectedFacilityTank(facilityViewModel);
            }
            LoadFacilities();
            if (facilityViewModel.IsBack)
            {
                Back();
            }
        }
        #endregion Commands

        #region Methods
        public void LoadFacilities()
        {
            Facilities = new ObservableCollection<Facility>(db.Facilities.ToList());
        }

        private void SetSelectedFacilityTank(FacilityViewModel facilityViewModel)
        {
            if (facilityViewModel.SelectedFacilityTank != null)
            {
                facilityViewModel.FacilityTanks.ForEach(ft => ft.IsUsedTank = false);
                facilityViewModel.FacilityTanks.FirstOrDefault(ft => ft.FacilityTankId == facilityViewModel.SelectedFacilityTank.FacilityTankId).IsUsedTank = true;
                facilityViewModel.FacilityTanks.ForEach(ft => AttachOrUpdate(ft));
                db.SaveChanges();
            }
        }

        private void AttachOrUpdate(FacilityTank facilityTank)
        {
            var instanceFacilityTank = db.FacilityTanks.Find(facilityTank.FacilityTankId);
            if (instanceFacilityTank == null)
            {
                db.FacilityTanks.Attach(facilityTank);
            }
            else
            {
                instanceFacilityTank.IsUsedTank = facilityTank.IsUsedTank;
            }
        }
        #endregion Methods
    }
}
