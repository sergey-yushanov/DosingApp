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
        private ObservableCollection<JobComponentScreen> jobComponentScreens;

        private JobScreen jobScreen;

        private bool isRunning;
        private bool isCompleted;
        private Color progressBarColor;
        private string title;

        // loop variables
        //private ObservableCollection<JobComponent> jobComponentsDosed;
        //private ObservableCollection<JobComponent> jobComponentsError;
        private CommonLoop commonLoop;
        private CollectorLoop collectorLoop;
        private SingleDosLoop singleDosLoop;
        //private float progressBarValue;
        //private float requiredVolume;
        //private float dosedVolume;
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

            JobScreen = new JobScreen(jobComponents);

            //JobComponents = new ObservableCollection<JobComponent>(jobComponents);

            //requiredVolume = 0;

            //JobComponentScreens = new ObservableCollection<JobComponentScreen>();
            //for(int i = 0; i < jobComponents.Count; i++)
            //{
            //    JobComponentScreens.Add(new JobComponentScreen(jobComponents[i]));
            //    //requiredVolume += (float)jobComponents[i].Volume;
            //}

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
        public JobScreen JobScreen
        {
            get { return jobScreen; }
            set { SetProperty(ref jobScreen, value); }
        }

        public ObservableCollection<JobComponent> JobComponents
        {
            get { return jobComponents; }
            set { SetProperty(ref jobComponents, value); }
        }

        public ObservableCollection<JobComponentScreen> JobComponentScreens
        {
            get { return jobComponentScreens; }
            set { SetProperty(ref jobComponentScreens, value); }
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

        //public float ProgressBarValue
        //{
        //    get { return progressBarValue; }
        //    set { SetProperty(ref progressBarValue, value); }
        //}

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

        public CollectorScreen Collector
        {
            get 
            {
                //UpdateJobComponents();
                return WebSocketService.Collector; 
            }
        }

        public SingleDosScreen SingleDos
        {
            get
            {
                //UpdateJobComponents();
                return WebSocketService.SingleDos;
            }
        }

        public CommonScreen Common
        {
            get 
            {
                //UpdateJobComponents();
                return WebSocketService.Common; 
            }
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
            WebSocketService.CommonLoopMessage(new CommonLoop { CommandStart = true });
        }

        private void StopJob()
        {
            WebSocketService.CommonLoopMessage(new CommonLoop { CommandStop = true });
            Back3Pages();
        }

        private void PauseJob()
        {
            WebSocketService.CommonLoopMessage(new CommonLoop { CommandPause = true });
        }
        #endregion Commands

        #region Methods
        public void MakeRequirements(List<JobComponent> jobComponents)
        {
            var valveNums = new List<int>();
            var requiredVolumes = new List<float>();
            float carrierRequiredVolume = 0;
            float singleRequiredVolume = 0;

            foreach (var jobComponent in jobComponents)
            {
                if (jobComponent.Dispenser == DispenserSuffix.Carrier)
                {
                    carrierRequiredVolume = (float)jobComponent.Volume;
                    continue;
                }

                if (jobComponent.Dispenser == DispenserSuffix.Dry)
                {
                    continue;
                }

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Collector) >= 0)
                {
                    valveNums.Add(jobComponent.GetDispenserNumber());
                    requiredVolumes.Add((float)jobComponent.Volume);
                    continue;
                }

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Single) >= 0)
                {
                    singleRequiredVolume = (float)jobComponent.Volume;
                    continue;
                }
            }

            collectorLoop = new CollectorLoop
            {
                ValveNums = valveNums,
                RequiredVolumes = requiredVolumes
            };

            commonLoop = new CommonLoop
            {
                CarrierRequiredVolume = carrierRequiredVolume
            };

            singleDosLoop = new SingleDosLoop
            {
                RequiredVolume = singleRequiredVolume
            };
        }

        public void WebSocketSendRequirements()
        {
            WebSocketService.CollectorLoopMessage(1, collectorLoop);
            WebSocketService.SingleLoopMessage(1, singleDosLoop);
            WebSocketService.CommonLoopMessage(commonLoop);
        }

        public void UpdateJobComponents()
        {
            JobScreen.Update(Common, Collector, SingleDos);
            //OnPropertyChanged(nameof(JobScreen));

            //dosedVolume = 0;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    JobComponentScreens[i].Update(Common, Collector);
            //    dosedVolume += (float)JobComponentScreens[i].DosedVolume;
            //}

            //ProgressBarValue = dosedVolume / requiredVolume;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    Console.WriteLine(JobComponentScreens[i].DosedVolume);
            //}
            //Console.WriteLine("=========");

            //OnPropertyChanged(nameof(JobComponentScreens));
        }

        //public void Update(CommonScreen common, CollectorScreen collector)
        //{
        //    Console.WriteLine(common.CarrierDosedVolume);
        //    Console.WriteLine(Dispenser);
            

        //    if (Dispenser == DispenserSuffix.Dry)
        //        return;

        //    if (Dispenser == DispenserSuffix.Carrier)
        //        DosedVolume = common.CarrierDosedVolume;
        //    else
        //        DosedVolume = collector.DosedVolumes[GetDispenserNumber() - 1];

        //    DosedVolumeError = (Volume - DosedVolume) / Volume * 100;
        //}

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
