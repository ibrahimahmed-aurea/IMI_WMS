using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Imi.Framework.Wpf.Data.Converters
{
    [ValueConversion(typeof(bool?), typeof(bool?))]
    public class InvertNullableBoolConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter,
                               System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.GetType() != typeof(bool?) &&
                value.GetType() != typeof(bool))
            {
                throw new System.ArgumentException("This converter can only invert boolean values",
                                                   "value");
            }

            bool? convValue = (bool?)value;

            if (convValue.HasValue)
            {
                if (convValue.Value)
                    return false;
                else
                    return true;
            }
            else
                return null;
        }

        public object ConvertBack(object value, System.Type targetType,
                                   object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.GetType() != typeof(bool?) &&
                value.GetType() != typeof(bool))
            {
                throw new System.ArgumentException("This converter can only invert boolean values",
                                                   "value");
            }

            bool? convValue = (bool?)value;

            if (convValue.HasValue)
            {
                if (convValue.Value)
                    return false;
                else
                    return true;
            }
            else
                return null;
        }
    }
}
