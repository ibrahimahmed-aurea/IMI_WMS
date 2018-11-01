using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Net.Sockets;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Adapter.Net.Sockets;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Component for disassembling of a Vocollect ASCII message into XML.
    /// </summary>
    [Persistence(PersistenceMode.EndPoint)]
    public class StreamToXmlDisassembler : PipelineComponent
    {

        private StringBuilder buffer;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="StreamToXmlDisassembler"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public StreamToXmlDisassembler(PropertyCollection configuration)
            : base(configuration)
        {
            buffer = new StringBuilder();
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();

            Encoding encoding = Encoding.GetEncoding(Configuration.ReadAsString("CodePageName")); 

            buffer.Append(encoding.GetString(((MemoryStream)msg.Data).ToArray()));

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Stream buffer content: \"{0}\"", buffer.ToString().Trim());

            string data = null;
            
            while ((data = ReadBuffer(buffer)) != null)
            {
                MultiPartMessage xmlMessage = CreateMessage(data);
                
                if (xmlMessage != null)
                    resultCollection.Add(xmlMessage);
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
            if (msg.Metadata.ReadAsString("ReceiveAdapterId") == "tcp")
                return true;
            else
                return false;
        }

        private string ReadBuffer(StringBuilder buffer)
        {
            try
            {
                int idx = -1;

                //Find end of message
                for (int i = 0; i < buffer.Length - 2; i++)
                {
                    if ((buffer[i] == '\r')
                        && (buffer[i + 1] == '\n')
                        && (buffer[i + 2] == '\n'))
                    {
                        idx = i;
                        break;
                    }
                }

                if (idx > -1)
                {
                    string data = buffer.ToString(0, idx);
                    buffer.Remove(0, idx + 3);

                    return data;
                }

                return null;
            }
            catch (IndexOutOfRangeException ex)
            {
                //Clear buffer
                buffer.Remove(0, buffer.Length);

                throw new ComponentException("Error reading message buffer.", ex);
            }
            catch (ArgumentException ex)
            {
                //Clear buffer
                buffer.Remove(0, buffer.Length); 
 
                throw new ComponentException("Error reading message buffer.", ex);
            }
        }
                
        private MultiPartMessage CreateMessage(string data)
        {
            //Set culture to en-US for correct parsing
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            try
            {
                string[] sep = { "('", "', '", "','", "')" };
                string[] paramArray = data.Split(sep, StringSplitOptions.None);

                if (paramArray.Length > 1)
                {
                    MultiPartMessage msg = new MultiPartMessage("http://www.im.se/wms/voice/vocollect/xml_param_base", new MemoryStream());

                    string messageType = data.Substring(0, 1).ToUpper() + data.Substring(1, data.IndexOf(" (") - 1).Trim();

                    /* Get additional sequence number from time stamp */
                    int sequence = 0;

                    int sequencePosition = paramArray[1].IndexOf('.');

                    if ((sequencePosition > -1) && (sequencePosition < paramArray[1].Length - 1))
                        int.TryParse(paramArray[1].Substring(sequencePosition + 1), out sequence);

                    msg.Properties.Write("Sequence", sequence);

                    /* Convert date time to format: "2000-08-17T16:32:32" */
                    DateTime timeStamp = DateTime.ParseExact(paramArray[1].Substring(0, 17), "MM-dd-yy HH:mm:ss", null);
                    paramArray[1] = timeStamp.ToString("s");
                                        
                    msg.Properties.Write("MessageType", messageType);
                    msg.Properties.Write("TimeStamp", timeStamp);
                    msg.Properties.Write("SerialNumber", paramArray[2]);
                    
                    /* Calculate hash code */
                    int hashCode = data.Substring(data.IndexOf("','")).GetHashCode() ^ messageType.GetHashCode();
                    
                    msg.Properties.Write("HashCode", hashCode);

                    /* Format message as XML */
                    XmlWriter xml = XmlWriter.Create(msg.Data);
                    
                    try
                    {
                        xml.WriteStartElement("message");
                        xml.WriteAttributeString("type", messageType);

                        xml.WriteStartElement("params");

                        for (int i = 1; i < paramArray.Length; i++)
                        {
                            xml.WriteStartElement("param");
                            xml.WriteAttributeString("index", (i - 1).ToString());
                            xml.WriteString(paramArray[i]);
                            xml.WriteEndElement();
                        }

                        xml.WriteEndElement();

                        xml.WriteEndElement();
                    }
                    finally
                    {
                        xml.Close();
                    }
                                        
                    return msg;
                }

                return null;
            }
            catch (ArgumentException ex)
            {
                throw new ComponentException("Message disassembly failed.", ex);
            }
            catch (FormatException ex)
            {
                throw new ComponentException("Message disassembly failed.", ex);
            }
            catch (XmlException ex)
            {
                throw new ComponentException("Message disassembly failed.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ComponentException("Message disassembly failed.", ex);
            }
        }
    }
}
