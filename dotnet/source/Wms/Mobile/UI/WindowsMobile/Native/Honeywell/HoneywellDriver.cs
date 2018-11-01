using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Intermec.DataCollection;
using Imi.Wms.Mobile.UI.Shared;
using HSM.Embedded.Decoding;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI.Native
{
    public class HoneywellDriver : INativeDriver
    {
        public event EventHandler<ScanEventArgs> Scan;

        private DecodeAssembly _reader;
        private Form _form;

        public HoneywellDriver()
        {
            _reader = new DecodeAssembly();
            _reader.Connect();
            _reader.DecodeEvent += new DecodeAssembly.DecodeEventHandler(_reader_DecodeEvent);
            
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.Code128Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.Code39Base32Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.Code39Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.Code93Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.GS1_128Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.ISBT128Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.MatrixCodes.PDF417Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.MatrixCodes.QRCodeEnabled, 1);
                        
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_13_Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_13_ChKDigitXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_13_2DigitAddendaEnabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_13_5DigitAddendaEnabled, 1);

            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_8_Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_8_ChKDigitXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_8_2DigitAddendaEnabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.EAN_8_5DigitAddendaEnabled, 1);
            
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_NumSysXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_ChKDigitXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_AddendaXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_2DigitAddendaEnabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_A_5DigitAddendaEnabled, 1);

            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_E0_Enabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_E_ChKDigitXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_E_NumSysXmit, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_E_2DigitAddendaEnabled, 1);
            _reader.SetDecoderProperty(DecodeAssembly.RetailCodes.UPC_E_5DigitAddendaEnabled, 1);

            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.Int25Enabled, 1);

            _reader.SetDecoderProperty(DecodeAssembly.LinearCodes.IATA25Enabled, 1);

        }

        public Form Form 
        {
            get
            {
                return _form;
            }
            set
            {
                if (_form != null)
                {
                    _form.KeyDown -= _form_KeyDown;
                    _form.KeyUp -= _form_KeyUp;
                }

                _form = value;

                if (_form != null)
                {
                    _form.KeyDown += _form_KeyDown;
                    _form.KeyUp += _form_KeyUp;
                }
            }
        }
                
        void _form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)42)
            {
                Logger.Write("Honeywell - Begin Scan");

                _form.KeyDown -= _form_KeyDown;

                try
                {
                    _reader.ScanBarcode();
                }
                catch (Exception ex)
                {
                    Logger.Write(ex.ToString());
                }

                e.Handled = true;
            }
        }

        void _form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)42)
            {
                Logger.Write("Honeywell - End Scan");

                _form.KeyDown += _form_KeyDown;
                
                try
                {
                    _reader.CancelScanBarcode();
                }
                catch (Exception ex)
                {
                    Logger.Write(ex.ToString());
                }

                e.Handled = true;
            }
        }
        
        private void _reader_DecodeEvent(object sender, DecodeAssembly.DecodeEventArgs e)
        {
            if (e.ResultCode == DecodeAssembly.ResultCodes.Success && Scan != null)
            {
                System.Media.SystemSounds.Beep.Play();

                ScanEventArgs args = new ScanEventArgs();
                
                args.Data = e.Message;
                
                Scan(this, args);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_reader != null)
            {
                try
                {
                    _reader.CancelScanBarcode();
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
