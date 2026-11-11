using DosingApp.Models;
using DosingApp.Models.AgroAssist.Dto;
using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.AspNetCore.WebUtilities;
using DosingApp.DataContext;
using DosingApp.Models.AgroAssist;
using DosingApp.Services.Import;
using System.Threading;
using DosingApp.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using DosingApp.Services.Export;

namespace DosingApp.ViewModels
{
    public class RemoteViewModel : BaseViewModel
    {
        #region Attributes
        private static ApiService apiService { get; set; }
        private string baseAddress;
        private string email;
        private string password;

        public ICommand DownloadCommand { get; protected set; }
        public ICommand UploadCommand { get; protected set; }

        private readonly SyncService syncService;
        private bool isBusy;
        private string statusText = string.Empty;
        private int current;
        private int total;
        #endregion Attributes

        #region Constructor
        public RemoteViewModel()
        {
            BaseAddress = "https://gateway-frontend.agroassist.ru";
            Email = "sergey.yushanov@gmail.com";
            Password = "AgroAssist231!";
            apiService = new ApiService(BaseAddress) {Email = Email, Password = Password };
            syncService = new SyncService();

            DownloadCommand = new Command(async () => await DownloadAsync(), () => !IsBusy);
            UploadCommand = new Command(async() => await UploadAsync(), () => !IsBusy);
        }
        #endregion Constructor

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public bool IsNotBusy => !IsBusy;

        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public int Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        }

        public int Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        public string BaseAddress
        {
            get { return baseAddress; }
            set { SetProperty(ref baseAddress, value); }
        }

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        #endregion Properties

        #region Commands
        private async Task DownloadAsync()
        {
            if (!await apiService.IsServerReachableAsync().ConfigureAwait(false))
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Нет связи", "Сервер недоступен. Проверьте интернет.", "OK"));
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            var cts = new CancellationTokenSource();
            var popupVm = new SyncProgressViewModel(cts);
            var popup = new SyncProgressPage(popupVm);

            // Открыть всплывающее окно
            await PopupNavigation.Instance.PushAsync(popup);

            try
            {
                var progress = new Progress<SyncProgress>(p => popupVm.Apply(p));
                var result = await Task.Run(() => syncService.SyncAllAsync(apiService, progress, cts.Token)).ConfigureAwait(false);
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowSuccess($"Загрузка завершена"));
            }
            catch (OperationCanceledException)
            {
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowCancelled("Загрузка отменена"));
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowError($"Ошибка: {ex.Message}"));
            }
            finally
            {
                IsBusy = false;
                cts.Dispose();
            }
        }

        private async Task UploadAsync()
        {
            if (!await apiService.IsServerReachableAsync().ConfigureAwait(false))
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Нет связи", "Сервер недоступен. Проверьте интернет.", "OK"));
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            var cts = new CancellationTokenSource();
            var popupVm = new SyncProgressViewModel(cts);
            var popup = new SyncProgressPage(popupVm);

            // Открыть всплывающее окно
            await PopupNavigation.Instance.PushAsync(popup);

            try
            {
                var progress = new Progress<SyncProgress>(p => popupVm.Apply(p));

                using (var db = App.GetContext())
                {
                    var builder = new WithdrawalPayloadBuilder(db);
                    var sync = new WithdrawalSyncService(apiService);
                    var orchestrator = new WithdrawalSyncOrchestrator(db, builder, sync);

                    var summary = await orchestrator.PushAsync(progress, cts.Token);
                    await Device.InvokeOnMainThreadAsync(() => popupVm.ShowSuccess(
                        $"Отправка завершена\n" +
                        $"Создано: {summary.Created}  |  Обновлено: {summary.Updated}  |  Удалено: {summary.Deleted}  |  Ошибок: {summary.Failed}"
                    ));
                }
            }
            catch (OperationCanceledException)
            {
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowCancelled("Загрузка отменена"));
            }
            catch (Exception ex)
            {
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowError($"Ошибка: {ex.Message}"));
            }
            finally
            {
                IsBusy = false;
                cts.Dispose();
            }
        }
        #endregion Commands

        #region Methods

        #endregion Methods
    }
}
