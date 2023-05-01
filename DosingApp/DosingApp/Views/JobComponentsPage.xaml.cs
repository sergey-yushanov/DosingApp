using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class JobComponentsPage : ContentPage
    {
        private CancellationTokenSource cancellation;

        public JobComponentsViewModel ViewModel { get; private set; }
        public JobComponentsPage(JobComponentsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;

            this.cancellation = new CancellationTokenSource();
            TimerStart();
        }

        protected override bool OnBackButtonPressed()
        {
            //Task.Run(() => ViewModel.DeviceButtonStopJobAsync()).Wait();
            //if (ViewModel.IsExitJob)
            //{
            //    Console.WriteLine("Is Exit Job");
            //    TimerStop();
            //}
            return true;
        }


        //protected override void OnDisappearing()
        //{
        //    Console.WriteLine("OnDisappearing");
        //    var jobComponentsViewModel = (JobComponentsViewModel)BindingContext;
        //    //jobComponentsViewModel.WebSocketService.WebsocketClientExit();
        //    jobComponentsViewModel.ModbusService.MasterDispose();
        //    TimerStop();
        //    base.OnDisappearing();
        //}

        public void TimerStart()
        {
            Console.WriteLine("Timer Start");
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(TimeSpan.FromSeconds(1), () => 
            {
                if (cts.IsCancellationRequested) return false;
                Device.BeginInvokeOnMainThread(() => {
                    ViewModel.UpdateJobComponents();
                    //if (ViewModel.IsNotInitializedLoop) ViewModel.ExitJob();
                    if (ViewModel.IsExitJob) TimerStop();
                });
                return true;
            });
        }

        public void TimerStop()
        {
            Console.WriteLine("Timer Stop");
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}