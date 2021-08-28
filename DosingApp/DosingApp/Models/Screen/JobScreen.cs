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
        public float ProgressBarValue { get; set; }
        public float RequiredVolume { get; set; }
        public float DosedVolume { get; set; }

        public JobScreen(List<JobComponent> jobComponents)
        {
            RequiredVolume = 0;

            JobComponentScreens = new ObservableCollection<JobComponentScreen>();
            for (int i = 0; i < jobComponents.Count; i++)
            {
                JobComponentScreens.Add(new JobComponentScreen(jobComponents[i]));
                RequiredVolume += (float)jobComponents[i].Volume;
            }
        }

        public void Update(CommonScreen commonScreen, CollectorScreen collectorScreen)
        {
            DosedVolume = 0;

            for (int i = 0; i < JobComponentScreens.Count; i++)
            {
                JobComponentScreens[i].Update(commonScreen, collectorScreen);
                DosedVolume += (float)JobComponentScreens[i].DosedVolume;
            }

            ProgressBarValue = DosedVolume / RequiredVolume;

            //for (int i = 0; i < JobComponentScreens.Count; i++)
            //{
            //    Console.WriteLine(JobComponentScreens[i].DosedVolume);
            //}
            //Console.WriteLine("=========");

            OnPropertyChanged(nameof(JobComponentScreens));
        }

    }
}
