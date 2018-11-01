using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.Wpf.Controls
{
    public static class ControlHelper
    {
        public static bool IsControlOnView(object control, object view)
        {
            //If data context match the control should be on the view.
            if (control.GetType().GetProperty("DataContext") != null && view.GetType().GetProperty("DataContext") != null)
            {
                if (view.GetType().GetProperty("DataContext").GetValue(view, null) == control.GetType().GetProperty("DataContext").GetValue(control, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
