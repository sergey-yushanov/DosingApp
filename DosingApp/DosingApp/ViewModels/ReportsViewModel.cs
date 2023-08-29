﻿using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
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
using static DosingApp.Services.ExcelService;

namespace DosingApp.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        #region Attributes
        private DateTime? fromDate;
        private DateTime? toDate;

        private bool isReportReady;
        private string reportReadyText;

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

            IsReportReady = false;
        }
        #endregion Constructor

        #region Properties
        public DateTime? FromDate
        {
            get { return fromDate; }
            set 
            {
                SetProperty(ref fromDate, value);
                IsReportReady = false;
            }
        }

        public DateTime? ToDate
        {
            get { return toDate; }
            set
            { 
                SetProperty(ref toDate, value);
                IsReportReady = false;
            }
        }

        public bool IsReportReady
        {
            get { return isReportReady; }
            set 
            {
                SetProperty(ref isReportReady, value);
                ReportReadyText = value ? "Отчёт готов" : "Сформировать";
            }
        }

        public string ReportReadyText
        {
            get { return reportReadyText; }
            set { SetProperty(ref reportReadyText, value); }
        }
        #endregion Properties

        #region Commands
        private void CreateReport()
        {
            if (FromDate.HasValue && ToDate.HasValue)
            {
                string datesString = ((DateTime)FromDate).ToString("ddMMyyyy") + "_" + ((DateTime)ToDate).ToString("ddMMyyyy");
                string reportPath = Path.Combine(App.ReportsFolderPath, "СЗР-Mix__" + datesString + ".xlsx");
                string sheetName = "Отчёт";
                ExcelService.GenerateExcelFromTemplate(reportPath);

                List<ExcelCell> excelCells = new List<ExcelCell>() {
                    new ExcelCell {
                        ColumnName = "E",
                        RowIndex = 4,
                        Text = ((DateTime)FromDate).ToString("dd.MM.yyyy HH:mm:ss") + " - " + ((DateTime)ToDate).AddDays(1).AddSeconds(-1).ToString("dd.MM.yyyy HH:mm:ss") }
                };
                ExcelService.InsertDataIntoCells(reportPath, sheetName, excelCells);

                int columnsNum = 11;
                List<List<string>> values = new List<List<string>>();
                List<List<CellValues>> valuesDataType = new List<List<CellValues>>();
                int indexReport = 1;
                foreach (Report report in GetReports().OrderBy(r => r.ReportDateTime).ToList())
                {
                    var reportComponents = LoadReportComponents(report);
                    string[,] row = new string[reportComponents.Count + 1, columnsNum];
                    CellValues[,] rowDataType = new CellValues[reportComponents.Count + 1, columnsNum];

                    row[0, 0] = indexReport.ToString();
                    row[0, 1] = report.ReportDateTime.ToString("dd.MM.yyyy");
                    row[0, 2] = report.ReportDateTime.ToString("HH:mm");
                    row[0, 3] = report.AssignmentName;
                    row[0, 4] = report.RecipeName;
                    row[0, 8] = report.AssignmentPlace;
                    row[0, 9] = report.AssignmentNote;
                    row[0, 10] = report.OperatorName;

                    rowDataType[0, 0] = CellValues.String;
                    rowDataType[0, 1] = CellValues.String;
                    rowDataType[0, 2] = CellValues.String;
                    rowDataType[0, 3] = CellValues.String;
                    rowDataType[0, 4] = CellValues.String;
                    rowDataType[0, 8] = CellValues.String;
                    rowDataType[0, 9] = CellValues.String;
                    rowDataType[0, 10] = CellValues.String;

                    double totalRequiredVolume = 0;
                    double totalDosedVolume = 0;
                    int indexComponent = 0;
                    foreach (ReportComponent reportComponent in reportComponents)
                    {
                        row[indexComponent, 5] = reportComponent.Name;
                        row[indexComponent, 6] = ((double)reportComponent.RequiredVolume).ToString("N2");
                        row[indexComponent, 7] = ((double)reportComponent.DosedVolume).ToString("N2");

                        rowDataType[indexComponent, 5] = CellValues.String;
                        rowDataType[indexComponent, 6] = CellValues.Number;
                        rowDataType[indexComponent, 7] = CellValues.Number;

                        totalRequiredVolume += (double)reportComponent.RequiredVolume;
                        totalDosedVolume += (double)reportComponent.DosedVolume;
                        
                        indexComponent++;
                    }

                    row[indexComponent, 5] = "Объем партии";
                    row[indexComponent, 6] = totalRequiredVolume.ToString("N2");
                    row[indexComponent, 7] = totalDosedVolume.ToString("N2");

                    rowDataType[indexComponent, 5] = CellValues.String;
                    rowDataType[indexComponent, 6] = CellValues.Number;
                    rowDataType[indexComponent, 7] = CellValues.Number;

                    for (int i = 0; i < row.GetLength(0); i++)
                    {
                        List<string> partyValues = new List<string>() { "" };
                        List<CellValues> partyValuesDataType = new List<CellValues>() { CellValues.String };
                        for (int j = 0; j < row.GetLength(1); j++)
                        {
                            partyValues.Add(row[i, j]);
                            partyValuesDataType.Add(rowDataType[i, j]);
                        }
                        values.Add(partyValues);
                        valuesDataType.Add(partyValuesDataType);
                    }
                    indexReport++;
                }

                ExcelStructure excelStructure = new ExcelStructure() { Headers = new List<string>(), Values = values, DataTypes = valuesDataType };
                ExcelService.InsertDataIntoSheet(reportPath, sheetName, excelStructure, 0);

                IsReportReady = true;
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
