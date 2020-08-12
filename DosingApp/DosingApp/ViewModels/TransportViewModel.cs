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

        #endregion Services

        #region Attributes
        TransportsViewModel transportsViewModel;
        public Transport Transport { get; private set; }
        private bool isBack;
        private string title;
        private float? volume;

        private ObservableCollection<TransportTank> transportTanks;
        private TransportTank selectedTransportTank;

        public ICommand EditTanksCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public TransportViewModel(Transport transport)
        {
            Transport = transport;
            IsBack = true;
            LoadTransportTanks();

            EditTanksCommand = new Command(EditTransportTanksAsync);
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

        public float? Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
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
                Volume = value != null ? value.Volume : null;
                SetProperty(ref selectedTransportTank, value); 
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

        private async void EditTransportTanksAsync()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя транспорта", "Ok");
                return;
            }

            if (Transport.TransportId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для перехода к списку емкостей необходимо сохранить транспорт. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    TransportsViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Transport.TransportId != 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new TransportTanksPage(new TransportTanksViewModel(Transport)));
            }
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
            InitSelectedTransportTank();
        }

        public void InitSelectedTransportTank()
        {
            SelectedTransportTank = TransportTanks.FirstOrDefault(ft => ft.IsUsedTank);
            Volume = SelectedTransportTank != null ? SelectedTransportTank.Volume : null;
        }
        #endregion Methods
    }
}
