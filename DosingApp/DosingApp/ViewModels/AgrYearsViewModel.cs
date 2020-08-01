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
    public class AgrYearsViewModel : BaseViewModel
    {
        #region Services
        //private readonly DataService<AgrYear> dataServiceAgrYears;
        public readonly AppDbContext db;
        #endregion Services

        #region Attributes
        private ObservableCollection<AgrYear> agrYears;
        private AgrYear selectedAgrYear;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public AgrYearsViewModel()
        {
            db = App.GetContext();
            LoadAgrYears();
            //CreateAgrYears();

            CreateCommand = new Command(CreateAgrYear);
            DeleteCommand = new Command(DeleteAgrYear);
            SaveCommand = new Command(SaveAgrYear);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<AgrYear> AgrYears 
        {
            get { return agrYears; }
            set { SetProperty(ref agrYears, value); }
        }

        public AgrYear SelectedAgrYear
        {
            get { return selectedAgrYear; }
            set
            {
                if (selectedAgrYear != value)
                {
                    AgrYearViewModel tempAgrYear = new AgrYearViewModel(value) { AgrYearsViewModel = this };
                    selectedAgrYear = null;
                    OnPropertyChanged(nameof(SelectedAgrYear));
                    Application.Current.MainPage.Navigation.PushAsync(new AgrYearPage(tempAgrYear));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateAgrYear()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AgrYearPage(new AgrYearViewModel(new AgrYear()) { AgrYearsViewModel = this }));
        }

        private void DeleteAgrYear(object agrYearInstance)
        {
            AgrYearViewModel agrYearViewModel = agrYearInstance as AgrYearViewModel;
            if (agrYearViewModel.AgrYear != null && agrYearViewModel.AgrYear.AgrYearId != 0)
            {
                db.AgrYears.Attach(agrYearViewModel.AgrYear);
                db.AgrYears.Remove(agrYearViewModel.AgrYear);
                db.SaveChanges();
                LoadAgrYears();
                Back();
            }
        }

        private void SaveAgrYear(object agrYearInstance)
        {
            AgrYearViewModel agrYearViewModel = agrYearInstance as AgrYearViewModel;
            if (agrYearViewModel.AgrYear != null && agrYearViewModel.IsValid)
            {
                if (agrYearViewModel.AgrYear.AgrYearId == 0)
                {
                    db.Entry(agrYearViewModel.AgrYear).State = EntityState.Added;
                }
                else
                {
                    db.AgrYears.Attach(agrYearViewModel.AgrYear);
                    db.AgrYears.Update(agrYearViewModel.AgrYear);
                }
                db.SaveChanges();
            }
            LoadAgrYears();
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadAgrYears()
        {
            AgrYears = new ObservableCollection<AgrYear>(db.AgrYears.ToList());
        }

        private void CreateAgrYears()
        {
            var agrYears = new List<AgrYear>()
            {
                new AgrYear { Name = "AgrYear 1", FinishDate = "today" },
                new AgrYear { Name = "AgrYear 2", FinishDate = "tomorrow" },
                new AgrYear { Name = "AgrYear 3", FinishDate = "day after tomorrow" }
            };

            db.AgrYears.AddRange(agrYears);
            db.SaveChanges();
        }
        #endregion Methods

    }
}
