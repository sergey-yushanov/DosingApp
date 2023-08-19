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
        #endregion Attributes

        #region Constructor
        public ReportsViewModel()
        {
            SelectedDate = DateTime.Now.Date;
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
        #endregion Methods

    }
}
