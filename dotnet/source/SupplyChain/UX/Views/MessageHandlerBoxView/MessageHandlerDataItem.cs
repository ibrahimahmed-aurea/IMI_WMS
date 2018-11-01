using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Imi.SupplyChain.UX.Views
{
    public enum MessageHandlerItemType { Information = 1, Warning = 2, Error = 4 };

    public class MessageHandlerDataItem
    {
        public MessageHandlerItemType Type { get; set; }
        public Brush TypeBrush { get; set; }
        public string Text { get; set; }
    }
}
