using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using System.IO;
using Imi.Framework.Shared.Diagnostics;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Sets the trace context for the current thread, directing the trace output to the correct destination.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public class TraceContextComponent : PipelineComponent
    {
        private readonly ulong maxLogSize = 0;
        private readonly string logPath;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TraceContextComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public TraceContextComponent(PropertyCollection configuration)
            : base(configuration)
        {
            maxLogSize = (ulong)configuration.Read("MaxLogSize");
            logPath = (string)configuration.Read("LogPath");
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();
            resultCollection.Add(msg);
                      
            if (MessageEngine.Instance.Tracing.Switch.Level != SourceLevels.Off)
            {
                ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

                if (contextListener != null)
                {
                    string sessionId = msg.Properties.ReadAsString("SerialNumber");

                    if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                        MessageEngine.Instance.Tracing.TraceData(TraceEventType.Verbose, 0, string.Format("TraceContextComponent: Switching context to: \"{0}\"", sessionId));
                                        
                    if (!contextListener.IsContextInitialized(sessionId))
                    {
                        contextListener.InitializeContext(sessionId,
                            new RollingFileTraceListener(Path.Combine(logPath, sessionId + ".log"), maxLogSize));
                    }

                   contextListener.Context = sessionId;
                }
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
            if (msg.MessageType == "http://www.im.se/wms/voice/vocollect/xml_param_base")
                return true;
            else
                return false;
        }
    }
}
