using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Imi.Framework.Wpf.Data
{
    [ValueConversion(typeof(string), typeof(bool?))]
    public class StringToNullableBoolConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter,
                               System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    switch (value as string)
                    {
                        case "0":
                            return false;
                        case "1":
                            return true;
                        default:
                            return null;
                    }
                }
                return
                    null;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("This converter can only convert string values",
                                    "value", ex);
            }
        }

        public object ConvertBack(object value, System.Type targetType,
                                   object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                switch (value as bool?)
                {
                    case true:
                        return "1";
                    case false:
                        return "0";
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("This converter can only convert bool values",
                                    "value", ex);
            }
        }
    }
}
