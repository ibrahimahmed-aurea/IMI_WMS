using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Imi.Framework.Wpf.Data
{
    [ValueConversion(typeof(int), typeof(int))]
    public class InvertValueConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter,
                               System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(int))
            {
                throw new System.ArgumentException("This converter can only convert integer values",
                                                    "value");
            }

            int index = (int)value;

            return index * -1; 

        }

        public object ConvertBack(object value, System.Type targetType,
                                   object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
