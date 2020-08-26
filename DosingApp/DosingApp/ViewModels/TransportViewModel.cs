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
    public class TransportViewModel : BaseViewModel
    {
        #region Attributes
        TransportsViewModel transportsViewModel;
        public Transport Transport { get; private set; }
        private bool isBack;    // need to page navigation while saving foreign key values
        private string title;

        private ObservableCollection<TransportTank> transportTanks;
        private TransportTank selectedTransportTank;

        public ICommand CreateTankCommand { get; protected set; }
        public ICommand DeleteTankCommand { get; protected set; }
        public ICommand SaveTankCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportViewModel(Transport transport)
        {
            Transport = transport;
            IsBack = true;

            CreateTankCommand = new Command(CreateTransportTank);
            DeleteTankCommand = new Command(DeleteTransportTank);
            SaveTankCommand = new Command(SaveTransportTank);
            BackCommand = new Command(Back);
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
            get
            {
                Title = (Transport.TransportId == 0) ? "Новый транспорт" : "Транспорт: " + Transport.Name;
                return Transport.Name;
            }
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
                    TransportTankViewModel tempTransportTank = new TransportTankViewModel(value) { TransportViewModel = this };
                    selectedTransportTank = null;
                    OnPropertyChanged(nameof(SelectedTransportTank));
                    Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(tempTransportTank));
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

        private async void CreateTransportTank()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название транспорта", "Ok");
                return;
            }

            if (Transport.TransportId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для создания емкости необходимо сохранить транспорт. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    TransportsViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Transport.TransportId != 0)
            {
                TransportTank newTransportTank = new TransportTank() { Transport = this.Transport };
                await Application.Current.MainPage.Navigation.PushAsync(new TransportTankPage(new TransportTankViewModel(newTransportTank) { TransportViewModel = this }));
            }
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
            Back();
        }

        private void SaveTransportTank(object transportTankInstance)
        {
            TransportTankViewModel transportTankViewModel = transportTankInstance as TransportTankViewModel;
            if (transportTankViewModel.TransportTank != null)
            {
                if (!transportTankViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название емкости", "Ok");
                    return;
                }

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
