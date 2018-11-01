using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.DataCollection
{
    public class ScannerService : IScannerService
    { 
        private IBarcodeDecoder decoder;

        public ScannerService(IBarcodeDecoder decoder)
        {
            this.decoder = decoder;
        }

        protected IList<BarcodeSegment> OnScanCompleted(string text)
        {
            return Decode(text);
        }

        private IList<BarcodeSegment> Decode(string text)
        {
            if (decoder == null)
            {
                BarcodeSegment segment = new BarcodeSegment();
                segment.Text = text;
                
                IList<BarcodeSegment> segments = new List<BarcodeSegment>();
                segments.Add(segment);

                return segments;
            }
            else
            {
                return decoder.Decode(text);
            }
        }

    }
}
