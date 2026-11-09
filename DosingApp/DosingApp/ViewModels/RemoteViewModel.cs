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
        //private int savedCount;
        //private double progress;
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
            UploadCommand = new Command(Upload);
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

        //public int SavedCount
        //{
        //    get { return savedCount; }
        //    set { SetProperty(ref savedCount, value); }
        //}

        //public double Progress
        //{
        //    get { return progress; }
        //    set { SetProperty(ref progress, value); }
        //}

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

                //await Device.InvokeOnMainThreadAsync(async () =>
                //{
                //    await PopupNavigation.Instance.PopAsync();
                //    //await Application.Current.MainPage.DisplayAlert("Готово", "Загрузка завершена", "OK");
                //});

                // Успех — переключаем popup в режим «готово»
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowSuccess($"Загрузка завершена"));
            }
            catch (OperationCanceledException)
            {
                //await Device.InvokeOnMainThreadAsync(async () =>
                //{
                //    await PopupNavigation.Instance.PopAsync();
                //    await Application.Current.MainPage.DisplayAlert("Отменено", "Загрузка прервана", "OK");
                //});

                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowCancelled("Загрузка отменена"));
            }
            catch (Exception ex)
            {
                //await Device.InvokeOnMainThreadAsync(async () =>
                //{
                    //await PopupNavigation.Instance.PopAsync();
                    //await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
                //});
                await Device.InvokeOnMainThreadAsync(() => popupVm.ShowError($"Ошибка: {ex.Message}"));
            }
            finally
            {
                IsBusy = false;
                cts.Dispose();
            }




            //TechOperationsListResponseDto techOperations = await GetTechOperationsAsync();

            //await LoadAndSaveUnitsAsync();
            //await LoadAndSavePesticidesAsync();
            //PlanWorksExportResponseDto planWorks = await GetPlanWorksAsync();
            //List<Guid> techoperationGuids = ListTechoperationGuids(planWorks);

        }

        private void Upload()
        {
            ;
        }
        #endregion Commands

        #region Methods
        //private async Task<PlanWorksExportResponseDto> GetPlanWorksAsync()
        //{
        //    PlanWorksExportResponseDto planWorks = new PlanWorksExportResponseDto();

        //    try
        //    {
        //        var request = new PlanWorksExportRequestDto
        //        {
        //            SeasonId = 35,
        //            StructureGuid = new Guid("c6cdff0f-b1a5-4297-ab7c-d2cc615f385b"),
        //            StartDatetime = DateTime.ParseExact("2026-11-01T00:00:00.000", "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture),
        //            EndDatetime = DateTime.ParseExact("2026-11-03T23:59:59.999", "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture),
        //            TechoperationStatusIds = new List<int>() { 1, 2 }
        //        };

        //        planWorks = await ApiService.PostAsync<PlanWorksExportRequestDto, PlanWorksExportResponseDto>("techoperations/plan_works/export", request);
        //        //await Application.Current.MainPage.DisplayAlert("Успех", $"План на день загружен", "ОК");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить план на день: {ex.Message}", "ОК");
        //    }
        //    return planWorks;
        //}

        //private List<Guid> ListTechoperationGuids(PlanWorksExportResponseDto planWorks)
        //{
        //    // "a7030eda-e022-4209-bef6-560c0064a9c0" - опрыскивание
        //    var targetType = Guid.Parse("a7030eda-e022-4209-bef6-560c0064a9c0");

        //    List<Guid> techoperationGuids = planWorks
        //        .Where(x => x.TechoperationTypeGuid == targetType && x.TechoperationGuid.HasValue)
        //        .Select(x => x.TechoperationGuid.Value)
        //        .ToList();

        //    return techoperationGuids;
        //}

        private async Task<TechOperationsListResponseDto> GetTechOperationsAsync()
        {
            TechOperationsListResponseDto techOperations = new TechOperationsListResponseDto();

            DateTime now = DateTime.Now;

            //Guid structureGuid = new Guid("c6cdff0f-b1a5-4297-ab7c-d2cc615f385b");
            DateTime startPlanDatetime = now.Date.AddDays(-1);
            DateTime endPlanDatetime = now.Date.AddDays(2).AddMilliseconds(-1);
            List<int> statusIds = new List<int> { 1, 2 };

            string url = "techoperations/list";
            //url = QueryHelpers.AddQueryString(url, "structure_guid", structureGuid.ToString());
            url = QueryHelpers.AddQueryString(url, "start_plan_datetime", startPlanDatetime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            url = QueryHelpers.AddQueryString(url, "end_plan_datetime", endPlanDatetime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            url = QueryHelpers.AddQueryString(url, "dates_filter_mode", "any");
            url = QueryHelpers.AddQueryString(url, "status_ids", "1");
            url = QueryHelpers.AddQueryString(url, "status_ids", "2");

            try
            {
                techOperations = await apiService.GetAsync<TechOperationsListResponseDto>(url);

                await Application.Current.MainPage.DisplayAlert("Успех", $"Задания загружены", "ОК");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить задания на день: {ex.Message}", "ОК");
            }
            return techOperations;
        }

        //public async Task LoadAndSaveUnitsAsync()
        //{
        //    if (IsBusy) return;
        //    IsBusy = true;

        //    Progress = -1;
        //    SavedCount = 0;
        //    StatusText = "Загрузка данных...";

        //    try
        //    {
        //        var progress = new Progress<int>(n => SavedCount = n);
        //        var total = 0;

        //        using (AppDbContext db = App.GetContext())
        //        {
        //            UnitImportService importService = new UnitImportService(ApiService, db);
        //            total = await Task.Run(() => importService.LoadAllAsync(progress)).ConfigureAwait(false);
        //        }
        //        await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Готово.", $"Сохранено {total} единиц измерения", "OK"));
        //    }
        //    catch (Exception ex)
        //    {
        //        await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Ошибка загрузки единиц измерения", ex.Message, "OK"));
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //public async Task LoadAndSavePesticidesAsync()
        //{
        //    if (IsBusy) return;
        //    IsBusy = true;
            
        //    try
        //    {
        //        var progress = new Progress<int>(n => SavedCount = n);
        //        var total = 0;

        //        using (AppDbContext db = App.GetContext())
        //        {
        //            PesticideImportService importService = new PesticideImportService(apiService, db);
        //            total = await Task.Run(() => importService.LoadAllAsync(progress)).ConfigureAwait(false);
        //        }
        //        await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Готово.", $"Сохранено {total} СЗР", "OK"));
        //    }
        //    catch (Exception ex)
        //    {
        //        await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("Ошибка загрузки СЗР", ex.Message, "OK"));
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
        #endregion Methods
    }
}
