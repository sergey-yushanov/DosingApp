using Acr.UserDialogs;
using CsvHelper;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.Files;
using DosingApp.Views;
using Microsoft.EntityFrameworkCore;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class JobComponentsViewModel : BaseViewModel
    {
        #region Attributes
        public Job Job { get; private set; }
        private ObservableCollection<JobComponent> jobComponents;
        private bool isRunning;
        private string title;

        public ICommand StartJobCommand { get; protected set; }
        public ICommand StopJobCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public JobComponentsViewModel(Job job, List<JobComponent> jobComponents)
        {
            Job = job;
            JobComponents = new ObservableCollection<JobComponent>(jobComponents);
            Title = "Задание: " + Job.Name;
            StartJobCommand = new Command(StartJob);
            StopJobCommand = new Command(StopJob);
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<JobComponent> JobComponents
        {
            get { return jobComponents; }
            set { SetProperty(ref jobComponents, value); }
        }

        public double? PartySize
        {
            get { return Job.PartySize; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        #endregion Properties

        #region Commands
        private void Back2Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void StartJob()
        {
            UserDialogs.Instance.ShowLoading("Смешивание", MaskType.None);
        }

        private void StopJob()
        {
            UserDialogs.Instance.HideLoading();
            Back2Pages();
        }

        #endregion Commands

        #region Methods
/*        public void LoadJobComponents()
        {
            using (AppDbContext db = App.GetContext())
            {
                var recipeComponentsDb = db.RecipeComponents.Where(rc => rc.RecipeId == Job.RecipeId);
            }
        }*/
        #endregion Methods

    }
}
