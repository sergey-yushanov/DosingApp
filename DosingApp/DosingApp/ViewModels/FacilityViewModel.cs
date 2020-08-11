using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class FacilityViewModel : BaseViewModel
    {
        #region Services
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        FacilitiesViewModel facilitiesViewModel;
        public Facility Facility { get; private set; }
        private bool isBack;
        private string title;

        private ObservableCollection<FacilityTank> facilityTanks;
        private FacilityTank selectedFacilityTank;

        public ICommand EditTanksCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public FacilityViewModel(Facility facility)
        {
            db = App.GetContext();
            Facility = facility;
            IsBack = true;
            LoadFacilityTanks();
            //InitSelectedFacilityTank();

            EditTanksCommand = new Command(EditFacilityTanksAsync);
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

        public string Type
        {
            get { return Facility.Type; }
            set
            {
                if (Facility.Type != value)
                {
                    Facility.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
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

        public ObservableCollection<FacilityTank> FacilityTanks
        {
            get { return facilityTanks; }
            set { SetProperty(ref facilityTanks, value); }
        }

        public FacilityTank SelectedFacilityTank
        {
            get { return selectedFacilityTank; }
            set { SetProperty(ref selectedFacilityTank, value); }
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

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name));
            }
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

        private async void EditFacilityTanksAsync()
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new FacilityTanksPage(new FacilityTanksViewModel(Facility)));
            
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя объекта", "Ok");
                return;
            }

            if (Facility.FacilityId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для перехода к списку емкостей необходимо сохранить объект. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    FacilitiesViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Facility.FacilityId != 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new FacilityTanksPage(new FacilityTanksViewModel(Facility)));
            }
        }
        #endregion Commands

        #region Methods
        public void LoadFacilityTanks()
        {
            var facilityTanksDB = db.FacilityTanks.Where(ft => ft.FacilityId == Facility.FacilityId).ToList();
            FacilityTanks = new ObservableCollection<FacilityTank>(facilityTanksDB);
            InitSelectedFacilityTank();
        }

        public void InitSelectedFacilityTank()
        {
            SelectedFacilityTank = FacilityTanks.FirstOrDefault(ft => ft.IsUsedTank);
        }
        #endregion Methods
    }
}
