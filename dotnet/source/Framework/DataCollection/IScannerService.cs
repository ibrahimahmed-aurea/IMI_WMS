using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.DataCollection
{
    public class ScanCompletedEventArgs : EventArgs
    {
        public IList<BarcodeSegment> Segments { get; set; }
    }

    public delegate void ScanCompletedEventHandler(object sender, ScanCompletedEventArgs e);

    public interface IScannerService
    {

    }
}
