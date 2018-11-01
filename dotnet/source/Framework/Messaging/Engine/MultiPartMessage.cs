using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Collections;
using System.Xml;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Base class for messages.
    /// </summary>
    public class MultiPartMessage : IDisposable
    {
        private PropertyCollection _metadataCollection;
        private Collection<MessagePart> _messagePartCollection;
        private readonly string _messageId;
        private Stream _dataStream;
        private PropertyCollection _propertyCollection;
        private bool _isDisposed;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MultiPartMessage"/> class.</para>
        /// </summary>
        public MultiPartMessage() : this((Stream)null)
        {
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MultiPartMessage"/> class.</para>
        /// </summary>
        /// <param name="messageType">
        /// The type of message.
        /// </param>
        public MultiPartMessage(string messageType) : this(messageType, null)
        {
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MultiPartMessage"/> class.</para>
        /// </summary>
        /// <param name="data">
        /// A <see cref="Stream"/> containing message data.
        /// </param>
        public MultiPartMessage(Stream data) : this("", data)
        {
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MultiPartMessage"/> class.</para>
        /// </summary>
        /// <param name="messageType">
        /// The type of message.
        /// </param>
        /// <param name="data">
        /// A <see cref="Stream"/> containing message data.
        /// </param>
        public MultiPartMessage(string messageType, Stream data) : this(messageType, data, new PropertyCollection())
        {
        }

        ~MultiPartMessage()
        {
            Dispose(false);
        }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="MultiPartMessage"/> class.</para>
        /// </summary>
        /// <param name="messageType">
        /// The type of message.
        /// </param>
        /// <param name="data">
        /// A <see cref="Stream"/> containing message data.
        /// </param>
        /// <param name="propertyCollection">
        /// <see cref="PropertyCollection"/> class for storing the message's properties.
        /// </param>
        protected MultiPartMessage(string messageType, Stream data, PropertyCollection propertyCollection)
        {
            _metadataCollection = new PropertyCollection(false);
            _metadataCollection.Write("MessageType", messageType);

            _messagePartCollection = new Collection<MessagePart>();

            this._propertyCollection = propertyCollection;
            this._dataStream = data;
                        
            _messageId = Guid.NewGuid().ToString();
        }
                
        /// <summary>
        /// Message meta data
        /// </summary>
        public PropertyCollection Metadata
        {
            get
            {
                return _metadataCollection;
            }
        }
    
        /// <summary>
        /// Returns the type of message.
        /// </summary>
        public string MessageType
        {
            get
            {
                return Metadata.Read("MessageType") as string;
            }
        }

        /// <summary>
        /// Returns the Id of the message.
        /// </summary>
        public string MessageId
        {
            get
            {
                return _messageId;
            }
        }

        /// <summary>
        /// Tries to determine the message type from the message's Data stream using XML.
        /// </summary>
        /// <returns>True if the message type was determined, otherwise false.</returns>
        /// <remarks>
        /// The message type is a concatenation of the XML namespace and the name of the first node.
        /// </remarks>
        public virtual bool SetTypeFromXml()
        {
            try
            {
                if (Data != null)
                {
                    Data.Seek(0, SeekOrigin.Begin);

                    XmlReader xml = XmlReader.Create(Data);

                    try
                    {
                        while (xml.NodeType != XmlNodeType.Element)
                        {
                            xml.Read();
                        }

                        if (xml.NamespaceURI.Length == 0)
                            _metadataCollection.Write("MessageType", xml.Name);
                        else
                            _metadataCollection.Write("MessageType", xml.NamespaceURI + "/" + xml.Name);
                    }
                    finally
                    {
                        xml.Close();
                    }

                    return true;
                }
                else
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (XmlException)
            {
                return false;
            }
        }

        internal void Lock()
        {
            Metadata.Lock();
            Properties.Lock();
            
            foreach (MessagePart part in _messagePartCollection)
            {
                part.Properties.Lock();
            }
        }

        /// <summary>
        /// Returns a collection of message parts.
        /// </summary>
        public Collection<MessagePart> Parts
        {
            get
            { 
                return _messagePartCollection;
            }
        }

        /// <summary>
        /// Returns the data stream of the message.
        /// </summary>
        public virtual Stream Data
        {
            get
            {
                return _dataStream;
            }
        }

        /// <summary>
        /// Converts the message's properties into XML.
        /// </summary>
        /// <returns>An XML mapping of the message's properties.</returns>
        public virtual string ToXmlString()
        {
            StringBuilder xmlString = new StringBuilder();
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineHandling = NewLineHandling.Entitize;
            
            XmlWriter xml = XmlWriter.Create(xmlString, settings);

            try
            {
                xml.WriteStartElement("message");
                xml.WriteStartElement("metadata");

                foreach (string propertyName in Metadata)
                {
                    xml.WriteStartElement("property");
                    xml.WriteAttributeString("name", propertyName);

                    string value = "";

                    if (Metadata[propertyName] != null)
                    {
                        value = Metadata[propertyName].ToString();
                    }

                    xml.WriteAttributeString("value", value);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();

                xml.WriteStartElement("properties");

                foreach (string propertyName in Properties)
                {
                    xml.WriteStartElement("property");
                    xml.WriteAttributeString("name", propertyName);

                    string value = "";

                    if (Properties[propertyName] != null)
                    {
                        value = Properties[propertyName].ToString();
                    }

                    xml.WriteAttributeString("value", value);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();

                xml.WriteStartElement("parts");

                foreach (MessagePart part in Parts)
                {
                    xml.WriteStartElement("part");
                    xml.WriteStartElement("properties");

                    foreach (string propertyName in part.Properties)
                    {
                        xml.WriteStartElement("property");
                        xml.WriteAttributeString("name", propertyName);
                        
                        string value = "";

                        if (part.Properties[propertyName] != null)
                        {
                            value = part.Properties[propertyName].ToString();
                        }

                        xml.WriteAttributeString("value", value);
                        xml.WriteEndElement();
                    }

                    xml.WriteEndElement();
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();

                xml.Flush();
            }
            finally
            {
                xml.Close();
            }

            return xmlString.ToString();
        }

        /// <summary>
        /// Returns a collection of message properties.
        /// </summary>
        public virtual PropertyCollection Properties
        {
            get
            {
                return _propertyCollection;            
            }
        }

        /// <summary>Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</summary>
        /// <returns>A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return MessageType;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                
                if (disposing)
                {
                    if (_dataStream != null)
                    {
                        _dataStream.Dispose();
                    }

                    foreach (MessagePart part in _messagePartCollection)
                    {
                        if (part.Data != null)
                        {
                            part.Data.Dispose();
                        }
                    }
                }
            }
        }
    }
}
