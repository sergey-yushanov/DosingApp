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

namespace DosingApp.ViewModels
{
    public class TransportViewModel : BaseViewModel
    {
        #region Services
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        TransportsViewModel transportsViewModel;
        public Transport Transport { get; private set; }

        private ObservableCollection<TransportTank> tanks;
        private TransportTank selectedTank;
        
        //private TransportTank tank;

        public ICommand EditTanksCommand { get; protected set; }

        public ICommand CreateTankCommand { get; protected set; }
        public ICommand DeleteTankCommand { get; protected set; }
        public ICommand SaveTankCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportViewModel(Transport transport)
        {
            db = App.GetContext();
            Transport = transport;
            //LoadTanks();

/*            EditTanksCommand = new Command(EditTanks);
            CreateTankCommand = new Command(CreateTank);
            DeleteTankCommand = new Command(DeleteTank);
            SaveTankCommand = new Command(SaveTank);*/
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
/*        public ObservableCollection<TransportTank> Tanks
        {
            get { return tanks; }
            set { SetProperty(ref tanks, value); }
        }

        public TransportTank SelectedTank
        {
            get { return selectedTank; }
            set
            {
                if (selectedTank != value)
                {
                    TransportTankViewModel tempTank = new TransportTankViewModel(value) { TransportViewModel = this };
                    selectedTank = null;
                    OnPropertyChanged(nameof(SelectedTank));
                    Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(tempTank));
                }
            }
        }*/

        public TransportsViewModel TransportsViewModel
        {
            get { return transportsViewModel; }
            set { SetProperty(ref transportsViewModel, value); }
        }

        public string Name
        {
            get { return Transport.Name; }
            set
            {
                if (Transport.Name != value)
                {
                    Transport.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Number
        {
            get { return Transport.Number; }
            set
            {
                if (Transport.Number != value)
                {
                    Transport.Number = value;
                    OnPropertyChanged(nameof(Number));
                }
            }
        }

        public string Tank
        {
            get { return Transport.Tank; }
            set
            {
                if (Transport.Tank != value)
                {
                    Transport.Tank = value;
                    OnPropertyChanged(nameof(Tank));
                }
            }
        }

        public float Volume
        {
            get { return Transport.Volume; }
            set
            {
                if (Transport.Volume != value)
                {
                    Transport.Volume = value;
                    OnPropertyChanged(nameof(Volume));
                }
            }
        }

        public bool IsValid
        {
            get { return (!string.IsNullOrEmpty(Name)); }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

/*        private void EditTanks()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TransportTanksPage(this));
        }

        private void CreateTank()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(new TransportTankViewModel(new TransportTank()) { TransportViewModel = this }));
        }

        private void DeleteTank(object tankInstance)
        {
            TransportTankViewModel transportTankViewModel = tankInstance as TransportTankViewModel;
            if (transportTankViewModel != null)
            {
                Tanks.Remove(transportTankViewModel.Tank);
            }
            Back();
        }

        private void SaveTank(object tankInstance)
        {
            TransportTankViewModel transportTankViewModel = tankInstance as TransportTankViewModel;
            if (transportTankViewModel != null && transportTankViewModel.IsValid)
            {
                Tanks.Add(transportTankViewModel.Tank);
            }
            Back();
        }
*/        
        #endregion Commands

        #region Methods
        public void LoadTanks()
        {
/*            var tanksDB = db.TransportTanks.Where(t => t.TransportId == Transport.TransportId).ToList();
            Tanks = new ObservableCollection<TransportTank>(tanksDB);*/
        }

        /*private void SetUsedTank(TransportTank tank)
        {
            if (!Tanks.Contains(tank))
            {
                throw new ArgumentException("Tank is not in list");
            }
            var currentUsedTank = GetUsedTank();
            if (currentUsedTank != null)
            {
                currentUsedTank.IsUsedTank = false;
            }
            tank.IsUsedTank = true;
        }

        private TransportTank GetUsedTank()
        {
            var currentUsedTank = Tanks.FirstOrDefault(tt => tt.IsUsedTank == true);
            return currentUsedTank;
        }*/
        #endregion Methods
    }
}
