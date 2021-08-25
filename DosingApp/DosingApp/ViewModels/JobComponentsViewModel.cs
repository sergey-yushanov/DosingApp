using CsvHelper;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.Files;
using DosingApp.Models.Screen;
using DosingApp.Models.WebSocket;
using DosingApp.Services;
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
        private bool isCompleted;
        private Color progressBarColor;
        private string title;

        // loop variables
        //private ObservableCollection<JobComponent> jobComponentsDosed;
        //private ObservableCollection<JobComponent> jobComponentsError;
        private CollectorLoop collectorLoop;
        private float progress;
        private bool isPause;

        //

        public ICommand StartJobCommand { get; protected set; }
        public ICommand StopJobCommand { get; protected set; }
        public ICommand PauseJobCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        public WebSocketService WebSocketService { get; protected set; }
        #endregion Attributes

        #region Constructor
        public JobComponentsViewModel(Job job, List<JobComponent> jobComponents)
        {
            Job = job;
            JobComponents = new ObservableCollection<JobComponent>(jobComponents);
            Title = "Задание: " + Job.Name + "\nКомпоненты";
            StartJobCommand = new Command(StartJob);
            StopJobCommand = new Command(StopJob);
            PauseJobCommand = new Command(PauseJob);
            BackCommand = new Command(Back);

            WebSocketService = new WebSocketService();
            if (WebSocketService.Mixer != null)
            {
                MakeRequirements(jobComponents);
                WebSocketSendRequirements();
            }
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

        public bool IsCompleted
        {
            get 
            {
                ProgressBarColor = (Color)(isCompleted ? Application.Current.Resources["OrangeColor"] : Application.Current.Resources["GreenColor"]);
                return isCompleted; 
            }
            set { SetProperty(ref isCompleted, value); }
        }

        public Color ProgressBarColor
        {
            get { return progressBarColor; }
            set { SetProperty(ref progressBarColor, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsPause
        {
            get { return isPause; }
            set { SetProperty(ref isPause, value); }
        }

        public float Progress
        {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }

        public CollectorScreen Collector
        {
            get { return WebSocketService.Collector; }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Back2Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Back3Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 2]);
            Application.Current.MainPage.Navigation.PopAsync();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void StartJob()
        {
            //UserDialogs.Instance.ShowLoading("Смешивание", MaskType.None);
            WebSocketService.CollectorLoopMessage(1, new CollectorLoop { CommandStart = true });
        }

        private void StopJob()
        {
            //UserDialogs.Instance.HideLoading();
            WebSocketService.CollectorLoopMessage(1, new CollectorLoop { CommandStop = true });
            Back3Pages();
        }

        private void PauseJob()
        {
            WebSocketService.CollectorLoopMessage(1, new CollectorLoop { CommandPause = true });
        }
        #endregion Commands

        #region Methods
        public void MakeRequirements(List<JobComponent> jobComponents)
        {
            var valveNums = new List<int>();
            var requiredVolumes = new List<float>();
            
            foreach (var jobComponent in jobComponents)
            {
                if (jobComponent.Dispenser == DispenserSuffix.Carrier || jobComponent.Dispenser == DispenserSuffix.Dry)
                    continue;
                else
                    valveNums.Add(jobComponent.GetDispenserNumber());

                requiredVolumes.Add((float)jobComponent.Volume);
            }

            collectorLoop = new CollectorLoop
            {
                ValveNums = valveNums,
                RequiredVolumes = requiredVolumes
            };
        }

        public void WebSocketSendRequirements()
        {
            WebSocketService.CollectorLoopMessage(1, collectorLoop);
        }

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
