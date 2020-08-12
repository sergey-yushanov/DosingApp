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
    public class ApplicatorViewModel : BaseViewModel
    {
        #region Services

        #endregion Services

        #region Attributes
        ApplicatorsViewModel applicatorsViewModel;
        public Applicator Applicator { get; private set; }
        private bool isBack;
        private string title;

        private ObservableCollection<ApplicatorTank> applicatorTanks;
        private ApplicatorTank selectedApplicatorTank;

        public ICommand EditTanksCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ApplicatorViewModel(Applicator applicator)
        {
            Applicator = applicator;
            IsBack = true;
            LoadApplicatorTanks();

            EditTanksCommand = new Command(EditApplicatorTanksAsync);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ApplicatorsViewModel ApplicatorsViewModel
        {
            get { return applicatorsViewModel; }
            set { SetProperty(ref applicatorsViewModel, value); }
        }

        public string Name
        {
            get
            {
                Title = (Applicator.ApplicatorId == 0) ? "Новый аппликатор" : "Аппликатор: " + Applicator.Name;
                return Applicator.Name;
            }
            set
            {
                if (Applicator.Name != value)
                {
                    Applicator.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public ObservableCollection<ApplicatorTank> ApplicatorTanks
        {
            get { return applicatorTanks; }
            set { SetProperty(ref applicatorTanks, value); }
        }

        public ApplicatorTank SelectedApplicatorTank
        {
            get { return selectedApplicatorTank; }
            set { SetProperty(ref selectedApplicatorTank, value); }
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

        private async void EditApplicatorTanksAsync()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя аппликатора", "Ok");
                return;
            }

            if (Applicator.ApplicatorId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для перехода к списку емкостей необходимо сохранить аппликатор. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    ApplicatorsViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Applicator.ApplicatorId != 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ApplicatorTanksPage(new ApplicatorTanksViewModel(Applicator)));
            }
        }
        #endregion Commands

        #region Methods
        public void LoadApplicatorTanks()
        {
            using (AppDbContext db = App.GetContext())
            {
                var applicatorTanksDB = db.ApplicatorTanks.Where(ft => ft.ApplicatorId == Applicator.ApplicatorId).ToList();
                ApplicatorTanks = new ObservableCollection<ApplicatorTank>(applicatorTanksDB);
            }
            InitSelectedApplicatorTank();
        }

        public void InitSelectedApplicatorTank()
        {
            SelectedApplicatorTank = ApplicatorTanks.FirstOrDefault(ft => ft.IsUsedTank);
        }
        #endregion Methods
    }
}
