using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Imi.Framework.Wpf.Controls
{
    public class ExcelHelper
    {

        #region Help functions

        private static readonly char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private static string AdjustSheetName(string p)
        {
            StringBuilder s = new StringBuilder();

            foreach (char c in p)
            {
                if (char.IsLetterOrDigit(c) || c.Equals(' ') || c.Equals('+'))
                {
                    s.Append(c);
                    if (s.Length == 31)
                        break;
                }
            }

            // return max 31 characters
            return s.ToString();
        }

        //In COM, colors are represented as an integer in the 00BBGGRR format. 
        private static Int32 COMColor(Color color)
        {
            return (color.B << 16) | (color.G << 8) | color.R;
        }

        private static string ToExcelCellName(int col, int row)
        {
            string cellName = string.Empty;
            List<int> valueBases = new List<int>();
            int currentValue = col;

            valueBases.Add(1);

            while (col >= (Math.Pow(26, (valueBases.Count)) + valueBases.Sum()))
            {
                valueBases.Add((int)Math.Pow(26, (valueBases.Count)));
            }

            valueBases.Reverse();
            foreach (int valueBase in valueBases)
            {
                int letterIndex = (int)Math.Floor(((double)currentValue - (valueBase > 1 ? 0.1 : 0)) / valueBase);

                cellName += letters[letterIndex - 1];

                currentValue = currentValue - (valueBase * letterIndex);
            }

            cellName += row.ToString();

            return cellName;
        }

        private static void MergeCells(Worksheet worksheet, int x, int y, int x2, int y2, Color borderColor)
        {
            string firstCellName = ToExcelCellName(x, y);
            string secondCellName = ToExcelCellName(x2, y2);

            Range workSheet_range = worksheet.get_Range(firstCellName, secondCellName);
            workSheet_range.Merge(false);
            workSheet_range.Borders.Color = COMColor(borderColor);

        }

        private static void SetRangeValues(Worksheet worksheet, int x, int y, int x2, int y2, object[,] data)
        {
            string firstCellName = ToExcelCellName(x, y);
            string secondCellName = ToExcelCellName(x2, y2);

            Range workSheet_range = worksheet.get_Range(firstCellName, secondCellName);

            workSheet_range.Cells.Value2 = data;
        }

        private static void SetTableName(Worksheet worksheet, int x, int y, int x2, int y2, string name)
        {
            string firstCellName = ToExcelCellName(x, y);
            string secondCellName = ToExcelCellName(x2, y2);

            Range workSheet_range = worksheet.get_Range(firstCellName, secondCellName);

            workSheet_range.Name = name;
        }

        private static void SetCellColumnVisuals(Worksheet worksheet, int x, int y, int rowCount, string numberFormat, Color backGround, Color foreGround, int fontSize, bool isBold)
        {
            string cellFromName = ToExcelCellName(x, y);
            string cellToName = ToExcelCellName(x, y + rowCount - 1);

            Range workSheet_range = worksheet.get_Range(cellFromName, cellToName);

            workSheet_range.VerticalAlignment = Constants.xlTop;

            workSheet_range.NumberFormat = numberFormat;

            if (backGround != null)
            {
                workSheet_range.Interior.Color = COMColor(backGround);
            }

            if (foreGround != null)
            {
                workSheet_range.Borders.Color = COMColor(foreGround);
                workSheet_range.Font.Color = COMColor(foreGround);
            }

            workSheet_range.Font.Name = "Segoe UI";
            workSheet_range.Font.Size = fontSize;
            workSheet_range.Font.Bold = isBold;
        }

        private static void SetCellValueAndVisuals(Worksheet worksheet, int x, int y, object cellValue, string numberFormat, Color backGround, Color foreGround, int fontSize, bool isBold)
        {
            string cellName = ToExcelCellName(x, y);
            worksheet.Cells[y, x] = cellValue;

            Range workSheet_range = worksheet.get_Range(cellName, cellName);
            workSheet_range.NumberFormat = numberFormat;

            if (backGround != null)
            {
                workSheet_range.Interior.Color = COMColor(backGround);
            }

            if (foreGround != null)
            {
                workSheet_range.Borders.Color = COMColor(foreGround);
                workSheet_range.Font.Color = COMColor(foreGround);
            }

            workSheet_range.Font.Name = "Segoe UI";
            workSheet_range.Font.Size = fontSize;
            workSheet_range.Font.Bold = isBold;
        }

        private static void AutoFitColumn(Worksheet worksheet, int x)
        {
            string cellName = ToExcelCellName(x, 1);

            Range workSheet_range = worksheet.get_Range(cellName, cellName);
            workSheet_range.EntireColumn.AutoFit();
        }

        private static void FillSheet(Worksheet theSheet, int x, int y, DataExportContainer dataContainer, bool allData, bool importTemplate, string importTemplateName = "")
        {
            int headerRow = y;
            int captionRow = y + 1;
            int dataColumnCaptionRow = y + 2;
            int firstDataRow = y + 3;

            if (importTemplate)
            {
                dataColumnCaptionRow = 0;
                captionRow = y;
                firstDataRow = y + 1;
            }

            IEnumerable<IEnumerable<object>> dataRows = dataContainer.GetDataRows(allData, importTemplate, importTemplateName);

            if (dataRows.Count() > 0)
            {
                List<string> columnFormats = new List<string>(dataContainer.GetColumnFormatStrings(allData, importTemplate, importTemplateName));

                object[,] data = new object[dataRows.Count(), columnFormats.Count()];

                int rowRange = dataRows.Count();
                if (importTemplate)
                {
                    if (rowRange < 5000)
                    {
                        rowRange = 5000;
                    }
                }

                // Set visual styles for dataarea
                for (int i = 0; i < columnFormats.Count; i++)
                {
                    SetCellColumnVisuals(theSheet, x + i, firstDataRow, rowRange, columnFormats[i], Brushes.White.Color, Brushes.Black.Color, 10, false);
                }

                // Create array of data
                int rowIdx = 0;

                foreach (IEnumerable<object> row in dataRows)
                {
                    int colIdx = 0;

                    foreach (object columnValue in row)
                    {
                        object value = columnValue;

                        if (value is string)
                        {
                            value = string.Format("'{0}", columnValue);
                        }

                        data[rowIdx, colIdx] = value;
                        colIdx++;
                    }

                    rowIdx++;
                }

                // Set the values in the sheet
                SetRangeValues(theSheet, x, firstDataRow, (x + columnFormats.Count() - 1), (firstDataRow + dataRows.Count() - 1), data);
                SetTableName(theSheet, x, captionRow, (x + columnFormats.Count() - 1), (firstDataRow + dataRows.Count() - 1), "DataTable");
            }

            // Set Captions on columns and autofit columns

            IEnumerable<string> headerCaptions = null;
            IEnumerable<string> dataColumnHeaderCaptions = null;


            headerCaptions = dataContainer.GetHeaderCaptions(allData, importTemplate, importTemplate, importTemplateName);

            if (!importTemplate)
            {
                dataColumnHeaderCaptions = dataContainer.GetDataColumnHeaderCaptions(allData);
            }

            int columnIndex = x;
            foreach (string caption in headerCaptions)
            {
                SetCellValueAndVisuals(theSheet, columnIndex, captionRow, caption, "", Brushes.LightGray.Color, Brushes.Black.Color, 10, true);
                columnIndex++;
            }

            if (!importTemplate)
            {
                columnIndex = x;
                foreach (string dataCaption in dataColumnHeaderCaptions)
                {
                    SetCellValueAndVisuals(theSheet, columnIndex, dataColumnCaptionRow, dataCaption, "", Brushes.LightGray.Color, Brushes.Black.Color, 10, true);
                    AutoFitColumn(theSheet, columnIndex);
                    columnIndex++;
                }


                // Set heading
                SetCellValueAndVisuals(theSheet, x, y, theSheet.Name, "", new Color() { R = 0, G = 112, B = 192 }, Brushes.White.Color, 14, true);
                MergeCells(theSheet, x, y, x + headerCaptions.Count() - 1, y, Brushes.Black.Color);
            }

            theSheet.EnableAutoFilter = true;
        }

        #endregion

        public static void CreateExcelSheet(int x, int y, DataExportContainer dataContainer)
        {
            Excel.Application excelApp = null;
            Workbook workbook = null;
            Sheets sheets = null;
            Worksheet firstSheet = null;
            Worksheet allDataSheet = null;

            CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            bool makeImportTemplate = true;

            try
            {

                if (dataContainer.ImportTemplateNames.Count == 0) { makeImportTemplate = false; }

                #region Setup Excel, Workbook and sheet

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add(Type.Missing) as Excel.Workbook;
                sheets = workbook.Sheets;

                int totalNumOfSheets = 2;


                if (makeImportTemplate) { totalNumOfSheets += dataContainer.ImportTemplateNames.Count; }

                int sheetsToAdd = totalNumOfSheets - sheets.Count;

                if (sheetsToAdd < 0)
                {
                    for (int i = 0; i < Math.Abs(sheetsToAdd); i++)
                    {
                        ((Worksheet)sheets[1]).Delete();
                    }

                    sheetsToAdd = totalNumOfSheets - sheets.Count;
                }

                if (sheetsToAdd > 0)
                {
                    sheets.Add(Type.Missing, Type.Missing, sheetsToAdd);
                }


                firstSheet = (Worksheet)sheets[1];
                allDataSheet = (Worksheet)sheets[2];


                #endregion

                string title = dataContainer.GetTitle(false);

                if (!string.IsNullOrEmpty(title))
                {
                    firstSheet.Name = AdjustSheetName(title);
                }

                string titleAllDataSheet = dataContainer.GetTitle(true);

                if (!string.IsNullOrEmpty(titleAllDataSheet))
                {
                    allDataSheet.Name = AdjustSheetName(titleAllDataSheet);
                }

                if (makeImportTemplate)
                {
                    int i = 2;
                    foreach (string templateName in dataContainer.ImportTemplateNames)
                    {
                        i++;
                        string caption = "Import Template (" + templateName.Split(' ')[0] + ")";
                        if (dataContainer.ImportTemplateNames.Count > 1)
                        {
                            if (caption.Length > 30)
                            {
                                caption = "Import Template (" + i.ToString() + ")";
                            }
                        }
                        else
                        {
                            caption = "Import Template";
                        }

                        ((Worksheet)sheets[i]).Name = caption;
                    }
                }

                FillSheet(firstSheet, x, y, dataContainer, false, false);
                FillSheet(allDataSheet, x, y, dataContainer, true, false);

                if (makeImportTemplate)
                {
                    int i = 2;
                    foreach (string templateName in dataContainer.ImportTemplateNames)
                    {
                        i++;
                        FillSheet((Worksheet)sheets[i], x, y, dataContainer, true, true, templateName);
                    }
                }


                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (firstSheet != null)
                    Marshal.ReleaseComObject(firstSheet);

                if (allDataSheet != null)
                    Marshal.ReleaseComObject(allDataSheet);

                if (makeImportTemplate)
                {
                    int i = 2;
                    foreach (string templateName in dataContainer.ImportTemplateNames)
                    {
                        i++;
                        Marshal.ReleaseComObject((Worksheet)sheets[i]);
                    }
                }


                if (sheets != null)
                    Marshal.ReleaseComObject(sheets);

                if (workbook != null)
                    Marshal.ReleaseComObject(workbook);

                if (excelApp != null)
                    Marshal.ReleaseComObject(excelApp);

                firstSheet = null;
                sheets = null;
                workbook = null;
                excelApp = null;

                Thread.CurrentThread.CurrentCulture = oldCulture;

                GC.Collect();
            }
        }



    }
}
