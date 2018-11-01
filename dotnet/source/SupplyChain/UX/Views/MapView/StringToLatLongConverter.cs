using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Imi.SupplyChain.UX.Views
{

    public static class StringToLatLongConverter
    {
        public static LatLong ToLatLong(String latlong)
        {
            // expected format
            // N56 09.854 E13 46.825 loose this
            // N56 9 13.14 E13 46 5.28
            String[] parts = latlong.Split(null);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double minutes;
            double seconds;
            double time;

            Boolean northern = false;

            if (parts[0].StartsWith("N"))
                northern = true;
            else if (parts[0].StartsWith("S"))
                northern = false;
            else
                throw new ArgumentException("Illegal format, does not start with N or S. Format mask is Nxx xx xx.xxx Exxx xx xx.xxx");

            double lat = Convert.ToDouble(parts[0].Substring(1));

            minutes = Convert.ToDouble(parts[1]);
            seconds = (Convert.ToDouble(parts[2], provider) / 60.0);
            time = minutes + seconds;
            time /= 60;
            lat += time;

            Boolean eastern = false;

            if (parts[3].StartsWith("E"))
                eastern = true;
            else if (parts[3].StartsWith("W"))
                eastern = false;
            else
                throw new ArgumentException("Illegal format, does not start with E or W. Format mask is Nxx xx xx.xxx Exxx xx xx.xxx");

            double lng = Convert.ToDouble(parts[3].Substring(1));
            minutes = Convert.ToDouble(parts[4]);
            seconds = (Convert.ToDouble(parts[5], provider) / 60.0);
            time = minutes + seconds;
            time /= 60;
            lng += time;

            if (!northern)
                lat *= -1;

            if (!eastern)
                lng *= -1;

            return new LatLong() { Latitude = lat, Longitude = lng };
        }
    }
}
