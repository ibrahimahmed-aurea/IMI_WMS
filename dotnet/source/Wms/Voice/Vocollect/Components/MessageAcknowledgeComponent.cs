using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Net.Sockets;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Component responsible for message akcnowledgement.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public class MessageAcknowledgeComponent : PipelineComponent
    {
        readonly byte ackByte;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageAcknowledgeComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public MessageAcknowledgeComponent(PropertyCollection configuration)
            : base(configuration)
        {
            this.ackByte = (byte)Configuration.Read("ODRConfirmationByte");
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            try
            {
                Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();
                resultCollection.Add(msg);

                if (msg.Properties.ReadAsString("MessageType").Contains("ODR"))
                {
                    msg.Properties.Write("IsOutputDataRecord", true);

                    MultiPartMessage ackMsg = new MultiPartMessage("http://www.im.se/wms/voice/vocollect/ack", new MemoryStream());

                    ackMsg.Data.WriteByte(ackByte);
                    ackMsg.Metadata.Write("SendUri", msg.Metadata.Read("ReceiveUri"));

                    TcpAdapter adapter = (TcpAdapter)MessageEngine.Instance.AdapterProxy.GetAdapterById("tcp");
                    
                    //Transmit raw byte data
                    adapter.TransmitMessage(ackMsg);
                }
                else
                    msg.Properties.Write("IsOutputDataRecord", false);
                
                return resultCollection;
            }
            catch (MessageEngineException ex)
            {
                throw new ComponentException("Failed to acknowledge message: \"" + msg.MessageType + "\"", ex);
            }

        }

        /// <summary>
        /// Checks if the component supports processing of a specified message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message is supported, othwerwise false.</returns>
        public override bool Supports(MultiPartMessage msg)
        {
            if (msg.MessageType == "http://www.im.se/wms/voice/vocollect/xml_param_base")
                return true;
            else
                return false;
        }

    }
}
