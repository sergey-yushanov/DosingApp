using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DosingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DosingApp.Services
{
    public class ExcelCell
    {
        public string ColumnName { get; set; }
        public uint RowIndex { get; set; }
        public string Text { get; set; }
    }

    public class ExcelService
    {
        public class ExcelStructure
        {
            public List<string> Headers { get; set; } = new List<string>();
            public List<List<string>> Values { get; set; } = new List<List<string>>();
            public List<List<CellValues>> DataTypes { get; set; } = new List<List<CellValues>>();
        }

        private Cell ConstructCell(string value, CellValues dataTypes) =>
            new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataTypes)
            };

        private Cell ConstructCell(double value, CellValues dataTypes) =>
            new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataTypes)
            };

        public string GenerateExcel(string filePath, string sheetName)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            var wbPart = document.AddWorkbookPart();
            wbPart.Workbook = new Workbook();

            var part = wbPart.AddNewPart<WorksheetPart>();
            part.Worksheet = new Worksheet(new SheetData());

            //  Here are created the sheets, you can add all the child sheets that you need.
            var sheets = wbPart.Workbook.AppendChild(
                new Sheets(
                    new Sheet()
                    {
                        Id = wbPart.GetIdOfPart(part),
                        SheetId = 1,
                        Name = sheetName
                    }
                )
            );

            // Just save and close you Excel file
            wbPart.Workbook.Save();
            document.Dispose();
            // Dont't forget return the filePath
            return filePath;
        }

        public string GenerateExcelFromTemplate(string filePath)
        {
            File.Delete(filePath);

            // получаем текущую сборку
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            // берем из нее ресурс и создаем из него поток
            using (Stream stream = assembly.GetManifestResourceStream($"DosingApp.Resources.Reports.{App.SOURCEREPORTFILENAME}"))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(fs);  // копируем файл в нужное нам место
                    fs.Flush();
                }
            }

            return filePath;
        }

        public void InsertDataIntoSheet(string fileName, string sheetName, ExcelStructure data, int rowOffsetCount)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            using (var document = SpreadsheetDocument.Open(fileName, true))
            {
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.GetFirstChild<Sheets>().
                             Elements<Sheet>().FirstOrDefault().
                             Name = sheetName;

                var part = wbPart.WorksheetParts.First();
                var sheetData = part.Worksheet.Elements<SheetData>().First();

                for (int i = 0; i < rowOffsetCount; i++)
                {
                    sheetData.AppendChild(new Row());
                }

                if (data.Headers.Count > 0)
                {
                    var row = sheetData.AppendChild(new Row());
                    foreach (var header in data.Headers)
                    {
                        row.Append(ConstructCell(header, CellValues.String));
                    }
                }

                int di = 0;
                foreach (var value in data.Values)
                {
                    int dj = 0;
                    var dataRow = sheetData.AppendChild(new Row());
                    foreach (var dataElement in value)
                    {
                        CellValues dataType = (dataElement == null) ? CellValues.String : data.DataTypes[di][dj];
                        if (dataType == CellValues.Number)
                        {
                            dataRow.Append(ConstructCell(double.Parse(dataElement), dataType));
                        }
                        else
                        {
                            dataRow.Append(ConstructCell(dataElement, dataType));
                        }                        
                        dj++;
                    }
                    di++;
                }
                wbPart.Workbook.Save();
            }
        }

        public void InsertDataIntoCells(string fileName, string sheetName, List<ExcelCell> excelCells)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, true))
            {
                // Get the SharedStringTablePart. If it does not exist, create a new one
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                {
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
                }

                Workbook workbook = spreadSheet.WorkbookPart.Workbook;
                Sheet sheet = workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(sheet.Id);

                foreach (ExcelCell excelCell in excelCells)
                {
                    // Insert the text into the SharedStringTablePart.
                    int index = InsertSharedStringItem(excelCell.Text, shareStringPart);
                    // Insert cell into the worksheet
                    Cell cell = InsertCellInWorksheet(excelCell.ColumnName, excelCell.RowIndex, worksheetPart);
                    // Set the value of cell
                    cell.CellValue = new CellValue(index.ToString());
                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                }

                // Save the worksheet
                worksheetPart.Worksheet.Save();
            }
        }


        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        // Given a WorkbookPart, inserts a new worksheet.
        private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
        {
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            string sheetName = "Sheet" + sheetId;

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        public void CreateReport(Report report, List<ReportComponent> reportComponents)
        {

        }

        public string InvoicePrepareToPrint(Report report, List<ReportComponent> reportComponents)
        {
            string excelFilePath = App.GetInvoiceFilePath(false);
            string pdfFilePath = Path.Combine(Path.Combine(App.FolderPath, App.InvoicesFolder), App.PDFINVOICEFILENAME);
            string fontsFolder = Path.Combine(App.FolderPath, App.FontsFolder);

            InvoiceToExcel(report, reportComponents);
            ConvertExcelToPdf(excelFilePath, pdfFilePath, fontsFolder);

            return pdfFilePath;
        }

        public void InvoiceToExcel(Report report, List<ReportComponent> reportComponents)
        {
            List<ExcelCell> excelCells = new List<ExcelCell>();

            ExcelCell excelCellNumber = new ExcelCell { ColumnName = "G", RowIndex = 5, Text = report.ReportId.ToString() };
            ExcelCell excelCellDate = new ExcelCell { ColumnName = "A", RowIndex = 11, Text = report.ReportDateTime.Date.ToString("dd.MM.yyyy") };

            uint rowIndexOffset = 19;
            uint i = 0;
            foreach (ReportComponent reportComponent in reportComponents)
            {
                excelCells.Add(new ExcelCell { ColumnName = "C", RowIndex = rowIndexOffset + i, Text = reportComponent.Name });
                excelCells.Add(new ExcelCell { ColumnName = "F", RowIndex = rowIndexOffset + i, Text = "л" });
                excelCells.Add(new ExcelCell { ColumnName = "G", RowIndex = rowIndexOffset + i, Text = ((double)reportComponent.DosedVolume).ToString("N2") });
                excelCells.Add(new ExcelCell { ColumnName = "H", RowIndex = rowIndexOffset + i, Text = ((double)reportComponent.DosedVolume).ToString("N2") });

                i++;
            }

            excelCells.Add(excelCellNumber);
            excelCells.Add(excelCellDate);

            string filePath = App.GetInvoiceFilePath(true);
            InsertDataIntoCells(filePath, "Лист1", excelCells);
        }

        public void ConvertExcelToPdf(string excelFile, string pdfFile, string fontsFolder)
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(excelFile, Spire.Xls.ExcelVersion.Version2010);
            workbook.CustomFontFileDirectory = new string[] { fontsFolder };
            workbook.SaveToFile(pdfFile, Spire.Xls.FileFormat.PDF);
        }
    }
}
