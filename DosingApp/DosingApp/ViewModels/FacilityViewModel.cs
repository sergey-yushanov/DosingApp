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
    public class FacilityViewModel : BaseViewModel
    {
        #region Attributes
        FacilitiesViewModel facilitiesViewModel;
        public Facility Facility { get; private set; }
        private bool isBack;    // need to page navigation while saving foreign key values
        private string title;
        private string pickerType;
        private bool isOwnType;

        private ObservableCollection<FacilityTank> facilityTanks;
        private FacilityTank selectedFacilityTank;

        public ICommand CreateTankCommand { get; protected set; }
        public ICommand DeleteTankCommand { get; protected set; }
        public ICommand SaveTankCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public FacilityViewModel(Facility facility)
        {
            Facility = facility;
            IsBack = true;

            if (Facility.FacilityId != 0)
            {
                PickerType = Facility.Type;
            }

            CreateTankCommand = new Command(CreateFacilityTank);
            DeleteTankCommand = new Command(DeleteFacilityTank);
            SaveTankCommand = new Command(SaveFacilityTank);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public FacilitiesViewModel FacilitiesViewModel
        {
            get { return facilitiesViewModel; }
            set { SetProperty(ref facilitiesViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = (Facility.FacilityId == 0) ? "Новый объект" : "Объект: " + Facility.Name;
                return Facility.Name; 
            }
            set
            {
                if (Facility.Name != value)
                {
                    Facility.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PickerType
        {
            get 
            {
                IsOwnType = String.Equals(pickerType, FacilityType.Own);
                return pickerType;
            }
            set 
            {
                if (Types.Contains(value))
                {
                    if (SetProperty(ref pickerType, value))
                    {
                        // set type property, which is different Types list
                        Type = FacilityType.GetNotOwnList().Contains(value) ? value : null;
                    }
                }
                else
                {
                    SetProperty(ref pickerType, FacilityType.Own);
                }
            }
        }

        public bool IsOwnType
        {
            get { return isOwnType; }
            set { SetProperty(ref isOwnType, value); }
        }

        public string Type
        {
            get
            {
                return Facility.Type;
            }
            set
            {
                if (Facility.Type != value)
                {
                    Facility.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public ObservableCollection<string> Types
        {
            get { return new ObservableCollection<string>(FacilityType.GetList()); }
        }

        public string Address
        {
            get { return Facility.Address; }
            set
            {
                if (Facility.Address != value)
                {
                    Facility.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public string Code
        {
            get { return Facility.Code; }
            set
            {
                if (Facility.Code != value)
                {
                    Facility.Code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

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
                    FacilityTankViewModel tempFacilityTank = new FacilityTankViewModel(value) { FacilityViewModel = this };
                    selectedFacilityTank = null;
                    OnPropertyChanged(nameof(SelectedFacilityTank));
                    Application.Current.MainPage.Navigation.PushAsync(new FacilityTankPage(tempFacilityTank));
                }
            }
        }

        public bool IsValid
        {
            get { return (!String.IsNullOrEmpty(Name)); }
        }

        public bool IsBack
        {
            get { return isBack; }
            set { SetProperty(ref isBack, value); }
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

        private async void CreateFacilityTank()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название объекта", "Ok");
                return;
            }

            if (Facility.FacilityId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для создания емкости необходимо сохранить объект. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    FacilitiesViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Facility.FacilityId != 0)
            {
                FacilityTank newFacilityTank = new FacilityTank() { Facility = this.Facility };
                await Application.Current.MainPage.Navigation.PushAsync(new FacilityTankPage(new FacilityTankViewModel(newFacilityTank) { FacilityViewModel = this }));
            }
        }

        private void DeleteFacilityTank(object facilityTankInstance)
        {
            FacilityTankViewModel facilityTankViewModel = facilityTankInstance as FacilityTankViewModel;
            if (facilityTankViewModel.FacilityTank.FacilityTankId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.FacilityTanks.Remove(facilityTankViewModel.FacilityTank);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveFacilityTank(object facilityTankInstance)
        {
            FacilityTankViewModel facilityTankViewModel = facilityTankInstance as FacilityTankViewModel;
            if (facilityTankViewModel.FacilityTank != null)
            {
                if (!facilityTankViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название емкости", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (facilityTankViewModel.FacilityTank.FacilityTankId == 0)
                    {
                        db.Entry(facilityTankViewModel.FacilityTank).State = EntityState.Added;
                    }
                    else
                    {
                        db.FacilityTanks.Update(facilityTankViewModel.FacilityTank);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadFacilityTanks()
        {
            using (AppDbContext db = App.GetContext())
            {
                var facilityTanksDB = db.FacilityTanks.Where(ft => ft.FacilityId == Facility.FacilityId).ToList();
                FacilityTanks = new ObservableCollection<FacilityTank>(facilityTanksDB);
            }
        }
        #endregion Methods
    }
}
