using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Imi.SupplyChain.UX.Views
{
    public class ImportPresenter : DataPresenter<IImportView>
    {
        [ServiceDependency]
        public IActionCatalogService ActionCatalog
        {
            get;
            set;
        }

        [ServiceDependency]
        public IUXSettingsService SettingsService
        {
            get;
            set;
        }

        public Dictionary<string, int> PropertyToExcelColumnIIndexDic = new Dictionary<string, int>();


        public virtual void OnViewUpdated(object viewResults)
        {
            foreach (object item in WorkItem.Items.FindByType(viewResults.GetType()))
                WorkItem.Items.Remove(item);

            foreach (object item in WorkItem.Items.FindByType(viewResults.GetType().GetGenericArguments()[0]))
                WorkItem.Items.Remove(item);

            if ((viewResults != null))
            {
                WorkItem.Items.Add(viewResults);
            }
        }

        public virtual object ReadExcelData(string excelFilePath, Type dataType)
        {
            object resultData = null;

            if (!string.IsNullOrEmpty(excelFilePath))
            {
                Excel.Application excelApp = null;
                Excel.Workbook workBook = null;
                Excel.Sheets sheets = null;
                Excel.Worksheet worksheet = null;
                Excel.Range usedRange = null;

                try
                {

                    excelApp = new Excel.Application();
                    excelApp.Visible = false;
                    excelApp.DisplayAlerts = false;

                    // Open Workbook
                    workBook = excelApp.Workbooks.Open(excelFilePath, ReadOnly: true);

                    //Excel.Sheets
                    sheets = workBook.Sheets;

                    worksheet = (Excel.Worksheet)sheets[1];


                    //Get the used Range
                    usedRange = worksheet.UsedRange;

                    bool firstRow = true;
                    bool firstRowOk = false;
                    //Iterate the rows in the used range
                    foreach (Excel.Range row in usedRange.Rows)
                    {
                        bool nullRow = true;
                        //Iterate through the row's data and put in a string array
                        object[] rowData = new object[row.Columns.Count];
                        for (int i = 0; i < row.Columns.Count; i++)
                        {
                            rowData[i] = row.Cells[1, i + 1].Value2;
                            
                            if (rowData[i] != null)
                            {
                                nullRow = false;
                            }
                        }

                        if (nullRow)
                        {
                            break;
                        }

                        if (firstRow)
                        {
                            firstRow = false;

                            firstRowOk = CheckFirstRow(rowData);

                            if (firstRowOk)
                            {
                                Type listGenericType = typeof(List<>);
                                Type listType = listGenericType.MakeGenericType(dataType);
                                ConstructorInfo ci = listType.GetConstructor(new Type[] { });
                                resultData = ci.Invoke(new object[] { });

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            object newRow = Activator.CreateInstance(dataType);

                            foreach (KeyValuePair<string, int> PropertyAndIndex in PropertyToExcelColumnIIndexDic)
                            {
                                if (rowData[PropertyAndIndex.Value] != null && rowData[PropertyAndIndex.Value].GetType() == typeof(double) && ((System.Type)dataType.GetProperty(PropertyAndIndex.Key).PropertyType).GetGenericArguments()[0] == typeof(decimal))
                                {
                                    dataType.GetProperty(PropertyAndIndex.Key).SetValue(newRow, Convert.ToDecimal(rowData[PropertyAndIndex.Value]), null);
                                }
                                else if (rowData[PropertyAndIndex.Value] != null && rowData[PropertyAndIndex.Value].GetType() == typeof(double) && ((System.Type)dataType.GetProperty(PropertyAndIndex.Key).PropertyType).GetGenericArguments()[0] == typeof(int))
                                {
                                    dataType.GetProperty(PropertyAndIndex.Key).SetValue(newRow, Convert.ToInt32(rowData[PropertyAndIndex.Value]), null);
                                }
                                else if (rowData[PropertyAndIndex.Value] != null && rowData[PropertyAndIndex.Value].GetType() == typeof(double) && ((System.Type)dataType.GetProperty(PropertyAndIndex.Key).PropertyType).GetGenericArguments()[0] == typeof(DateTime))
                                {
                                    dataType.GetProperty(PropertyAndIndex.Key).SetValue(newRow, DateTime.FromOADate((double)rowData[PropertyAndIndex.Value]), null);
                                }
                                else
                                {
                                    dataType.GetProperty(PropertyAndIndex.Key).SetValue(newRow, rowData[PropertyAndIndex.Value], null);
                                }
                            }

                            resultData.GetType().GetMethod("Add").Invoke(resultData, new object[] { newRow });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (usedRange != null)
                    {
                        Marshal.ReleaseComObject(usedRange);
                    }

                    if (worksheet != null)
                    {
                        Marshal.ReleaseComObject(worksheet);
                    }

                    if (sheets != null)
                    {
                        Marshal.ReleaseComObject(sheets);
                    }

                    if (workBook != null)
                    {
                        Marshal.ReleaseComObject(workBook);
                    }

                    if (excelApp != null)
                    {
                        excelApp.Quit();
                        Marshal.ReleaseComObject(excelApp);
                    }

                    usedRange = null;
                    worksheet = null;
                    sheets = null;
                    workBook = null;
                    excelApp = null;

                    GC.Collect();
                }

            }

            return resultData;
        }

        private bool CheckFirstRow(object[] rowData)
        {
            for (int i = 0; i < rowData.Length; i++)
            {
                string columnName = rowData[i].ToString();
                if (PropertyToExcelColumnIIndexDic.ContainsKey(columnName))
                {
                    PropertyToExcelColumnIIndexDic[columnName] = i;
                }
            }

            foreach (int colIndex in PropertyToExcelColumnIIndexDic.Values)
            {
                if (colIndex == -1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
