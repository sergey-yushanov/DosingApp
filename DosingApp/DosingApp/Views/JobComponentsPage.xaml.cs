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

        protected override void OnDisappearing()
        {
            var jobComponentsViewModel = (JobComponentsViewModel)BindingContext;
            //jobComponentsViewModel.WebSocketService.WebsocketClientExit();
            jobComponentsViewModel.ModbusService.MasterDispose();
            TimerStop();
            base.OnDisappearing();
        }

        public void TimerStart()
        {
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(TimeSpan.FromSeconds(1), () => 
            {
                if (cts.IsCancellationRequested) return false;
                Device.BeginInvokeOnMainThread(() => ViewModel.UpdateJobComponents());
                return true;
            });
        }

        public void TimerStop()
        {
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}