using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Imi.Wms.Mobile.UI.Shared;
using Symbol.Barcode;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI.Native
{
    public class MotorolaDriver : INativeDriver
    {
        private Symbol.Barcode.Reader _reader;
        private Symbol.Barcode.ReaderData _readerData = null;
        public event EventHandler<ScanEventArgs> Scan;

        public MotorolaDriver()
        {

            _reader = new Reader();

            _readerData = new Symbol.Barcode.ReaderData(Symbol.Barcode.ReaderDataTypes.Text, Symbol.Barcode.ReaderDataLengths.MaximumLabel);

            _reader.Actions.Enable();

            _reader.Decoders.DisableAll();

            _reader.Decoders.UPCA.Enabled = true;
            _reader.Decoders.UPCA.ReportCheckDigit = true;

            _reader.Decoders.UPCE0.Enabled = true;
            _reader.Decoders.UPCE0.ReportCheckDigit = true;

            _reader.Decoders.CODE39.Enabled = true;
            _reader.Decoders.CODE39.ReportCheckDigit = true;

            _reader.Decoders.EAN8.Enabled = true;
            _reader.Decoders.EAN8.ConvertToEAN13 = true;

            _reader.Decoders.CODE128.Enabled = true;
            _reader.Decoders.EAN13.Enabled = true;

            _reader.Decoders.QRCODE.Enabled = true;

            _reader.ReadNotify += new EventHandler(_reader_ReadNotify);

            _reader.Actions.Read(_readerData);

        }

        public Form Form { get; set; }

        void _reader_ReadNotify(object sender, EventArgs e)
        {
            try
            {
                if (Scan != null)
                {
                    ScanEventArgs args = new ScanEventArgs();

                    Symbol.Barcode.ReaderData nextReaderData = _reader.GetNextReaderData();  //Get(s)NextReaderData
                    args.Data = nextReaderData.Text;


                    Scan(this, args);
                }
            }
            finally
            {
                _reader.Actions.Read(_readerData);  //await next scan.
            }

        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_reader != null)
            {
                try
                {
                    _reader.Actions.Flush();
                    _reader.Actions.Disable();
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
