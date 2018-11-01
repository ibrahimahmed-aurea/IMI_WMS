using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Intermec.DataCollection;
using Imi.Wms.Mobile.UI.Shared;
using HandHeldProducts.Embedded.Decoding;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.UI.Native
{
    public class Dolphin6XDriver : INativeDriver
    {
        public event EventHandler<ScanEventArgs> Scan;

        private DecodeAssembly _reader;
        private SymbologyConfigurator _confEan13;
        private SymbologyConfigurator _confEan8;
        private SymbologyConfigurator _confUPCA;
        private SymbologyConfigurator _confUPCE0;
        private Form _form;

        public Dolphin6XDriver()
        {
            _reader = new DecodeAssembly();
            _reader.DecodeEvent += new DecodeAssembly.DecodeEventHandler(_reader_DecodeEvent);

            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.Code128, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.DataMatrix, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.Code39, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.Code93, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.GS1_128, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.ISBT, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.PDF417, true);
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.QR, true);
            
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.EAN13, true);
            _confEan13 = new SymbologyConfigurator(SymbologyConfigurator.Symbologies.EAN13);
            _confEan13.ReadConfig(DecodeAssembly.SetupTypes.Current);
            _confEan13.Flags = SymbologyConfigurator.SymbologyFlags.Enable;
            _confEan13.Flags |= SymbologyConfigurator.SymbologyFlags.CheckTransmit;
            _confEan13.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda2Digit;
            _confEan13.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda5Digit;
            _confEan13.WriteConfig();
            _confEan13.Dispose();
            
            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.EAN8, true);
            _confEan8 = new SymbologyConfigurator(SymbologyConfigurator.Symbologies.EAN8);
            _confEan8.ReadConfig(DecodeAssembly.SetupTypes.Current);
            _confEan8.Flags = SymbologyConfigurator.SymbologyFlags.Enable;
            _confEan8.Flags |= SymbologyConfigurator.SymbologyFlags.CheckTransmit;
            _confEan8.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda2Digit;
            _confEan8.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda5Digit;
            _confEan8.WriteConfig();
            _confEan8.Dispose();

            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.UPCA, true);
            _confUPCA = new SymbologyConfigurator(SymbologyConfigurator.Symbologies.UPCA);
            _confUPCA.ReadConfig(DecodeAssembly.SetupTypes.Current);
            _confUPCA.Flags = SymbologyConfigurator.SymbologyFlags.Enable;
            _confUPCA.Flags |= SymbologyConfigurator.SymbologyFlags.CheckTransmit;
            _confUPCA.Flags |= SymbologyConfigurator.SymbologyFlags.NumSysTransmit;
            _confUPCA.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda2Digit;
            _confUPCA.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda5Digit;
            _confUPCA.WriteConfig();
            _confUPCA.Dispose();

            _reader.EnableSymbology(SymbologyConfigurator.Symbologies.UPCE0, true);
            _confUPCE0 = new SymbologyConfigurator(SymbologyConfigurator.Symbologies.UPCE0);
            _confUPCE0.ReadConfig(DecodeAssembly.SetupTypes.Current);
            _confUPCE0.Flags = SymbologyConfigurator.SymbologyFlags.Enable;
            _confUPCE0.Flags |= SymbologyConfigurator.SymbologyFlags.CheckTransmit;
            _confUPCE0.Flags |= SymbologyConfigurator.SymbologyFlags.NumSysTransmit;
            _confUPCE0.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda2Digit;
            _confUPCE0.Flags |= SymbologyConfigurator.SymbologyFlags.Addenda5Digit;
            _confUPCE0.WriteConfig();
            _confUPCE0.Dispose();
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
