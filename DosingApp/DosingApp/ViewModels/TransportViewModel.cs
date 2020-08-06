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

        private ObservableCollection<TransportTank> transportTanks;
        private TransportTank selectedTransportTank;
        private string title;
        //private bool isEditTanksEnabled;

        //private TransportTank tank;

        public ICommand EditTanksCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportViewModel(Transport transport)
        {
            db = App.GetContext();
            Transport = transport;
            //LoadTransportTanks();
            //SelectedTransportTank = GetUsedTransportTank();

            EditTanksCommand = new Command(EditTanks);
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
                SetProperty(ref selectedTransportTank, value);
                //SetUsedTransportTank(selectedTransportTank);
            }
        }

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

        /*public string Tank
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

        public float? Volume
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
        }*/

        public bool IsValid
        {
            get { return (!string.IsNullOrEmpty(Name)); }
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

        private async void EditTanks()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя транспорта.", "Ok");
                return;
            }
            
            if (Transport.TransportId == 0)
            {
                bool result = await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для перехода к списку емкостей необходимо сохранить транспорт. Выполнить сохранение", "Да", "Нет");
                if (result)
                {
                    TransportsViewModel.SaveCommand.Execute(this);
                }
            }

            if (Transport.TransportId != 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new TransportTanksPage(this));
            }
        }
        #endregion Commands

        #region Methods
        public void LoadTransportTanks()
        {
            var transportTanksDB = db.TransportTanks.Where(tt => tt.TransportId == Transport.TransportId).ToList();
            TransportTanks =  new ObservableCollection<TransportTank>(transportTanksDB);
        }

        private void SetUsedTransportTank(TransportTank transportTank)
        {
            if (transportTank == null)
                return;

            if (!TransportTanks.Contains(transportTank))
            {
                throw new ArgumentException("Tank is not in list");
            }
            var currentUsedTransportTank = GetUsedTransportTank();
            if (currentUsedTransportTank != null)
            {
                currentUsedTransportTank.IsUsedTank = false;
            }
            transportTank.IsUsedTank = true;
        }

        private TransportTank GetUsedTransportTank()
        {
            var currentUsedTransportTank = TransportTanks.FirstOrDefault(tt => tt.IsUsedTank == true);
            return currentUsedTransportTank;
        }
        #endregion Methods
    }
}
