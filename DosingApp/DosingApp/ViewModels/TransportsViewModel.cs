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
                using (AppDbContext db = App.GetContext())
                {
                    db.Transports.Remove(transportViewModel.Transport);
                    db.SaveChanges();
                }
            }
            LoadTransports();
            Back();
        }

        private void SaveTransport(object transportInstance)
        {
            TransportViewModel transportViewModel = transportInstance as TransportViewModel;
            if (transportViewModel.Transport != null && transportViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (transportViewModel.Transport.TransportId == 0)
                    {
                        db.Entry(transportViewModel.Transport).State = EntityState.Added;
                    }
                    else
                    {
                        db.Transports.Update(transportViewModel.Transport);
                    }
                    db.SaveChanges();
                }

                SetSelectedTransportTank(transportViewModel);
            }
            LoadTransports();
            if (transportViewModel.IsBack)
            {
                Back();
            }
        }
        #endregion Commands

        #region Methods
        public void LoadTransports()
        {
            using (AppDbContext db = App.GetContext())
            {
                Transports = new ObservableCollection<Transport>(db.Transports.ToList());
            }
        }

        private void SetSelectedTransportTank(TransportViewModel transportViewModel)
        {
            if (transportViewModel.SelectedTransportTank != null)
            {
                transportViewModel.TransportTanks.ForEach(ft => ft.IsUsedTank = false);
                transportViewModel.TransportTanks.FirstOrDefault(ft => ft.TransportTankId == transportViewModel.SelectedTransportTank.TransportTankId).IsUsedTank = true;

                using (AppDbContext db = App.GetContext())
                {
                    db.TransportTanks.UpdateRange(transportViewModel.TransportTanks);
                    db.SaveChanges();
                }
            }
        }
        #endregion Methods
    }
}
