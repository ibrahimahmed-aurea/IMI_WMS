using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Diagnostics;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Assembles a message into a stream of bytes to be sent to the voice terminal.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public class MessageToStreamAssembler : PipelineComponent
    {

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MessageToStreamAssembler"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public MessageToStreamAssembler(PropertyCollection configuration)
            : base(configuration)
        { 
        
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();

            if (msg.MessageType == "http://www.im.se/wms/voice/vocollect/voicedirect/ack")
            {
                resultCollection.Add(msg);
            }
            else
            {
                if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                    MessageEngine.Instance.Tracing.TraceData(TraceEventType.Verbose, 0, msg.ToXmlString());

                MultiPartMessage streamMsg = new MultiPartMessage(new MemoryStream());
                StreamWriter writer = new StreamWriter(streamMsg.Data, Encoding.GetEncoding(Configuration.ReadAsString("CodePageName")));
                
                int i = 0;

                foreach (string propertyName in msg.Properties)
                {
                    if (i > 0)
                        writer.Write(",");

                    writer.Write("\"");

                    string value = msg.Properties.Read(propertyName).ToString();
                    
                    if (value.Contains("\""))
                        value = value.Replace('"', ' ');

                    //Write error messages in lower case
                    if (propertyName == "Message")
                        writer.Write(value.ToLower());
                    else
                        writer.Write(value.ToString());

                    writer.Write("\"");

                    i++;
                }

                foreach (MessagePart part in msg.Parts)
                {
                    i = 0;

                    foreach (string propertyName in part.Properties)
                    {
                        if (i > 0)
                            writer.Write(",");

                        writer.Write("\"");

                        string value = part.Properties.Read(propertyName).ToString();

                        if (value.Contains("\""))
                            value = value.Replace('"', ' ');

                        //Write error messages in lower case
                        if (propertyName == "Message")
                            writer.Write(value.ToLower());
                        else
                            writer.Write(value.ToString());

                        writer.Write("\"");

                        i++;
                    }

                    writer.Write("\r\n");
                }

                writer.Write("\r\n\r\n");

                writer.Flush();

                resultCollection.Add(streamMsg);
            }

            return resultCollection;

        }

        /// <summary>
        /// Checks if the component supports processing of a specified message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message is supported, othwerwise false.</returns>
        public override bool Supports(MultiPartMessage msg)
        {
            if (msg.MessageType.StartsWith("http://www.im.se/wms/voice/vocollect/voicedirect"))
                return true;
            else
                return false;
        }
       
    }
}
