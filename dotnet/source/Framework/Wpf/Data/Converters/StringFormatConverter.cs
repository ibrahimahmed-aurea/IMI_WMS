using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;

namespace Imi.Framework.Wpf.Data.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value is string) && ((value as string).Equals("")))
            {
                return null;
            }
            else if (value == null)
            {
                return null;
            }
            else if (value is DateTime)
            {
                if (parameter != null && parameter is string)
                {
                    parameter = ((string)parameter).Replace("{", "{0:");
                }

                if ((parameter != null) && ((string)parameter).Contains("{0:"))
                    return string.Format((string)parameter, value);
                else
                    return ((DateTime)value).ToString((string)parameter);
            }
            else
            {
                string format = string.Format("{{0:{0}}}", parameter);
                return string.Format(format, value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else if (value.GetType() == targetType)
            {
                return value;
            }
            else if (value is string)
            {
                string s = value as string;

                if (string.IsNullOrEmpty(s))
                    return null;
                else if (targetType == typeof(DateTime?))
                {
                    DateTime result;

                    if (DateTime.TryParse(s, out result))
                        return new DateTime?(result);
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(DateTime))
                {
                    DateTime result;

                    if (DateTime.TryParse(s, out result))
                        return result;
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(int?))
                {
                    int result;

                    if (int.TryParse(s, out result))
                        return new int?(result);
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(int))
                {
                    int result;

                    if (int.TryParse(s, out result))
                        return result;
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(long?))
                {
                    long result;

                    if (long.TryParse(s, out result))
                        return new long?(result);
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(long))
                {
                    long result;

                    if (long.TryParse(s, out result))
                        return result;
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(double?))
                {
                    double result;

                    if (double.TryParse(s, out result))
                        return new double?(result);
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(double))
                {
                    double result;

                    if (double.TryParse(s, out result))
                        return result;
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(decimal?))
                {
                    decimal result;

                    if (decimal.TryParse(s, out result))
                        return new decimal?(result);
                    else
                        return DependencyProperty.UnsetValue;
                }
                else if (targetType == typeof(decimal))
                {
                    decimal result;

                    if (decimal.TryParse(s, out result))
                        return result;
                    else
                        return DependencyProperty.UnsetValue;
                }
                else
                    return DependencyProperty.UnsetValue;
            }
            else
                return DependencyProperty.UnsetValue;
        }
    }
}
