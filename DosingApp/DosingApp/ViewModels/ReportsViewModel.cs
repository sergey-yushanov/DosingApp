using CsvHelper;
using CsvHelper.Configuration;
using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.Files;
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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DosingApp.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Report> reports;
        private Report selectedReport;
        private DateTime? selectedDate;

        public ICommand PrintReportCommand { get; protected set; }

        public ExcelService ExcelService { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ReportsViewModel()
        {
            SelectedDate = DateTime.Now.Date;
            PrintReportCommand = new Command(PrintReport);

            ExcelService = new ExcelService();
        }
        #endregion Constructor

        #region Properties
        public ObservableCollection<Report> Reports
        {
            get { return reports; }
            set { SetProperty(ref reports, value); }
        }

        public Report SelectedReport
        {
            get { return selectedReport; }
            set { SetProperty(ref selectedReport, value); }
        }

        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                SetProperty(ref selectedDate, value);
                LoadReports();
            }
        }
        #endregion Properties

        #region Commands
        private async void PrintReport(object reportInstance)
        {
            Report report = reportInstance as Report;
            //if (await Application.Current.MainPage.DisplayAlert("Предупреждение", "Выбранный отчет от " + report.ReportDateTime.ToString("f") + " будет отправлен на печать. Распечатать?", "Да", "Нет"))
            //{
                // заполняем файл Excel данными
                ExportToExcel(report);

                // преобразуем в pdf
                string filePath = App.GetReportFilePath(false);
                string printFilePath = Path.Combine(App.FolderPath, App.PDFREPORTFILENAME);
                ExcelService.ConvertExcelToPdf(filePath, printFilePath);

                //await Application.Current.MainPage.Navigation.PushAsync(new GoogleDriveViewerPage(printFilePath));
                //await Application.Current.MainPage.Navigation.PushAsync(new PdfJsPage(printFilePath));

                var pdfDocEntity = new PdfDocEntity
                {
                    FileName = "Требование-накладная М-11.pdf",
                    Url = printFilePath
                };
                await Application.Current.MainPage.Navigation.PushAsync(new PdfDocumentView(pdfDocEntity));
            //}
        }
        #endregion Commands

        #region Methods
        public void LoadReports()
        {
            using (AppDbContext db = App.GetContext())
            {
                if (SelectedDate.HasValue)
                {
                    var reportsDB = db.Reports.Where(r => r.ReportDateTime.Date == SelectedDate.Value).ToList();
                    Reports = new ObservableCollection<Report>(reportsDB.OrderByDescending(r => r.ReportDateTime));
                }
            }
        }

        public List<ReportComponent> LoadReportComponents(int reportId)
        {
            using (AppDbContext db = App.GetContext())
            {
                return db.ReportComponents.Where(r => r.ReportId == reportId).ToList();
            }
        }

        public void ExportToExcel(Report report)
        {
            // загружаем данные
            List<ExcelCell> excelCells = new List<ExcelCell>();

            ExcelCell excelCellNumber = new ExcelCell { ColumnName = "G", RowIndex = 5, Text = report.ReportId.ToString() };
            ExcelCell excelCellDate = new ExcelCell { ColumnName = "A", RowIndex = 11, Text = report.ReportDateTime.Date.ToString("dd.MM.yyyy") };

            uint rowIndexOffset = 19;
            uint i = 0;
            foreach (ReportComponent reportComponent in LoadReportComponents(report.ReportId))
            {
                excelCells.Add(new ExcelCell { ColumnName = "C", RowIndex = rowIndexOffset + i, Text = reportComponent.Name });
                excelCells.Add(new ExcelCell { ColumnName = "F", RowIndex = rowIndexOffset + i, Text = "л" });
                excelCells.Add(new ExcelCell { ColumnName = "G", RowIndex = rowIndexOffset + i, Text = ((double)reportComponent.DosedVolume).ToString("N2") });
                excelCells.Add(new ExcelCell { ColumnName = "H", RowIndex = rowIndexOffset + i, Text = ((double)reportComponent.DosedVolume).ToString("N2") });

                i++;
            }

            excelCells.Add(excelCellNumber);
            excelCells.Add(excelCellDate);

            string filePath = App.GetReportFilePath(true);
            ExcelService.InsertDataIntoCells(filePath, "Лист1", excelCells);
        }
        #endregion Methods

    }
}
