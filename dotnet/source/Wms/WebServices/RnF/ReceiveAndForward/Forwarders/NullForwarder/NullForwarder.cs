using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
    public class NullForwarder : IForwarder
    {
        public void CreateContext(string userdata, string objectName)
        {
        }

        public void ReleaseContext()
        {
        }

        public void Forward(IInterfaceClass data)
        {
            // send this to null
        }

    }
}
