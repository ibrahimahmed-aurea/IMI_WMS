using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
	public interface IForwarder
	{
        void CreateContext(string userdata, string objectName);
        void ReleaseContext();
        void Forward(IInterfaceClass data);
	}
}
