using CsvHelper;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.Files;
using DosingApp.Models.Modbus;
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
using System.Threading;
using System.Threading.Tasks;
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
        private static readonly int nCollectors = Mixer.MaxCollectors;
        private static readonly int nDoseValves = 4;
        private ObservableCollection<CollectorLoop> collectorsLoop;

        //private SingleDosLoop singleDosLoop;
        private VolumeDosLoop volumeDosLoop;
        private PowderDosLoop powderDosLoop;
        //private double progressBarValue;
        //private double requiredVolume;
        //private double dosedVolume;
        private bool isPause;

        private bool isLoopNotPause;
        private bool isLoopCont;
        private bool isLoopStart;

        private bool isLoopDone;
        private bool isLoopReported;

        private bool isLoopWasActive;

        private string statusText;
        private Color statusColor;

        private string airTemperature;

        private TimeSpan dosingTime;

        public ICommand StartJobCommand { get; protected set; }
        public ICommand StopJobCommand { get; protected set; }
        public ICommand PauseJobCommand { get; protected set; }
        public ICommand ContJobCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        //public WebSocketService WebSocketService { get; protected set; }
        public ModbusService ModbusService { get; protected set; }

        public bool IsExitJob { get; set; }
        public bool IsNotInitializedLoop { get; set; }
        #endregion Attributes

        #region Constructor
        public JobComponentsViewModel(Job job, List<JobComponent> jobComponents)
        {
            IsLoopDone = false;
            isLoopReported = false;

            Job = job;
            JobScreen = new JobScreen(jobComponents);

            CollectorsLoop = new ObservableCollection<CollectorLoop>();
            for (int i = 0; i < nCollectors; i++)
            {
                CollectorsLoop.Add(new CollectorLoop(i));
            }

            //JobComponents = new ObservableCollection<JobComponent>(jobComponents);

            //requiredVolume = 0;

            //JobComponentScreens = new ObservableCollection<JobComponentScreen>();
            //for(int i = 0; i < jobComponents.Count; i++)
            //{
            //    JobComponentScreens.Add(new JobComponentScreen(jobComponents[i]));
            //    //requiredVolume += (double)jobComponents[i].Volume;
            //}

            Title = "Задание: " + Job.Name + "\nКомпоненты";
            StartJobCommand = new Command(StartJob);
            StopJobCommand = new Command(StopJob);
            PauseJobCommand = new Command(PauseJob);
            ContJobCommand = new Command(ContJob);
            BackCommand = new Command(Back);

            //WebSocketService = new WebSocketService();
            //if (WebSocketService.Mixer != null)
            //{
            //MakeRequirements(jobComponents);
            //WebSocketSendRequirements();
            //ModbusSendRequirements();
            //}

            ModbusService = new ModbusService();
            IsNotInitializedLoop = false;
            //if (!ModbusService.IsConnected)
            //{
            //    IsNotInitializedLoop = true;
            //}

            if (ModbusService.Mixer != null)
            {
                ModbusService.WriteSingleRegister(CommonModbus.LoopClear());
                MakeRequirements(jobComponents);
                ModbusSendRequirements();
            }

            IsExitJob = false;
            //ModbusService = new ModbusService();
            UpdateJobComponents();

            isLoopWasActive = false;
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

        //public double ProgressBarValue
        //{
        //    get { return progressBarValue; }
        //    set { SetProperty(ref progressBarValue, value); }
        //}

        public string AirTemperature
        {
            get { return airTemperature; }
            set { SetProperty(ref airTemperature, value); }
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

        public bool IsLoopNotPause
        {
            get { return isLoopNotPause; }
            set { SetProperty(ref isLoopNotPause, value); }
        }

        public bool IsLoopCont
        {
            get { return isLoopCont; }
            set { SetProperty(ref isLoopCont, value); }
        }

        public bool IsLoopStart
        {
            get { return isLoopStart; }
            set { SetProperty(ref isLoopStart, value); }
        }

        public bool IsLoopDone
        {
            get { return isLoopDone; }
            set { SetProperty(ref isLoopDone, value); }
        }

        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public Color StatusColor
        {
            get { return statusColor; }
            set { SetProperty(ref statusColor, value); }
        }

        //public bool IsContinue
        //{

        //}
        //public ushort TestRegister
        //{
        //    get
        //    {
        //        return ModbusService.TestRegister;
        //    }
        //}

        //public CollectorScreen Collector1
        //{
        //    get
        //    {
        //        //UpdateJobComponents();
        //        return ModbusService.Collector1;
        //        //return WebSocketService.Collector;
        //    }
        //}


        //public CollectorScreen Collector2
        //{
        //    get
        //    {
        //        //UpdateJobComponents();
        //        return ModbusService.Collector2;
        //        //return WebSocketService.Collector;
        //    }
        //}

        //public SingleDosScreen SingleDos
        //{
        //    get
        //    {
        //        //UpdateJobComponents();
        //        return ModbusService.SingleDos;
        //        //return WebSocketService.SingleDos;
        //    }
        //}

        public ObservableCollection<CollectorLoop> CollectorsLoop
        {
            get { return collectorsLoop; }
            set { SetProperty(ref collectorsLoop, value); }
        }

        public CollectorScreen Collector1
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.Collectors[0];
            }
        }

        public CollectorScreen Collector2
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.Collectors[1];
            }
        }

        public CollectorScreen Collector3
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.Collectors[2];
            }
        }

        public CollectorScreen Collector4
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.Collectors[3];
            }
        }

        public VolumeDosScreen VolumeDos
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.VolumeDoses[0];
                //return WebSocketService.SingleDos;
            }
        }

        public PowderDosScreen PowderDos
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.PowderDoses[0];
            }
        }

        public CommonScreen Common
        {
            get 
            {
                //UpdateJobComponents();
                return ModbusService.Common;
                //return WebSocketService.Common; 
            }
        }

        public TimeSpan DosingTime
        {
            get { return dosingTime; }
            set { SetProperty(ref dosingTime, value); }
        }
        #endregion Properties

        #region Commands
        private void Back()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        //private void Back2Pages()
        //{
        //    Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
        //    Application.Current.MainPage.Navigation.PopAsync();
        //}

        private void Back3Pages()
        {
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 2]);
            Application.Current.MainPage.Navigation.PopAsync();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void StartJob()
        {
            //WebSocketService.CommonLoopMessage(new CommonLoop { CommandStart = true });
            ModbusSendRequirements();
            Thread.Sleep(1000);
            ModbusService.WriteSingleRegister(CommonModbus.LoopStart());
            JobScreen.StartDateTime = DateTime.Now;
        }

        private async void StopJob()
        {
            if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Если идет дозация, то она будет завершена. Выйти?", "Да", "Нет"))
            {
                ModbusService.WriteSingleRegister(CommonModbus.LoopStop());
                if (!isLoopReported)
                {
                    SaveReport();
                }
                ModbusService.MasterDispose();
                IsExitJob = true;
                Back3Pages();
            }
        }

        private void PauseJob()
        {
            //WebSocketService.CommonLoopMessage(new CommonLoop { CommandPause = true });
            ModbusService.WriteSingleRegister(CommonModbus.LoopPause());
        }

        private void ContJob()
        {
            //WebSocketService.CommonLoopMessage(new CommonLoop { CommandPause = true });
            ModbusService.WriteSingleRegister(CommonModbus.LoopContinue());
        }
        #endregion Commands

        #region Methods
        public void MakeRequirements(List<JobComponent> jobComponents)
        {
            //var valveNums = new List<int>();
            //var requiredVolumes = new List<double>();
            
            double carrierRequiredVolume = 0;
            //double singleRequiredVolume = 0;
            double volumeDosRequiredVolume = 0;
            double powderDosRequiredVolume = 0;

            foreach (var jobComponent in jobComponents)
            {
                if (jobComponent.Dispenser == DispenserSuffix.Carrier)
                {
                    carrierRequiredVolume = (double)jobComponent.Volume;
                    continue;
                }

                if (jobComponent.Dispenser == DispenserSuffix.Dry)
                {
                    continue;
                }

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Collector) >= 0)
                {
                    //valveNums.Add(jobComponent.GetDispenserNumber());
                    //requiredVolumes.Add((double)jobComponent.Volume);

                    int collectorIndex = (int)Char.GetNumericValue(jobComponent.Dispenser[0]) - 1;
                    CollectorsLoop[collectorIndex].ValveNums.Add(DispenserNumber.Offset(jobComponent.Dispenser));
                    CollectorsLoop[collectorIndex].RequiredVolumes.Add((double)jobComponent.Volume);
                    
                    continue;
                }


                //if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Single) >= 0)
                //{
                //    singleRequiredVolume = (double)jobComponent.Volume;
                //    continue;
                //}

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Volume) >= 0)
                {
                    volumeDosRequiredVolume = (double)jobComponent.Volume;
                    continue;
                }

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Powder) >= 0)
                {
                    powderDosRequiredVolume = (double)jobComponent.Volume;
                    continue;
                }
            }

            //collectorLoop = new CollectorLoop
            //{
            //    ValveNums = valveNums,
            //    RequiredVolumes = requiredVolumes
            //};

            commonLoop = new CommonLoop
            {
                CarrierRequiredVolume = carrierRequiredVolume,
                CarrierReserve = (double)Job.Recipe.CarrierReserve
            };

            //singleDosLoop = new SingleDosLoop
            //{
            //    RequiredVolume = singleRequiredVolume
            //};

            volumeDosLoop = new VolumeDosLoop
            {
                RequiredVolume = volumeDosRequiredVolume
            };

            powderDosLoop = new PowderDosLoop
            {
                RequiredVolume = powderDosRequiredVolume
            };
        }

        public void WebSocketSendRequirements()
        {            
            //WebSocketService.CollectorLoopMessage(1, collectorLoop);
            //WebSocketService.SingleLoopMessage(1, singleDosLoop);
            //WebSocketService.CommonLoopMessage(commonLoop);


            //WebSocketService.AllLoopMessage(commonLoop, collectorLoop, singleDosLoop);
        }

        public void ModbusSendRequirements()
        {
            for (int i = 0; i < nCollectors; i++)
            {
                bool[] usedValves = new bool[nDoseValves];
                ushort collectorNumber = (ushort)(i + 1);
                for (int j = 0; j < CollectorsLoop[i].ValveNums.Count; j++)
                {
                    ushort order = (ushort)(j + 1);
                    ushort valveNum = (ushort)CollectorsLoop[i].ValveNums[j];
                    ModbusService.WriteSingleRegister(CollectorModbus.ValveOrder(collectorNumber, valveNum, order));
                    ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRequired(collectorNumber, valveNum, (float)CollectorsLoop[i].RequiredVolumes[j]));
                    usedValves[valveNum - 1] = true;
                }

                for (int j = 0; j < nDoseValves; j++)
                {
                    if (!usedValves[j])
                    {
                        ModbusService.WriteSingleRegister(CollectorModbus.ValveOrder(collectorNumber, (ushort)(j + 1), 0));
                        ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRequired(collectorNumber, (ushort)(j + 1), 0.0f));
                    }
                }
            }

            ModbusService.WriteSingleRegister32(VolumeDosModbus.VolumeRequired(1, (float)volumeDosLoop.RequiredVolume));
            ModbusService.WriteSingleRegister32(PowderDosModbus.VolumeRequired(1, (float)powderDosLoop.RequiredVolume));

            ModbusService.WriteSingleRegister32(CommonModbus.VolumeRequired((float)commonLoop.CarrierRequiredVolume));
            ModbusService.WriteSingleRegister32(CommonModbus.Reserve((float)commonLoop.CarrierReserve));

            if (Job.Recipe.IsMotherLiquor)
            {
                ModbusService.WriteSingleRegister(CommonModbus.MotherLiquorEnable());
            }
            else
            {
                ModbusService.WriteSingleRegister(CommonModbus.MotherLiquorDisable());
            }

            double fillMotherLiquorVolume = ((float)Job.Recipe.FillMotherLiquorVolume) * ((float)commonLoop.CarrierRequiredVolume) / 100.0;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFillMotherLiquorVol((float)fillMotherLiquorVolume));
        }

        public void UpdateJobComponents()
        {
            ModbusService.MasterMessages();

            JobScreen.Update(Common, Collector1, Collector2, Collector3, Collector4, VolumeDos, PowderDos);

            IsLoopNotPause = !Common.IsLoopPause && Common.IsLoopActive;
            IsLoopCont = Common.IsLoopPause && Common.IsLoopActive;
            IsLoopStart = !Common.IsLoopActive && !isLoopWasActive;

            IsLoopDone = Common.IsLoopDone && isLoopWasActive;

            OnPropertyChanged(nameof(IsLoopNotPause));
            OnPropertyChanged(nameof(IsLoopCont));
            OnPropertyChanged(nameof(IsLoopStart));
            OnPropertyChanged(nameof(IsLoopDone));

            if (Common.IsLoopActive)
            {
                isLoopWasActive = true;
            }

            if (IsLoopDone && !isLoopReported)
            {
                SaveReport();
            }

            if (IsLoopStart) { StatusText = "Ожидание запуска дозации"; StatusColor = Color.Gray; }
            else if (IsLoopDone) { StatusText = "Дозация завершена"; StatusColor = Color.FromHex("#00C000"); }
            else if (Common.IsLoopPause) { StatusText = "Дозация приостановлена"; StatusColor = StatusColor = Color.Gray; }
            else { StatusText = "Идёт дозация"; StatusColor = Color.Orange; }

            DateTime now = DateTime.Now;
            if (JobScreen.StartDateTime == DateTime.MinValue)
                DosingTime = TimeSpan.Zero;
            else
                DosingTime = now.Subtract(JobScreen.StartDateTime);
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


        public void ExitJob()
        {
            Application.Current.MainPage.DisplayAlert("Предупреждение", "Отсутствует связь с ПЛК, вы будете перенаправлены на главную страницу", "Ok");
            {
                ModbusService.WriteSingleRegister(CommonModbus.LoopStop());
                if (!isLoopReported)
                {
                    SaveReport();
                }
                ModbusService.MasterDispose();
                IsExitJob = true;
                Back3Pages();
            }
        }

        public void SaveReport()
        {
            DateTime now = DateTime.Now;
            if (JobScreen.StartDateTime == DateTime.MinValue)
                JobScreen.StartDateTime = now;

            Report report = new Report {
                ReportDateTime = now,
                IsCompleted = IsLoopDone,
                AssignmentName = Job.Name,
                AssignmentPlace = Job.Assignment.Place,
                AssignmentNote = Job.Assignment.Note,
                RecipeName = Job.Recipe.Name,
                OperatorName = App.ActiveUser.DisplayName,
                DosingTime = now.Subtract(JobScreen.StartDateTime),
                VolumeRate = Job.VolumeRate,
                AirTemperature = Common.AirTemperature
            };

            List<ReportComponent> reportComponents = new List<ReportComponent>();
            foreach (var jobComponentScreen in JobScreen.JobComponentScreens)
            {
                reportComponents.Add(new ReportComponent {
                    Report = report,
                    Name = jobComponentScreen.Name,
                    RequiredVolume = jobComponentScreen.Volume,
                    DosedVolume = jobComponentScreen.DosedVolume,
                    Dispenser = jobComponentScreen.Dispenser
                });
            }

            using (AppDbContext db = App.GetContext())
            {
                db.Reports.Add(report);
                db.ReportComponents.AddRange(reportComponents);
                db.SaveChanges();
            }

            isLoopReported = true;
        }
        #endregion Methods
    }
}
