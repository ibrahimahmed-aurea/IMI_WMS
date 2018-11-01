using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter;
using Imi.Framework.Messaging.Adapter.Net.Sockets;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Component for detecting transmission errors.
    /// </summary>
    [Persistence(PersistenceMode.Adapter)]
    public class TransmissionControlComponent : PipelineComponent
    {
        private Dictionary<TcpAdapterEndPoint, VocollectSession> endPointDictionary;
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="TransmissionControlComponent"/> class.</para>
        /// </summary>
        /// <param name="configuration">
        /// Configuration properties.
        /// </param>
        public TransmissionControlComponent(PropertyCollection configuration)
            : base(configuration)
        {
            endPointDictionary = new Dictionary<TcpAdapterEndPoint, VocollectSession>();

            TcpAdapter tcpAdapter = (TcpAdapter)MessageEngine.Instance.AdapterProxy.GetAdapterById("tcp");

            /* This event will call session.ReleaseLock */
            tcpAdapter.EndPointDestroyed += new EventHandler<AdapterEndPointEventArgs>(EndPointDestroyedEventHandler); ;
        }
                

        private void EndPointDestroyedEventHandler(object sender, AdapterEndPointEventArgs e)
        {
            TcpAdapterEndPoint endPoint = (TcpAdapterEndPoint)e.EndPoint;
            
            VocollectSession session = null;

            lock (endPointDictionary)
            {
                if (endPointDictionary.ContainsKey(endPoint))
                {
                    session = endPointDictionary[endPoint];
                    endPointDictionary.Remove(endPoint);
                }
            }

            if (session != null)
            {
                try
                {
                    if (endPoint.Exception != null)
                    {
                        session.ReleaseLock(false);
                                                
                        /* Do not log ObjectDisposedException since this indicates we got a timeout */
                        if (!(endPoint.Exception is ObjectDisposedException))
                        {
                            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                                MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, endPoint.Exception);
                        }
                    }
                    else
                    {
                        session.ReleaseLock(true);
                    }
                }
                catch (ObjectDisposedException)
                { 
                    //Session has been disposed
                }
            }
        }

        /// <summary>
        /// Called by the pipeline to invoke the component.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A collection of messages produced by this component.</returns>
        public override Collection<MultiPartMessage> Invoke(MultiPartMessage msg)
        {
            string serialNumber = msg.Properties.ReadAsString("SerialNumber");

            if (!CanProcess(msg))
            {
                ContextTraceListener contextListener = ((ContextTraceListener)MessageEngine.Instance.Tracing.Listeners["ContextTraceListener"]);

                if (contextListener != null)
                    contextListener.ResetContext();

                return null;
            }
                        
            Collection<MultiPartMessage> resultCollection = new Collection<MultiPartMessage>();
            resultCollection.Add(msg);

            return resultCollection;
        }

        private bool CanProcess(MultiPartMessage msg)
        {
            VocollectSession session = SessionManager.Instance[msg.Properties.ReadAsString("SerialNumber")];

            /* Safe guard to ensure reset of session after new logon */
            if ((session != null) && (msg.Properties.ReadAsString("MessageType") == "PrTaskLUTCoreConfiguration"))
            {
                SessionManager.Instance.DestroySession(msg.Properties.ReadAsString("SerialNumber"));
                session.Dispose();
                session = null;
            }

            TcpAdapterEndPoint endPoint = MessageEngine.Instance.AdapterProxy.ResolveUriToEndPoint((Uri)msg.Metadata.Read("ReceiveUri")) as TcpAdapterEndPoint;

            if ((session != null) && (endPoint != null))
            {
                try
                {
                    if (!session.AcquireLock(Configuration.ReadAsInt("PendingOperationsTimeout")))
                    {
                        /* Thread timed out or aborted due to a newer request */
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Warning) == SourceLevels.Warning)
                            MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Warning, 0, "Thread timed out waiting for a pending operation to complete.");

                        return false;
                    }
                }
                catch (ObjectDisposedException)
                {
                   return false;
                }
                                
                lock (endPointDictionary)
                {
                    endPointDictionary.Add(endPoint, session);
                }
                                               
                if (IsDuplicate(msg, session, endPoint))
                {
                    /* Message is duplicate, retransmit last response */
                    if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Warning) == SourceLevels.Warning)
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Warning, 0, "Message timeout detected, retransmitting last response.");
                   
                    MultiPartMessage retransmitMsg = session.LastResponseMessage;
                    retransmitMsg.Metadata.Write("Loopback", false);
                    retransmitMsg.Metadata.Write("SendUri", msg.Metadata.Read("ReceiveUri"));

                    MessageEngine.Instance.TransmitMessage(retransmitMsg);
    
                    return false;
                }
                else
                {
                    /* Message is valid, continue processing */
                    session.LastRequestMessage = msg;
                    session.LastResponseMessage = null;
                }
            }

            return true;
        }
                
        private bool IsDuplicate(MultiPartMessage msg, VocollectSession session, TcpAdapterEndPoint endPoint)
        {
            int hashCode = msg.Properties.ReadAsInt("HashCode");
            int sequence = msg.Properties.ReadAsInt("Sequence");

            DateTime timeStamp = (DateTime)msg.Properties.Read("TimeStamp");
            
            if (msg.Properties.ReadAsString("MessageType").Contains("ODR"))
            {
                if ((timeStamp <= session.LastTimeStamp)
                    && (session.LastRequestMessage != null)
                    && (session.LastResponseMessage != null)
                    && (session.LastRequestMessage.Properties.ReadAsInt("HashCode") == hashCode)
                    && (sequence <= session.LastRequestMessage.Properties.ReadAsInt("Sequence")))
                {
                    /* Duplicate ODR message */
                    return true;
                }

                /* Message is valid update timestamp */
                session.LastTimeStamp = timeStamp;
            }
            else
            {
                /* Removed due to time synchronization issues, timestamp from terminal not accurate enough to be useful */
                //((TcpAdapter)endPoint.Adapter).SetEndPointTimeout(endPoint, timeStamp.AddMilliseconds(Configuration.ReadAsInt("PendingOperationsTimeout")));

                /* Always update timestamp for LUT messages */
                session.LastTimeStamp = timeStamp;

                if ((!session.LastOperationSuccessful)
                    && (session.LastRequestMessage != null)
                    && (session.LastResponseMessage != null)
                    && (session.LastRequestMessage.Properties.ReadAsInt("HashCode") == hashCode))
                {
                    /* Duplicate LUT message */
                    return true;
                }
            }

            return false;
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
