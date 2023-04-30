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
    public partial class MixerControlPage : ContentPage
    {
        private CancellationTokenSource cancellation;

        public MixerControlPage()
        {
            InitializeComponent();
            BindingContext = new MixerControlViewModel();

            this.cancellation = new CancellationTokenSource();
            TimerStart((MixerControlViewModel)BindingContext);
        }

        protected override bool OnBackButtonPressed()
        {
            var mixerControlViewModel = (MixerControlViewModel)BindingContext;
            mixerControlViewModel.ModbusService.MasterDispose();
            TimerStop();
            return base.OnBackButtonPressed();
        }

        //protected override void OnDisappearing()
        //{
        //    var mixerControlViewModel = (MixerControlViewModel)BindingContext;
        //    mixerControlViewModel.ModbusService.MasterDispose();
        //    TimerStop();
        //    base.OnDisappearing();
        //}

        public void TimerStart(MixerControlViewModel viewModel)
        {
            CancellationTokenSource cts = this.cancellation; // safe copy
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (cts.IsCancellationRequested) return false;
                Device.BeginInvokeOnMainThread(() => viewModel.Update());
                return true;
            });
        }

        public void TimerStop()
        {
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}