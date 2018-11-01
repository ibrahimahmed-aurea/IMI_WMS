using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Xml.Xsl;
using System.Security;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Transform;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Transforms a simple input XML document containing the values received by the voice terminal into a new XML document containing both values and property names.
    /// </summary>
    public class XslTransformComponent : XslTransformComponentBase
    {
        private string xsltPath;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="XslTransformComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public XslTransformComponent(PropertyCollection configuration)
            : base(configuration)
        {
            xsltPath = Configuration.ReadAsString("XsltPath");

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Pre-caching XSLT stylesheets...");    

            string[] stylesheetFiles = Directory.GetFiles(xsltPath, "*.xslt");

            foreach (string stylesheet in stylesheetFiles)
            {
                GetCachedTransform(stylesheet);
            }
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();

            DateTime startTime = DateTime.Now;
            
            string stylesheet = null;

            try
            {
                stylesheet = Path.Combine(xsltPath, msg.Properties.ReadAsString("MessageType") + ".xslt");
            }
            catch (ArgumentException ex)
            {
                throw new ComponentException("Invalid characters in MessageType.", ex);
            }

            MultiPartMessage transformedMsg = Transform(msg, stylesheet);

            PromoteProperties(transformedMsg);

            resultCollection.Add(transformedMsg);

            TimeSpan transformationTime = DateTime.Now - startTime;

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Message transformed in: {0}s {1}ms.", transformationTime.Seconds, transformationTime.Milliseconds);    

            return resultCollection;
        }

        private void PromoteProperties(MultiPartMessage msg)
        {
            try
            {
                msg.Data.Seek(0, SeekOrigin.Begin);

                XmlReader reader = XmlReader.Create(msg.Data);
                string temp = reader.ReadOuterXml();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        string dataType = reader.GetAttribute("datatype");

                        string data = reader.ReadString();

                        switch (dataType)
                        {
                            case "string":

                                msg.Properties.Write(reader.Name, data);
                                break;

                            case "integer":

                                int tempInt;

                                int.TryParse(data, out tempInt);
                                msg.Properties.Write(reader.Name, tempInt);

                                break;

                            case "double":

                                double tempDouble;

                                double.TryParse(data, out tempDouble);
                                msg.Properties.Write(reader.Name, tempDouble);

                                break;

                            case "dateTime":

                                DateTime tempDateTime;

                                DateTime.TryParse(data, out tempDateTime);
                                msg.Properties.Write(reader.Name, tempDateTime);

                                break;
                        }
                    }
                }
            }
            catch (SecurityException ex)
            {
                throw new ComponentException("Failed to promote XML data to message properties.", ex);
            }
            catch (XmlException ex)
            {
                throw new ComponentException("Failed to promote XML data to message properties.", ex);
            }
            catch (IOException ex)
            {
                throw new ComponentException("Failed to promote XML data to message properties.", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new ComponentException("Failed to promote XML data to message properties.", ex);
            }
            catch (ObjectDisposedException ex)
            {
                throw new ComponentException("Failed to promote XML data to message properties.", ex);
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
            {
                return true;
            }

            return false;
        }

    }
}
