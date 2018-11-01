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
using Imi.Wms.Mobile.Server;

namespace Imi.Wms.Mobile.Server.Components
{
    [Persistence(PersistenceMode.EndPoint)]
    public class StreamToMessageDisassembler : PipelineComponent
    {

        private MemoryStream _buffer;
        private const byte EndOfFileLength = 4;
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="StreamToMessageDisassembler"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public StreamToMessageDisassembler(PropertyCollection configuration)
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

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
            {
                msg.Data.Seek(0, SeekOrigin.Begin);
                var temp = new StringBuilder();
                temp.Append(Encoding.UTF8.GetString(((MemoryStream)msg.Data).ToArray()));
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Stream buffer content: \"{0}\"", temp.ToString().Trim());
            }

            if (_buffer == null)
            {
                _buffer = new MemoryStream(8192);
            }

            msg.Data.Seek(0, SeekOrigin.Begin);
            _buffer.Seek(0, SeekOrigin.End);
            msg.Data.CopyTo(_buffer);
            _buffer.Seek(0, SeekOrigin.Begin);

            int b = 0;
            int count = 0;
            
            while ((b = _buffer.ReadByte()) > -1)
            {
                if (b == 0)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }

                if (count == EndOfFileLength)
                {
                    resultCollection.Add(CreateMessage(_buffer));
                    _buffer = null;
                    break;
                }
            }
                        
            return resultCollection;
        }

        private static MultiPartMessage CreateMessage(Stream data)
        {
            var message = new MultiPartMessage(data);

            message.Data.Seek(0, SeekOrigin.Begin);

            XmlReader xml = XmlReader.Create(message.Data);
            
            while (xml.NodeType != XmlNodeType.Element)
            {
                xml.Read();
            }

            message.Metadata.Write("MessageType", "http://www.im.se/wms/mobile/" + xml.Name);
                        
            if (xml.MoveToAttribute("SessionId"))
            {
                message.Metadata.Write("SessionId", xml.ReadContentAsString());
            }

            if (xml.MoveToAttribute("Sequence"))
            {
                message.Metadata.Write("Sequence", xml.ReadContentAsInt());
            }
            
            return message;
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
    }
}
