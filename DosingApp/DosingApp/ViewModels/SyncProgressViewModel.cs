using DosingApp.Services.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public enum SyncState
    {
        Loading,
        Success,
        Error,
        Cancelled
    }

    public class SyncProgressViewModel : INotifyPropertyChanged
    {
        private readonly CancellationTokenSource _cts;

        private SyncState _state = SyncState.Loading;
        private string _statusText = "Подготовка...";

        private int _current;

        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }

        public SyncProgressViewModel(CancellationTokenSource cts)
        {
            _cts = cts;
            CancelCommand = new Command(() =>
            {
                if (_state != SyncState.Loading) return;
                StatusText = "Отмена...";
                _cts.Cancel();
            });

            CloseCommand = new Command(async () => await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync());
        }

        public SyncState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsFinished));
                OnPropertyChanged(nameof(IsSuccess));
                OnPropertyChanged(nameof(IsError));
                OnPropertyChanged(nameof(IsCancelled));
            }
        }

        public bool IsLoading => State == SyncState.Loading;
        public bool IsFinished => State != SyncState.Loading;
        public bool IsSuccess => State == SyncState.Success;
        public bool IsError => State == SyncState.Error;
        public bool IsCancelled => State == SyncState.Cancelled;

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        public int Current
        {
            get => _current;
            set { _current = value; OnPropertyChanged(); }
        }

        //public string CurrentText => $"Обработано: {Current}";
        
        // ── переходы состояний ──
        public void ShowSuccess(string message)
        {
            State = SyncState.Success;
            StatusText = message;
        }

        public void ShowError(string message)
        {
            State = SyncState.Error;
            StatusText = message;
        }

        public void ShowCancelled(string message)
        {
            State = SyncState.Cancelled;
            StatusText = message;
        }

        public void Apply(SyncProgress p)
        {
            if (State != SyncState.Loading) return;
            StatusText = p.Stage;
            Current = p.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
