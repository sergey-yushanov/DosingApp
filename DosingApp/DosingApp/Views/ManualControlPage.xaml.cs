﻿using DosingApp.DataContext;
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
    public partial class ManualControlPage : ContentPage
    {
        private CancellationTokenSource cancellation;

        public ManualControlPage()
        {
            InitializeComponent();
            BindingContext = new ManualControlViewModel();

            this.cancellation = new CancellationTokenSource();
            TimerStart((ManualControlViewModel)BindingContext);
        }

        protected override void OnDisappearing()
        {
            var manualControlViewModel = (ManualControlViewModel)BindingContext;
            manualControlViewModel.ModbusService.MasterDispose();
            TimerStop();
            base.OnDisappearing();
        }

        public void TimerStart(ManualControlViewModel viewModel)
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