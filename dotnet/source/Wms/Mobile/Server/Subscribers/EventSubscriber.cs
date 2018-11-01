using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server.Subscribers
{
    [SessionPolicy(SessionPolicy.Required)]
    public class EventSubscriber : MessageSubscriber<EventRequest, EventResponse>
    {
        public override void Invoke(EventRequest request)
        {
            bool outOfSync = false;

            lock (Session.SyncLock)
            {
                if (request.HashCode != Session.StateResponse.HashCode)
                {
                    outOfSync = true;
                }
            }

            if (!outOfSync)
            {
                using (MultiPartMessage eventMessage = new MultiPartMessage("", new MemoryStream(4096)))
                {
                    eventMessage.Metadata.Write("SendUri", new Uri("app://" + request.SessionId));

                    StreamWriter writer = new StreamWriter(eventMessage.Data, new UnicodeEncoding(false, false));
                    _requestSerializer.Serialize(writer, request);
                    
                    MessageEngine.Instance.TransmitMessage(eventMessage);
                }
            }
           
            TransmitResponseMessage(new EventResponse());
        }
    }
}
