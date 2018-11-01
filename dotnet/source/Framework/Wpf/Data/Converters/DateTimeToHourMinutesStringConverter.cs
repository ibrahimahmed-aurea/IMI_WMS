using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Imi.Framework.Wpf.Data
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateTimeToHourMinutesStringConverter : System.Windows.Data.IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value != null) && (value.GetType() == typeof(DateTime)))
            {
                DateTime d = (DateTime)value;
                return (d.ToString("HH:mm"));
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
