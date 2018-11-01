using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Xml;
using System.Security.Cryptography;
using Imi.Framework.Messaging.Engine;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server.Subscribers
{
    public class ApplicationSubscriber : SubscriberBase
    {
        private XmlSerializer _serializer;

        public ApplicationSubscriber()
        { 
             _serializer = new XmlSerializer(typeof(StateResponse));
        }

        public override void Invoke(MultiPartMessage msg)
        {
            string sessionId = ((Uri)msg.Metadata.Read("ReceiveUri")).Host;

            ClientSession session = SessionManager.Instance[sessionId];

            msg.Data.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(msg.Data, Encoding.Unicode);

            StateResponse stateResponse = (StateResponse)_serializer.Deserialize(reader);
            
            msg.Data.Seek(0, SeekOrigin.Begin);
                        
            using (XmlReader xmlReader = XmlReader.Create(msg.Data))
            {
                if (xmlReader.ReadToFollowing("StateResponse"))
                {
                    string xml = xmlReader.ReadInnerXml();
                    byte[] hash = MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(xml + DateTime.Now.Ticks.ToString())); //Added datetime to hash calculation to prevent loss of updates of identical layouts.

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }

                    stateResponse.HashCode = sb.ToString();
                }
            }
                        
            lock (session.SyncLock)
            {
                if (session.StateResponse != null && session.StateResponse.HashCode == stateResponse.HashCode)
                {
                    return;
                }

                session.StateResponse = stateResponse;
                session.StateResponse.TraceLevel = session.ClientTraceLevel.ToString();
            }
                        
            session.StateChangedEvent.Set();
        }
    }
}
