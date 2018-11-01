using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Wms.Mobile.Server.Adapter
{
    public class ApplicationTerminatedException : AdapterException
    {
        public ApplicationTerminatedException(string message)
            : base(message)
        { }
    }
}
