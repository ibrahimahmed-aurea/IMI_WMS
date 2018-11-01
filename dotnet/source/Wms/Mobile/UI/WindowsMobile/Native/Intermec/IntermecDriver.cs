using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Intermec.DataCollection;
using Imi.Wms.Mobile.UI.Shared;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI.Native
{
    public class IntermecDriver : INativeDriver
    {
        public event EventHandler<ScanEventArgs> Scan;

        private BarcodeReader _reader;

        public IntermecDriver()
        {
            _reader = new BarcodeReader();
            _reader.BarcodeRead += reader_BarcodeRead;  
            _reader.ThreadedRead(true);
            _reader.ScannerEnable = true;
        }

        public Form Form { get; set; }

        private void reader_BarcodeRead(object sender, BarcodeReadEventArgs bre)
        {
            try
            {
                _reader.ScannerEnable = false;

                if (Scan != null)
                {
                    ScanEventArgs args = new ScanEventArgs();
                    args.Data = bre.strDataBuffer;

                    Scan(this, args);
                }
            }
            finally
            {
                _reader.ScannerEnable = true;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_reader != null)
            {
                try
                {
                    _reader.ScannerEnable = false;
                }
                catch
                {
                }
                _reader.Dispose();
                _reader = null;
            }
        }

        #endregion
    }
}
