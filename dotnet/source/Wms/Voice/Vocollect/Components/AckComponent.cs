using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Imi.Framework.Integration.Engine;
using Imi.Framework.Integration.Adapter.Net.Sockets;

namespace Imi.Wms.Voice.Vocollect.Components
{
    [Persistence(PersistenceMode.Adapter)]
    public class AckComponent : BaseComponent
    {
        byte ackByte;

        public AckComponent(BasePropertyCollection configuration)
            : base(configuration)
        {
            this.ackByte = (byte)Configuration.Read("ODRConfirmationByte");
        }

        public override Collection<BaseMessage> Invoke(BaseMessage msg)
        {
            try
            {
                Collection<BaseMessage> resultCollection = new Collection<BaseMessage>();
                resultCollection.Add(msg);

                if (msg.Properties.ReadAsString("MessageType").Contains("ODR"))
                {
                    BaseMessage ackMsg = new BaseMessage("http://www.im.se/wms/voice/vocollect/ack", new MemoryStream());

                    ackMsg.Data.WriteByte(ackByte);
                    ackMsg.Metadata.Write("SendUri", msg.Metadata.Read("ReceiveUri"));

                    TcpAdapter adapter = (TcpAdapter)MessageEngine.Instance.AdapterProxy.GetAdapterById("tcp");
                    
                    //Transmit raw byte message
                    adapter.TransmitMessage(ackMsg);
                }
                
                return resultCollection;
            }
            catch (MessageEngineException ex)
            {
                throw new ComponentException("Failed to acknowledge message: \"" + msg.MessageType + "\"", ex);
            }

        }

        public override bool Supports(BaseMessage msg)
        {
            if (msg.MessageType == "http://www.im.se/wms/voice/vocollect/xml_param_base")
                return true;
            else
                return false;
        }

    }
}
