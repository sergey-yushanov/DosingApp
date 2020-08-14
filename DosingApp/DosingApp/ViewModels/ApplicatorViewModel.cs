using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class ApplicatorViewModel : BaseViewModel
    {
        #region Attributes
        ApplicatorsViewModel applicatorsViewModel;
        public Applicator Applicator { get; private set; }
        private bool isBack;
        private string title;

        private ObservableCollection<ApplicatorTank> applicatorTanks;
        private ApplicatorTank selectedApplicatorTank;

        public ICommand CreateTankCommand { get; protected set; }
        public ICommand DeleteTankCommand { get; protected set; }
        public ICommand SaveTankCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ApplicatorViewModel(Applicator applicator)
        {
            Applicator = applicator;
            IsBack = true;

            CreateTankCommand = new Command(CreateApplicatorTank);
            DeleteTankCommand = new Command(DeleteApplicatorTank);
            SaveTankCommand = new Command(SaveApplicatorTank);
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
            set
            {
                if (selectedApplicatorTank != value)
                {
                    ApplicatorTankViewModel tempApplicatorTank = new ApplicatorTankViewModel(value) { ApplicatorViewModel = this };
                    selectedApplicatorTank = null;
                    OnPropertyChanged(nameof(SelectedApplicatorTank));
                    Application.Current.MainPage.Navigation.PushAsync(new ApplicatorTankPage(tempApplicatorTank));
                }
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

        private async void CreateApplicatorTank()
        {
            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте имя аппликатора", "Ok");
                return;
            }

            if (Applicator.ApplicatorId == 0)
            {
                if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Для создания емкости необходимо сохранить аппликатор. Выполнить сохранение?", "Да", "Нет"))
                {
                    IsBack = false;
                    ApplicatorsViewModel.SaveCommand.Execute(this);
                    IsBack = true;
                }
            }

            if (Applicator.ApplicatorId != 0)
            {
                ApplicatorTank newApplicatorTank = new ApplicatorTank { Applicator = this.Applicator };
                await Application.Current.MainPage.Navigation.PushAsync(new ApplicatorTankPage(new ApplicatorTankViewModel(newApplicatorTank) { ApplicatorViewModel = this }));
            }
        }

        private void DeleteApplicatorTank(object applicatorTankInstance)
        {
            ApplicatorTankViewModel applicatorTankViewModel = applicatorTankInstance as ApplicatorTankViewModel;
            if (applicatorTankViewModel.ApplicatorTank.ApplicatorTankId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.ApplicatorTanks.Remove(applicatorTankViewModel.ApplicatorTank);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveApplicatorTank(object applicatorTankInstance)
        {
            ApplicatorTankViewModel applicatorTankViewModel = applicatorTankInstance as ApplicatorTankViewModel;
            if (applicatorTankViewModel.ApplicatorTank != null && applicatorTankViewModel.IsValid)
            {
                using (AppDbContext db = App.GetContext())
                {
                    if (applicatorTankViewModel.ApplicatorTank.ApplicatorTankId == 0)
                    {
                        db.Entry(applicatorTankViewModel.ApplicatorTank).State = EntityState.Added;
                    }
                    else
                    {
                        db.ApplicatorTanks.Update(applicatorTankViewModel.ApplicatorTank);
                    }
                    db.SaveChanges();
                }
            }
            Back();
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
        }
        #endregion Methods
    }
}
