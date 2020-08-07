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
    public class TransportsViewModel : BaseViewModel
    {
        #region Services
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        private ObservableCollection<Transport> transports;
        private Transport selectedTransport;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportsViewModel()
        {
            db = App.GetContext();
            LoadTransports();

            CreateCommand = new Command(CreateTransport);
            DeleteCommand = new Command(DeleteTransport);
            SaveCommand = new Command(SaveTransport);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Transport> Transports
        {
            get { return transports; }
            set { SetProperty(ref transports, value); }
        }

        public Transport SelectedTransport
        {
            get { return selectedTransport; }
            set
            {
                if (selectedTransport != value)
                {
                    TransportViewModel tempTransport = new TransportViewModel(value) { TransportsViewModel = this };
                    selectedTransport = null;
                    OnPropertyChanged(nameof(SelectedTransport));
                    Application.Current.MainPage.Navigation.PushAsync(new TransportPage(tempTransport));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateTransport()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TransportPage(new TransportViewModel(new Transport()) { TransportsViewModel = this }));
        }

        private void DeleteTransport(object transportInstance)
        {
            TransportViewModel transportViewModel = transportInstance as TransportViewModel;
            if (transportViewModel.Transport != null && transportViewModel.Transport.TransportId != 0)
            {
                db.Transports.Attach(transportViewModel.Transport);
                db.Transports.Remove(transportViewModel.Transport);
                db.SaveChanges();
            }
            LoadTransports();
            Back();
        }

        private void SaveTransport(object transportInstance)
        {
            TransportViewModel transportViewModel = transportInstance as TransportViewModel;
            if (transportViewModel.Transport != null && transportViewModel.IsValid)
            {
                if (transportViewModel.Transport.TransportId == 0)
                {
                    db.Entry(transportViewModel.Transport).State = EntityState.Added;
                }
                else
                {
                    db.Transports.Attach(transportViewModel.Transport);
                    db.Transports.Update(transportViewModel.Transport);
                    //if (transportViewModel.TransportTanks != null)
                    //{
                        //db.TransportTanks.AttachRange(transportViewModel.TransportTanks);
                        //db.TransportTanks.UpdateRange(transportViewModel.TransportTanks);
                    //}
                }
                db.SaveChanges();
            }
            LoadTransports();
            Back();
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
            //LoadTransportTanks();
            Back();
        }

        private void SaveTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank != null && transportTankViewModel.IsValid)
            {
                if (transportTankViewModel.TransportTank.TransportTankId == 0)
                {
                    db.Entry(transportTankViewModel.TransportTank).State = EntityState.Added;
                }
                else
                {
                    db.TransportTanks.Attach(transportTankViewModel.TransportTank);
                    db.TransportTanks.Update(transportTankViewModel.TransportTank);
                }
                db.SaveChanges();
            }
            //LoadTransportTanks();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadTransports()
        {
            Transports = new ObservableCollection<Transport>(db.Transports.ToList());
        }
        #endregion Methods

    }
}
