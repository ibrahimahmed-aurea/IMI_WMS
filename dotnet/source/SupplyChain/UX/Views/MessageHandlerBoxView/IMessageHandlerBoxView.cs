using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Views
{
    public enum MessageHandlerBoxButton
    {
        Ok,
        OkCancel,
        YesNoCancel,
        YesNo
    }

    public enum MessageHandlerBoxImage
    { 
        Error,
        Warning,
        Information
    }

    public enum MessageHandlerBoxResult
    { 
        Ok,
        Cancel,
        Yes,
        No
    }

    public interface IMessageHandlerBoxView
    {
        MessageHandlerBoxResult Show(string messageXml);
    }
}
