using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI.Shared
{
    public class ScanEventArgs : EventArgs
    {
        public string Data { get; set; }
    }

    public interface INativeDriver : IDisposable
    {
        event EventHandler<ScanEventArgs> Scan;
        Form Form { get; set; }
    }
}
