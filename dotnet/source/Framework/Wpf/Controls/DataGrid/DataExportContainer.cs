using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xceed.Wpf.DataGrid;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Imi.Framework.Wpf.Controls
{
    public class DataExportContainer
    {
        #region IExcelLeech Members

        private List<object> _dataBuffer = new List<object>();
        private string _title;
        private string _shortDatePattern;
        private List<Dictionary<byte, object>> _allPropertiesForExcelWithAttributesNeeded = new List<Dictionary<byte, object>>();
        private IDictionary<string, List<string>> _propertiesUsedByImport = new Dictionary<string, List<string>>();

        private static byte _ENTRY_HEADER = 1;
        private static byte _ENTRY_DATACOLUM_HEADER = 2;
        private static byte _ENTRY_FORMATSTRING = 3;
        private static byte _ENTRY_PROPERTYINFO = 4;
        private static byte _ENTRY_VISIBLE = 5;
        private static byte _ENTRY_USED_BY_IMPORT = 6;
        private static byte _ENTRY_ORDER_NUMBER = 7;

        public DataExportContainer(DataGrid grid, IDictionary<string, List<string>> propertiesUsedByImport)
        {
            _title = grid.Title;

            if (propertiesUsedByImport != null)
            {
                _propertiesUsedByImport = propertiesUsedByImport;
            }

            _shortDatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            if (_shortDatePattern.EndsWith(" tt"))
                _shortDatePattern = _shortDatePattern.Substring(0, _shortDatePattern.Length - 3);

            List<string> propertiesWithColumn = new List<string>();

            object firstItem = null;

            if ((grid.Items != null) && (grid.Items.Count > 0))
            {
                firstItem = grid.Items[0];
            }

            int orderNumber = 0;

            foreach (Column c in grid.Columns)
            {
                propertiesWithColumn.Add(c.FieldName);



                Dictionary<byte, object> newEntry = new Dictionary<byte, object>();
                newEntry.Add(_ENTRY_HEADER, c.Title.ToString());
                newEntry.Add(_ENTRY_DATACOLUM_HEADER, string.Empty);

                if (firstItem != null)
                {
                    PropertyInfo pi = firstItem.GetType().GetProperty(c.FieldName);

                    if (pi.GetCustomAttributes(typeof(DisplayNameAttribute), false).Count() > 0)
                    {
                        newEntry[_ENTRY_DATACOLUM_HEADER] = ((DisplayNameAttribute)pi.GetCustomAttributes(typeof(DisplayNameAttribute), false)[0]).DisplayName;

                        if (newEntry[_ENTRY_HEADER].ToString().Contains("\n\r[" + newEntry[_ENTRY_DATACOLUM_HEADER] + "]"))
                        {
                            newEntry[_ENTRY_HEADER] = newEntry[_ENTRY_HEADER].ToString().Replace("\n\r[" + newEntry[_ENTRY_DATACOLUM_HEADER] + "]", "");
                        }
                    }

                    newEntry.Add(_ENTRY_FORMATSTRING, GetFormatStringForPropertyInfo(pi));
                    newEntry.Add(_ENTRY_PROPERTYINFO, pi);

                    List<string> importsUsingProperty = new List<string>();
                    foreach (KeyValuePair<string, List<string>> item in _propertiesUsedByImport)
                    {
                        if (item.Value.Contains(pi.Name))
                        {
                            importsUsingProperty.Add(item.Key);
                        }
                    }

                    newEntry.Add(_ENTRY_USED_BY_IMPORT, importsUsingProperty);


                    orderNumber++;

                    newEntry.Add(_ENTRY_ORDER_NUMBER, orderNumber);
                }
                else
                {
                    newEntry.Add(_ENTRY_FORMATSTRING, "");
                }

                newEntry.Add(_ENTRY_VISIBLE, true);


                _allPropertiesForExcelWithAttributesNeeded.Add(newEntry);
            }

            if (firstItem != null)
            {
                foreach (PropertyInfo pi in firstItem.GetType().GetProperties())
                {
                    if (!propertiesWithColumn.Contains(pi.Name))
                    {
                        if (pi.GetGetMethod().GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Count() == 0)
                        {
                            Dictionary<byte, object> newEntry = new Dictionary<byte, object>();
                            newEntry.Add(_ENTRY_HEADER, pi.Name);
                            newEntry.Add(_ENTRY_DATACOLUM_HEADER, string.Empty);

                            if (pi.GetCustomAttributes(typeof(DisplayNameAttribute), false).Count() > 0)
                            {
                                newEntry[_ENTRY_DATACOLUM_HEADER] = ((DisplayNameAttribute)pi.GetCustomAttributes(typeof(DisplayNameAttribute), false)[0]).DisplayName;
                            }

                            newEntry.Add(_ENTRY_FORMATSTRING, GetFormatStringForPropertyInfo(pi));
                            newEntry.Add(_ENTRY_PROPERTYINFO, pi);
                            newEntry.Add(_ENTRY_VISIBLE, false);

                            List<string> importsUsingProperty = new List<string>();
                            foreach (KeyValuePair<string, List<string>> item in _propertiesUsedByImport)
                            {
                                if (item.Value.Contains(pi.Name))
                                {
                                    importsUsingProperty.Add(item.Key);
                                }
                            }

                            newEntry.Add(_ENTRY_USED_BY_IMPORT, importsUsingProperty);

                            orderNumber++;
                            newEntry.Add(_ENTRY_ORDER_NUMBER, orderNumber);

                            _allPropertiesForExcelWithAttributesNeeded.Add(newEntry);
                        }
                    }
                }
            }

        }

        public void AppendData(IList<object> data)
        {
            _dataBuffer.AddRange(data);
        }


        public string GetTitle(bool dataCarrier = false)
        {
            if (dataCarrier)
            {
                return "+" + _title;
            }
            else
            {
                return _title;
            }
        }

        public IEnumerable<string> GetDataColumnHeaderCaptions(bool dataCarrier = false)
        {
            if (dataCarrier)
            {
                return _allPropertiesForExcelWithAttributesNeeded.OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_DATACOLUM_HEADER].ToString());
            }
            else
            {
                return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE]).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_DATACOLUM_HEADER].ToString());
            }
        }

        public IEnumerable<string> GetHeaderCaptions(bool dataCarrier = false, bool onlyPropertyNames = false, bool onlyUsedByImport = false, string importTemplateName = "")
        {
            if (dataCarrier)
            {
                if (onlyPropertyNames)
                {
                    if (onlyUsedByImport)
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => ((List<string>) i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => ((PropertyInfo)i[_ENTRY_PROPERTYINFO]).Name);
                    }
                    else
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => ((PropertyInfo)i[_ENTRY_PROPERTYINFO]).Name);
                    }    
                }
                else
                {
                    if (onlyUsedByImport)
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_HEADER].ToString());
                    }
                    else
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_HEADER].ToString());
                    }
                }
            }
            else
            {
                if (onlyPropertyNames)
                {
                    if (onlyUsedByImport)
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE] && ((List<string>) i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => ((PropertyInfo)i[_ENTRY_PROPERTYINFO]).Name);
                    }
                    else
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE]).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => ((PropertyInfo)i[_ENTRY_PROPERTYINFO]).Name);
                    }
                }
                else
                {
                    if (onlyUsedByImport)
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE] && ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_HEADER].ToString());
                    }
                    else
                    {
                        return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE]).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_HEADER].ToString());
                    }
                }
            }
        }


        public IEnumerable<string> GetColumnFormatStrings(bool dataCarrier = false, bool onlyUsedByImport = false, string importTemplateName = "")
        {
            if (dataCarrier)
            {
                if (onlyUsedByImport)
                {
                    return _allPropertiesForExcelWithAttributesNeeded.Where(i => ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_FORMATSTRING].ToString());
                }
                else
                {
                    return _allPropertiesForExcelWithAttributesNeeded.OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_FORMATSTRING].ToString());
                }
            }
            else
            {
                if (onlyUsedByImport)
                {
                    return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE] && ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_FORMATSTRING].ToString());
                }
                else
                {
                    return _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE]).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(i => i[_ENTRY_FORMATSTRING].ToString());
                }
            }
        }


        public IEnumerable<IEnumerable<object>> GetDataRows(bool dataCarrier = false, bool onlyUsedByImport = false, string importTemplateName = "")
        {
            List<IEnumerable<object>> rows = new List<IEnumerable<object>>();

            if ((_dataBuffer != null) && (_dataBuffer.Count > 0))
            {
                List<PropertyInfo> pis = null;

                if (dataCarrier)
                {
                    if (onlyUsedByImport)
                    {
                        pis = _allPropertiesForExcelWithAttributesNeeded.Where(i => ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(e => e[_ENTRY_PROPERTYINFO] as PropertyInfo).ToList();
                    }
                    else
                    {
                        pis = _allPropertiesForExcelWithAttributesNeeded.OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(e => e[_ENTRY_PROPERTYINFO] as PropertyInfo).ToList();
                    }
                }
                else
                {
                    if (onlyUsedByImport)
                    {
                        pis = _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE] && ((List<string>)i[_ENTRY_USED_BY_IMPORT]).Contains(importTemplateName)).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(e => e[_ENTRY_PROPERTYINFO] as PropertyInfo).ToList();
                    }
                    else
                    {
                        pis = _allPropertiesForExcelWithAttributesNeeded.Where(i => (bool)i[_ENTRY_VISIBLE]).OrderBy(i => (int)i[_ENTRY_ORDER_NUMBER]).Select(e => e[_ENTRY_PROPERTYINFO] as PropertyInfo).ToList();
                    }
                }

                for (int i = 0; i < _dataBuffer.Count; i++)
                {
                    object item = _dataBuffer[i];

                    List<object> row = new List<object>();

                    foreach (PropertyInfo pi in pis)
                    {
                        row.Add(pi.GetValue(item, null));
                    }

                    rows.Add(row);
                }
            }

            return rows;
        }

        public List<string> ImportTemplateNames
        {
            get { return _propertiesUsedByImport.Keys.ToList(); }
        }


        #region private

        private string GetFormatStringForPropertyInfo(PropertyInfo pi)
        {
            Type t = pi.PropertyType;

            if ((t == typeof(DateTime)) || (t == typeof(DateTime?)))
            {
                return _shortDatePattern;
            }
            else
            {
                if (t == typeof(string))
                {
                    return "@";
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        #endregion
    }
}


