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

        #endregion Services

        #region Attributes
        private ObservableCollection<TransportTank> transportTanks;
        private TransportTank selectedTransportTank;
        private string title;

        public Transport Transport { get; private set; }

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportTanksViewModel(Transport transport)
        {
            Transport = transport;
            LoadTransportTanks();
            Title = "Транспорт: " + Transport.Name + "\nСписок емкостей";

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

        private void CreateTransportTank()
        {
            TransportTank newTransportTank = new TransportTank
            {
                Transport = this.Transport,
                IsUsedTank = false
            };
            Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(new TransportTankViewModel(newTransportTank) { TransportTanksViewModel = this }));
        }

        private void DeleteTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank.TransportTankId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.TransportTanks.Remove(transportTankViewModel.TransportTank);
                    db.SaveChanges();
                }
            }
            LoadTransportTanks();
            Back();
        }

        private void SaveTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank != null && transportTankViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (transportTankViewModel.TransportTank.TransportTankId == 0)
                    {
                        db.Entry(transportTankViewModel.TransportTank).State = EntityState.Added;
                    }
                    else
                    {
                        db.TransportTanks.Update(transportTankViewModel.TransportTank);
                    }
                    db.SaveChanges();
                }
            }
            LoadTransportTanks();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadTransportTanks()
        {
            using (AppDbContext db = App.GetContext())
            {
                var transportTanksDB = db.TransportTanks.Where(ft => ft.TransportId == Transport.TransportId).ToList();
                TransportTanks = new ObservableCollection<TransportTank>(transportTanksDB);
            }
        }
        #endregion Methods

    }
}
