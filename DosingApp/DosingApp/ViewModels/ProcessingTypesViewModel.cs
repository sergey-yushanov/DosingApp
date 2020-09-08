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
    public class ProcessingTypesViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<ProcessingType> processingTypes;
        private ProcessingType selectedProcessingType;

        public ICommand CreateCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ProcessingTypesViewModel()
        {
            CreateCommand = new Command(CreateProcessingType);
            DeleteCommand = new Command(DeleteProcessingType);
            SaveCommand = new Command(SaveProcessingType);
            BackCommand = new Command(Back);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<ProcessingType> ProcessingTypes
        {
            get { return processingTypes; }
            set { SetProperty(ref processingTypes, value); } 
        }

        public ProcessingType SelectedProcessingType
        {
            get { return selectedProcessingType; }
            set
            {
                if (selectedProcessingType != value)
                {
                    ProcessingTypeViewModel tempProcessingType = new ProcessingTypeViewModel(value) { ProcessingTypesViewModel = this };
                    selectedProcessingType = null;
                    OnPropertyChanged(nameof(SelectedProcessingType));
                    Application.Current.MainPage.Navigation.PushAsync(new ProcessingTypePage(tempProcessingType));
                }
            }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void CreateProcessingType()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ProcessingTypePage(new ProcessingTypeViewModel(new ProcessingType()) { ProcessingTypesViewModel = this }));
        }

        private void DeleteProcessingType(object processingTypeInstance)
        {
            ProcessingTypeViewModel processingTypeViewModel = processingTypeInstance as ProcessingTypeViewModel;
            if (processingTypeViewModel.ProcessingType != null && processingTypeViewModel.ProcessingType.ProcessingTypeId != 0)
            {
                using (AppDbContext db = App.GetContext())
                {
                    db.ProcessingTypes.Remove(processingTypeViewModel.ProcessingType);
                    db.SaveChanges();
                }
            }
            Back();
        }

        private void SaveProcessingType(object processingTypeInstance)
        {
            ProcessingTypeViewModel processingTypeViewModel = processingTypeInstance as ProcessingTypeViewModel;
            if (processingTypeViewModel.ProcessingType != null)
            {
                if (!processingTypeViewModel.IsValid)
                {
                    Application.Current.MainPage.DisplayAlert("Предупреждение", "Задайте название вида обработки", "Ok");
                    return;
                }

                using (AppDbContext db = App.GetContext())
                {
                    if (processingTypeViewModel.ProcessingType.ProcessingTypeId == 0)
                    {
                        db.Entry(processingTypeViewModel.ProcessingType).State = EntityState.Added;
                    }
                    else
                    {
                        db.ProcessingTypes.Update(processingTypeViewModel.ProcessingType);
                    }
                    db.SaveChanges();
                }
            }
            Back();
        }
        #endregion Commands

        #region Methods
        public void LoadProcessingTypes()
        {
            using (AppDbContext db = App.GetContext())
            {
                ProcessingTypes = new ObservableCollection<ProcessingType>(db.ProcessingTypes.ToList());
            }
        }
        #endregion Methods

    }
}
