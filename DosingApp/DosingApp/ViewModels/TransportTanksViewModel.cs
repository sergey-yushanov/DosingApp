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
    public class TransportTanksViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<TransportTank> dataServiceTransportTanks;
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        public TransportViewModel TransportViewModel { get; private set; }

        private ObservableCollection<TransportTank> transportTanks;
        private TransportTank selectedTransportTank;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportTanksViewModel(TransportViewModel transportViewModel)
        {
            TransportViewModel = transportViewModel;

            db = transportViewModel.db;
            LoadTransportTanks();

            CreateCommand = new Command(CreateTransportTank);
            DeleteCommand = new Command(DeleteTransportTank);
            SaveCommand = new Command(SaveTransportTank);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<TransportTank> TransportTanks
        {
            get { return transportTanks; }
            set { SetProperty(ref transportTanks, value); }
        }

        public TransportTank SelectedTransportTank
        {
            get { return selectedTransportTank; }
            set
            {
                if (selectedTransportTank != value)
                {
                    TransportTankViewModel tempTransportTank = new TransportTankViewModel(value) { TransportTanksViewModel = this };
                    selectedTransportTank = null;
                    OnPropertyChanged(nameof(SelectedTransportTank));
                    Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(tempTransportTank));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateTransportTank()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(new TransportTankViewModel(new TransportTank()) { TransportTanksViewModel = this }));
        }

        private void DeleteTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank != null && transportTankViewModel.TransportTank.TransportTankId != 0)
            {
                db.TransportTanks.Attach(transportTankViewModel.TransportTank);
                db.TransportTanks.Remove(transportTankViewModel.TransportTank);
                db.SaveChanges();
            }
            LoadTransportTanks();
            //TransportTanks = TransportViewModel.LoadTransportTanks();
            Back();
        }

        private void SaveTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank != null && transportTankViewModel.IsValid)
            {
                if (transportTankViewModel.TransportTank.TransportTankId == 0)
                {
                    transportTankViewModel.TransportTank.Transport = TransportViewModel.Transport;
                    db.Entry(transportTankViewModel.TransportTank).State = EntityState.Added;
                }
                else
                {
                    db.TransportTanks.Attach(transportTankViewModel.TransportTank);
                    db.TransportTanks.Update(transportTankViewModel.TransportTank);
                }
                db.SaveChanges();
            }
            LoadTransportTanks();
            //TransportTanks = TransportViewModel.LoadTransportTanks();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadTransportTanks()
        {
            var transportTanksDB = db.TransportTanks.Where(tt => tt.TransportId == TransportViewModel.Transport.TransportId).ToList();
            TransportTanks = new ObservableCollection<TransportTank>(transportTanksDB);
        }

        private void CreateTransportTanks()
        {
/*            var transportTanks = new List<TransportTank>()
            {
                new TransportTank { Name = "TransportTank 1", Code = "f1" },
                new TransportTank { Name = "TransportTank 2", Code = "f2" },
                new TransportTank { Name = "TransportTank 3", Code = "f3" }
            };

            db.TransportTanks.AddRange(transportTanks);
            db.SaveChanges();*/
        }
        #endregion Methods

    }
}
