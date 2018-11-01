using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class ShellNotification
    {
        public string Message { get; private set; }
        public Action<ShellNotification> NotificationCallback { get; private set; }

        public ShellNotification(string message, Action<object> notificationCallback)
        {
            Message = message;
            NotificationCallback = notificationCallback;
        }
    }
}
