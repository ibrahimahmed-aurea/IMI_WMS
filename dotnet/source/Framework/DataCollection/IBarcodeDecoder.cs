using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.DataCollection
{
    public interface IBarcodeDecoder
    {
        IList<BarcodeSegment> Decode(string text);
    }
}
