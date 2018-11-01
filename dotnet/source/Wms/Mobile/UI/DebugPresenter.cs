using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Text;
using System.Drawing;
using System.Reflection;

namespace Imi.Wms.Mobile.UI
{
    public class DebugPresenter
    {
        private DebugForm _form;
        
        public DebugPresenter(DebugForm form)
        {
            _form = form;
            AssembleDebugInfo();
        }

        private void AssembleDebugInfo()
        { 
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Version Information");
            sb.AppendLine("------------------------------");
            sb.AppendLine(Assembly.GetExecutingAssembly().FullName);
            sb.AppendLine("");
            sb.AppendLine("Device Platform");
            sb.AppendLine("------------------------------");
            sb.AppendLine(DeviceInfo.GetPlatform());
            sb.AppendLine("");
            sb.AppendLine("Installed Fonts");
            sb.AppendLine("------------------------------");
            
            using (InstalledFontCollection fontCollection = new InstalledFontCollection())
            {
                foreach (FontFamily font in fontCollection.Families)
                {
                    sb.AppendLine(font.Name);
                }
            }

            _form.ShowDebugInfo(sb.ToString());
        }

        public void EnableLogging(bool enable)
        {
            Logger.IsEnabled = enable;

        }
    }
}
