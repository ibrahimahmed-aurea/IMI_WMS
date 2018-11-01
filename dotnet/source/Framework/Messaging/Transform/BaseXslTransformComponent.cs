using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Messaging.Engine;

namespace Imi.Framework.Messaging.Transform
{
    /// <summary>
    /// Pipeline component for XSL transformations.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public abstract class BaseXslTransformComponent : PipelineComponent
    {

        private Dictionary<string, XslCompiledTransform> xsltDictionary;
        private ReaderWriterLock syncLock;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="BaseXslTransformComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        protected BaseXslTransformComponent(PropertyBag configuration)
            : base(configuration)
        {
            xsltDictionary = new Dictionary<string, XslCompiledTransform>();
            syncLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            return null;
        }

        /// <summary>
        /// Transforms the specified message using the specified stylesheet.
        /// </summary>
        /// <param name="msg">The message to transform.</param>
        /// <param name="stylesheetUri">The stylesheet containing the transformation rules.</param>
        /// <returns>A <see cref="MultiPartMessage"/> containing the transformed XML.</returns>
        protected virtual MultiPartMessage Transform(MultiPartMessage msg, string stylesheetUri)
        {
            try
            {
                MultiPartMessage transformedMsg = new MultiPartMessage(new MemoryStream());
                msg.Data.Seek(0, SeekOrigin.Begin);
                XmlReader xml = XmlReader.Create(msg.Data);

                //Get compiled XSL transform from uri or cache
                XslCompiledTransform xslt = GetCachedTransform(stylesheetUri);

                //Run transformation
                xslt.Transform(xml, null, transformedMsg.Data);

                transformedMsg.SetTypeFromXml();

                return transformedMsg;
            }
            catch (ArgumentException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
            catch (XsltException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
            catch (XmlException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
            catch (IOException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
            catch (FormatException ex)
            {
                throw new ComponentException("XSL transformation failed.", ex);
            }
        }

        /// <summary>
        /// Returns and a compiled stylesheet.
        /// </summary>
        /// <param name="stylesheetUri">The stylesheet to compile and cache.</param>
        /// <returns>A <see cref="XslCompiledTransform"/> object.</returns>
        protected virtual XslCompiledTransform GetCachedTransform(string stylesheetUri)
        {
            XslCompiledTransform xslt = null;
                        
            syncLock.AcquireReaderLock(Timeout.Infinite);

            try
            {
                if (xsltDictionary.ContainsKey(stylesheetUri))
                    xslt = xsltDictionary[stylesheetUri];
            }
            finally
            {
                syncLock.ReleaseReaderLock();
            }

            if (xslt == null)
            {
                syncLock.AcquireWriterLock(Timeout.Infinite);

                try
                {
                    if (xsltDictionary.ContainsKey(stylesheetUri))
                    {
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Stylesheet: \"{0}\" already cached by another thread.", stylesheetUri);    

                        xslt = xsltDictionary[stylesheetUri];
                    }
                    else
                    {
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Compiling and caching stylesheet: \"{0}\"...", stylesheetUri);    

                        xslt = new XslCompiledTransform();

                        //Load stylesheet from uri
                        xslt.Load(stylesheetUri);

                        xsltDictionary[stylesheetUri] = xslt;
                    }
                }
                finally
                {
                    syncLock.ReleaseWriterLock();
                }
            }

            return xslt;
           
        }

        /// <summary>
        /// Checks if the component supports processing of a specified message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message is supported, othwerwise false.</returns>
        public override bool Supports(MultiPartMessage msg)
        {
            return true;
        }

      
    }
}
