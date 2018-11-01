using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server.Subscribers
{
    [SessionPolicy(SessionPolicy.None)]
    public class CreateSessionSubscriber : MessageSubscriber<CreateSessionRequest, CreateSessionResponse>
    {
        public override void Invoke(CreateSessionRequest request)
        {
            try
            {
                ApplicationAdapter applicationAdapter = MessageEngine.Instance.AdapterProxy.GetAdapterById("app") as ApplicationAdapter;

                if (MessageEngine.Instance.AdapterProxy.ResolveUriToEndPoint(new Uri("app://" + request.SessionId)) == null)
                {
                    lock (Session.SyncLock)
                    {
                        Session.ClientIP = ((Uri)SourceMessage.Metadata.Read("ReceiveUri")).Host;
                        Session.TerminalId = request.TerminalId;
                        Session.ClientPlatform = request.ClientPlatform;
                        Session.ClientVersion = request.ClientVersion;
                        ApplicationAdapterEndPoint endPoint = applicationAdapter.StartApplication(request.ApplicationName, Session.Id);
                        Session.ApplicationEndPoint = endPoint;
                    }
                }

                CreateSessionResponse response = new CreateSessionResponse();

                TransmitResponseMessage(response);
            }
            catch
            {
                SessionManager.Instance.DestroySession(request.SessionId);
                throw;
            }
        }
    }
}
