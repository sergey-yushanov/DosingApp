using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
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

        public ICommand CreateTankCommand { get; protected set; }
        public ICommand DeleteTankCommand { get; protected set; }
        public ICommand SaveTankCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportViewModel(Transport transport)
        {
            db = App.GetContext();
            Transport = transport;
            Tanks = new ObservableCollection<TransportTank>();
            LoadTanks();

            CreateTankCommand = new Command(CreateTank);
            DeleteTankCommand = new Command(DeleteTank);
            SaveTankCommand = new Command(SaveTank);
        }
        #endregion Constructor

        #region Properties
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

        public ObservableCollection<TransportTank> Tanks
        {
            get { return tanks; }
            set { SetProperty(ref tanks, value); }
        }

/*        public TransportTank Tank
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
*/
        public bool IsValid
        {
            get { return (!string.IsNullOrEmpty(Name)); }
        }
        #endregion Properties

        #region Commands
        private void CreateTank()
        {
            TransportTank tank = new TransportTank();// { Transport = this.Transport };
            Tanks.Add(tank);

            //db.Entry(tank).State = EntityState.Added;
            //db.SaveChanges();
        }

        private void DeleteTank(object transportInstance)
        {
            /*TransportViewModel transportViewModel = transportInstance as TransportViewModel;
            if (transportViewModel.Transport != null && transportViewModel.Transport.TransportId != 0)
            {
                db.Transports.Attach(transportViewModel.Transport);
                db.Transports.Remove(transportViewModel.Transport);
                db.SaveChanges();
                Back();
            }*/
        }

        private void SaveTank(object transportInstance)
        {
            /*TransportViewModel transportViewModel = transportInstance as TransportViewModel;
            if (transportViewModel.Transport != null && transportViewModel.IsValid)
            {
                if (transportViewModel.Transport.TransportId == 0)
                {
                    //db.Transports.Add(transportViewModel.Transport);
                    db.Entry(transportViewModel.Transport).State = EntityState.Added;
                }
                else
                {
                    db.Transports.Attach(transportViewModel.Transport);
                    db.Transports.Update(transportViewModel.Transport);
                }
                db.SaveChanges();
            }
            Back();*/
        }
        #endregion Commands

        #region Methods
        public void LoadTanks()
        {
            //var tanksDB = db.TransportTanks.Where(t => t.TransportId == Transport.TransportId).ToList();
            //Tanks = new ObservableCollection<TransportTank>(tanksDB);
        }
        #endregion Methods
    }
}
