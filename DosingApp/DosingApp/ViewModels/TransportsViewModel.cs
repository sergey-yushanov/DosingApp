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
        //private readonly DataService<Transport> dataServiceTransports;
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
            //CreateTransports();

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
                    if (transportViewModel.Transport.TransportTanks != null)
                    {
                        db.TransportTanks.AttachRange(transportViewModel.Transport.TransportTanks);
                        db.TransportTanks.UpdateRange(transportViewModel.Transport.TransportTanks);
                    }
                }
                db.SaveChanges();
            }
            LoadTransports();
            //transportViewModel.SetIsEditTanksEnabled();
            //Back();
        }
        #endregion Commands

        #region Methods
        public void LoadTransports()
        {
            Transports = new ObservableCollection<Transport>(db.Transports.ToList());
        }

        private void CreateTransports()
        {
/*            var transports = new List<Transport>()
            {
                new Transport { Name = "Transport 1", Number = "tr1" },
                new Transport { Name = "Transport 2", Number = "tr2" },
                new Transport { Name = "Transport 3", Number = "tr3" }
            };

            db.Transports.AddRange(transports);
            db.SaveChanges();*/
        }
        #endregion Methods

    }
}
