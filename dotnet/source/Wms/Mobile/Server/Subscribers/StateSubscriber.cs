using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Wms.Mobile.Server.Interface;
using Imi.Wms.Mobile.Server.Configuration;

namespace Imi.Wms.Mobile.Server.Subscribers
{
    [SessionPolicy(SessionPolicy.Required)]
    public class StateSubscriber : MessageSubscriber<StateRequest, StateResponse>
    {
        ServerSection _config;

        public StateSubscriber()
        {
            _config = ConfigurationManager.GetSection(ServerSection.SectionKey) as ServerSection;
        }
                
        public override void Invoke(StateRequest request)
        {
            bool wait = false;

            lock (Session.SyncLock)
            {
                if (Session.StateResponse == null || request.HashCode == Session.StateResponse.HashCode)
                {
                    Session.AbortEvent.Reset();
                    Session.StateChangedEvent.Reset();
                    wait = true;
                }
            }

            if (wait)
            {
                int result = EventWaitHandle.WaitAny(new EventWaitHandle[] { Session.StateChangedEvent, Session.AbortEvent }, _config.StateTimeout * 1000);

                if (result != 0)
                {
                    CheckAndThrowException();

                    if (result == WaitHandle.WaitTimeout)
                    {
                        StateResponse emptyStateResponse = new StateResponse();
                        TransmitResponseMessage(emptyStateResponse);
                    }

                    return;
                }
            }

            lock (Session.SyncLock)
            {
                TransmitResponseMessage(Session.StateResponse);
            }
        }
    }
}
