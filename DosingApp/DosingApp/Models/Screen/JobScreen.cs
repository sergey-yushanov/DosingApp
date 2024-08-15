using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DosingApp.Models.Screen
{
    public class JobScreen : BaseViewModel
    {
        public ObservableCollection<JobComponentScreen> JobComponentScreens { get; set; }
        public DateTime StartDateTime;
        private double? progressBarValue;
        private double? requiredVolume;
        private double? dosedVolume;

        public double? ProgressBarValue
        {
            get { return progressBarValue; }
            set { SetProperty(ref progressBarValue, value); }
        }

        public double? RequiredVolume
        {
            get { return requiredVolume; }
            set { SetProperty(ref requiredVolume, value); }
        }

        public double? DosedVolume
        {
            get { return dosedVolume; }
            set { SetProperty(ref dosedVolume, value); }
        }

        public JobScreen(List<JobComponent> jobComponents)
        {
            RequiredVolume = 0;

            JobComponentScreens = new ObservableCollection<JobComponentScreen>();
            for (int i = 0; i < jobComponents.Count; i++)
            {
                JobComponentScreens.Add(new JobComponentScreen(jobComponents[i]));
                if (jobComponents[i].Dispenser != DispenserSuffix.Dry)
                {
                    RequiredVolume += (double)jobComponents[i].Volume;
                }
            }
        }

        public void Update(CommonScreen commonScreen, CollectorScreen collectorScreen, SingleDosScreen singleDosScreen)
        {
            DosedVolume = 0;

            for (int i = 0; i < JobComponentScreens.Count; i++)
            {
                JobComponentScreens[i].Update(commonScreen, collectorScreen, singleDosScreen);
                DosedVolume += (double?)JobComponentScreens[i].DosedVolume;

                //Console.Write(JobComponentScreens[i].Dispenser + ": ");
                //Console.WriteLine((double?)JobComponentScreens[i].DosedVolume);
            }

            ProgressBarValue = DosedVolume / RequiredVolume;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    Console.WriteLine(JobComponentScreens[i].DosedVolume);
            //}
            //Console.WriteLine("=========");

            //OnPropertyChanged(nameof(JobComponentScreens));
        }

        public void Update(
            CommonScreen commonScreen,
            CollectorScreen collectorScreen1,
            CollectorScreen collectorScreen2,
            CollectorScreen collectorScreen3,
            CollectorScreen collectorScreen4,
            VolumeDosScreen volumeDosScreen,
            PowderDosScreen powderDosScreen )
        {
            DosedVolume = 0;

            List<CollectorScreen> collectors = new List<CollectorScreen>();
            collectors.Add(collectorScreen1);
            collectors.Add(collectorScreen2);
            collectors.Add(collectorScreen3);
            collectors.Add(collectorScreen4);

            for (int i = 0; i < JobComponentScreens.Count; i++)
            {
                JobComponentScreens[i].Update(commonScreen, collectors, volumeDosScreen, powderDosScreen);
                DosedVolume += (double?)JobComponentScreens[i].DosedVolume;

                //Console.Write(JobComponentScreens[i].Dispenser + ": ");
                //Console.WriteLine((double?)JobComponentScreens[i].DosedVolume);
            }

            ProgressBarValue = DosedVolume / RequiredVolume;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    Console.WriteLine(JobComponentScreens[i].DosedVolume);
            //}
            //Console.WriteLine("=========");

            //OnPropertyChanged(nameof(JobComponentScreens));
        }

    }
}
