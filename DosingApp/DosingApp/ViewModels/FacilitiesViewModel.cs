using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FacilitiesViewModel : BaseViewModel
    {
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
                using (AppDbContext db = App.GetContext())
                {
                    db.FacilityTanks.RemoveRange(facilityViewModel.FacilityTanks);
                    db.Facilities.Remove(facilityViewModel.Facility);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveFacility(object facilityInstance)
        {
            FacilityViewModel facilityViewModel = facilityInstance as FacilityViewModel;
            if (facilityViewModel.Facility != null && facilityViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (facilityViewModel.Facility.FacilityId == 0)
                    {
                        db.Entry(facilityViewModel.Facility).State = EntityState.Added;
                    }
                    else
                    {
                        db.Facilities.Update(facilityViewModel.Facility);
                    }
                    db.SaveChanges();
                }
            }
            if (facilityViewModel.IsBack)
            {
                Back();
            }
        }
        #endregion Commands

        #region Methods
        public void LoadFacilities()
        {
            using (AppDbContext db = App.GetContext())
            {
                Facilities = new ObservableCollection<Facility>(db.Facilities.ToList());
            }
        }
        #endregion Methods
    }
}
