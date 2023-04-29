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
        private static int nCollectors = 2;
        private static int nDoseValves = 3;
        private ObservableCollection<CollectorLoop> collectorsLoop;

        //private SingleDosLoop singleDosLoop;
        private VolumeDosLoop volumeDosLoop;
        //private double progressBarValue;
        //private double requiredVolume;
        //private double dosedVolume;
        private bool isPause;

        //
        //private ushort testRegister;

        public ICommand StartJobCommand { get; protected set; }
        public ICommand StopJobCommand { get; protected set; }
        public ICommand PauseJobCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        //public WebSocketService WebSocketService { get; protected set; }
        public ModbusService ModbusService { get; protected set; }
        #endregion Attributes

        #region Constructor
        public JobComponentsViewModel(Job job, List<JobComponent> jobComponents)
        {
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
            BackCommand = new Command(Back);

            //WebSocketService = new WebSocketService();
            //if (WebSocketService.Mixer != null)
            //{
            //MakeRequirements(jobComponents);
            //WebSocketSendRequirements();
            //ModbusSendRequirements();
            //}

            ModbusService = new ModbusService();
            if (ModbusService.Mixer != null)
            {
                ModbusService.WriteSingleRegister(CommonModbus.LoopClear());

                MakeRequirements(jobComponents);
                ModbusSendRequirements();
            }
            //ModbusService = new ModbusService();
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
                return ModbusService.Collector1;
            }
        }

        public CollectorScreen Collector2
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.Collector2;
            }
        }

        public VolumeDosScreen VolumeDos
        {
            get
            {
                //UpdateJobComponents();
                return ModbusService.VolumeDos;
                //return WebSocketService.SingleDos;
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
            //WebSocketService.CommonLoopMessage(new CommonLoop { CommandStart = true });
            ModbusService.WriteSingleRegister(CommonModbus.LoopStart());
        }

        private async void StopJob()
        {
            if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Если идет дозация, то она будет завершена. Выйти?", "Да", "Нет"))
            {
                //WebSocketService.CommonLoopMessage(new CommonLoop { CommandStop = true });
                ModbusService.WriteSingleRegister(CommonModbus.LoopStop());
                Back3Pages();
            }
        }

        private void PauseJob()
        {
            //WebSocketService.CommonLoopMessage(new CommonLoop { CommandPause = true });
            ModbusService.WriteSingleRegister(CommonModbus.LoopPause());
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
                    CollectorsLoop[collectorIndex].ValveNums.Add(jobComponent.GetDispenserNumber());
                    CollectorsLoop[collectorIndex].RequiredVolumes.Add((double)jobComponent.Volume);
                    
                    continue;
                }


                //if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Single) >= 0)
                //{
                //    singleRequiredVolume = (double)jobComponent.Volume;
                //    continue;
                //}

                if (jobComponent.Dispenser.IndexOf(DispenserSuffix.Single) >= 0)
                {
                    volumeDosRequiredVolume = (double)jobComponent.Volume;
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

            ModbusService.WriteSingleRegister32(CommonModbus.VolumeRequired((float)commonLoop.CarrierRequiredVolume));
            ModbusService.WriteSingleRegister32(CommonModbus.Reserve((float)commonLoop.CarrierReserve));
        }

        public void UpdateJobComponents()
        {
            ModbusService.MasterMessages();

            //ushort[] registersCommon;
            //registersCommon = ModbusService.ReadRegisters(Registers.Common, CommonModbus.numberOfPoints);


            //CarrierDosedVolume
            // = (ushort)CommonModbus.Register32.CAR_DOSE_VOL;

            //var y = registersCommon[x];

            //OnPropertyChanged(nameof(TestRegister));

            //Console.WriteLine("UpdateJobComponents");
            JobScreen.Update(Common, Collector1, Collector2, VolumeDos);
            //OnPropertyChanged(nameof(JobScreen));

            //dosedVolume = 0;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    JobComponentScreens[i].Update(Common, Collector);
            //    dosedVolume += (double)JobComponentScreens[i].DosedVolume;
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
