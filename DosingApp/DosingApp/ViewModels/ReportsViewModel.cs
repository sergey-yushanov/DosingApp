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
        private DateTime? fromDate;
        private DateTime? toDate;

        public ICommand CreateReportCommand { get; protected set; }

        public ExcelService ExcelService { get; protected set; }
        #endregion Attributes

        #region Constructor
        public ReportsViewModel()
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
            CreateReportCommand = new Command(CreateReport);

            ExcelService = new ExcelService();
        }
        #endregion Constructor

        #region Properties
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { SetProperty(ref fromDate, value); }
        }

        public DateTime? ToDate
        {
            get { return toDate; }
            set { SetProperty(ref toDate, value); }
        }
        #endregion Properties

        #region Commands
        private void CreateReport()
        {
            if (FromDate.HasValue && ToDate.HasValue)
            {
                string datesString = ((DateTime)FromDate).ToString("dd.MM.yyyy HH:mm:ss") + " - " + ((DateTime)ToDate).AddDays(1).AddSeconds(-1).ToString("dd.MM.yyyy HH:mm:ss");
                string reportPath = Path.Combine(App.ReportsFolderPath, "Отчет за период " + datesString + ".xlsx");
                ExcelService.GenerateExcel(reportPath, "Отчет");

                foreach (Report report in GetReports())
                {
                    LoadReportComponents(report);
                }

                //string pdfFilePath = ExcelService.ReportPrepareToPrint(report, LoadReportComponents(report));
                //string title = "Требование-накладная № " + report.ReportId.ToString() + "\n" + report.ReportDateTime.ToString("dd.MM.yyyy HH:mm");
                //await Application.Current.MainPage.Navigation.PushAsync(new PdfDocumentView(title, pdfFilePath));
            }
        }
        #endregion Commands

        #region Methods
        public List<Report> GetReports()
        {
            List<Report> reportsDb = new List<Report>();

            using (AppDbContext db = App.GetContext())
            {
                if (FromDate.HasValue && ToDate.HasValue)
                {
                    reportsDb = db.Reports.Where(r => r.ReportDateTime.Date >= FromDate.Value && r.ReportDateTime.Date < ToDate.Value.AddDays(1)).ToList();
                }
            }
            return reportsDb.OrderByDescending(r => r.ReportDateTime).ToList();
        }

        public List<ReportComponent> LoadReportComponents(Report report)
        {
            using (AppDbContext db = App.GetContext())
            {
                return db.ReportComponents.Where(r => r.ReportId == report.ReportId).ToList();
            }
        }
        #endregion Methods

    }
}
