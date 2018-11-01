using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.IO;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Framework.Messaging.Adapter.Net.Sockets;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Shared.Diagnostics;
using Imi.Wms.Voice.Vocollect.Configuration;
using Imi.Framework.Messaging.Adapter;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{    
    /// <summary>
    /// Base class for handling messages received from the voice terminal.
    /// </summary>
    [SessionPolicy(SessionPolicy.Required)]
    public abstract class VocollectSubscriber : SubscriberBase
    {
        /// <summary>
        /// Error code 0. No error occured.
        /// </summary>
        protected const int VocollectErrorCodeNoError = 0;
        /// <summary>
        /// Error code 98. A critical error occured while processing the message, user must sign off.
        /// </summary>
        protected const int VocollectErrorCodeCritialError = 98;
        /// <summary>
        /// Error code 99. Informational error, user may continue.
        /// </summary>
        protected const int VocollectErrorCodeInformationalError = 99;
        /// <summary>
        /// Error code 100. Alarm returned from WMS.
        /// </summary>
        protected const int WarehouseAlarm = 100;

        private readonly SessionPolicy sessionPolicy;
        private readonly byte ackByte;
        
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectSubscriber"/> class.</para>
        /// </summary>
        public VocollectSubscriber()
        {
            SessionPolicyAttribute attribute = (SessionPolicyAttribute)this.GetType().GetCustomAttributes(typeof(SessionPolicyAttribute), true)[0];
            sessionPolicy = attribute.Policy;
            
            VocollectSection configSection = ConfigurationManager.GetSection(VocollectSection.SectionKey) as VocollectSection;
            ackByte = System.Convert.ToByte(configSection.ODRConfirmationByte, 16);
        }

        /// <summary>
        /// Internal invokation method.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        protected override void InternalInvoke(MultiPartMessage msg)
        {
            VocollectSession session = GetCurrentSession(msg);

            try
            {
                Invoke(msg, session);
            }
            finally
            {
                if (msg.MessageType.Contains("ODR"))
                {
                    /* Auto acknowledge ODR messages */
                    MultiPartMessage ackMsg = new MultiPartMessage("http://www.im.se/wms/voice/vocollect/voicedirect/ack", new MemoryStream());

                    ackMsg.Data.WriteByte(ackByte);
                    ackMsg.Metadata.Write("SendUri", msg.Metadata.Read("ReceiveUri"));

                    TransmitResponseMessage(ackMsg, session);

                }
            }
        }

        /// <summary>
        /// Returns the critical error message text defined in the app.config file.
        /// </summary>
        /// <param name="msg">Message for session reference.</param>
        /// <returns>Critical error message text from app.config file.</returns>
        protected string GetCriticalErrorMessageText(MultiPartMessage msg)
        {
            VocollectSection configSection = ConfigurationManager.GetSection(VocollectSection.SectionKey) as VocollectSection;
            
            MessageElement message = configSection.MessageCollection["CriticalError"];

            return string.Format(message.Text, this.GetType().Name);
        }

        /// <summary>
        /// Invokes the subscriber.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        /// <param name="session">The current <see cref="VocollectSession"/> object.</param>
        public abstract void Invoke(MultiPartMessage msg, VocollectSession session);


        /// <summary>
        /// Invokes the subscriber for processing of the message.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        public override void Invoke(MultiPartMessage msg)
        { 
        
        }

        /// <summary>
        /// Creates a response message ready to be transmitted back to the terminal.
        /// </summary>
        /// <param name="sourceMsg">The source message containing the receive Uri.</param>
        /// <returns>A <see cref="VocollectMessage"/> object.</returns>
        protected static MultiPartMessage CreateResponseMessage(MultiPartMessage sourceMsg)
        {
            MultiPartMessage msg = new VocollectMessage();
            
            sourceMsg.Metadata.Copy("ReceiveUri", msg.Metadata);
            msg.Metadata.Write("Loopback", true);
                                                
            return msg;
        }
        
        protected static void TransmitResponseMessage(MultiPartMessage msg, VocollectSession session)
        {
            if (session != null)
                session.LastResponseMessage = msg;

            try
            {
                MessageEngine.Instance.TransmitMessage(msg);
            }
            catch (AdapterException)
            {
                //Ignore any transmission errors, these will be logged by the TransmissionControlComponent
            }   
        }

        /// <summary>
        /// Creates a request message ready to be transmitted to the WMS.
        /// </summary>
        /// <param name="packageName">Package name.</param>
        /// <param name="procedureName">Name of stored procedure to call.</param>
        /// <param name="session">The current <see cref="VocollectSession"/> object.</param>
        /// <returns>A <see cref="MultiPartMessage"/> that can be sent to the WMS.</returns>
        protected static MultiPartMessage CreateRequestMessage(string packageName, string procedureName, VocollectSession session)
        {
            MultiPartMessage msg = new VocollectMessage();

            VocollectSection configSection = ConfigurationManager.GetSection(VocollectSection.SectionKey) as VocollectSection;
            msg.Metadata.Write("SendUri", new Uri(string.Format("warehouse://{2}/{0}/{1}", packageName, procedureName, configSection.Database)));
            
            if (session != null)
                msg.Metadata.Write("LanguageCode", session.ReadAsString("NLANGCOD"));
            
            return msg;
        }

        /// <summary>
        /// Returns the <see cref="VocollectSession"/> object associated with the terminal Id in the specified message.
        /// </summary>
        /// <param name="msg">A message containing the Terminal Id.</param>
        /// <returns>A <see cref="VocollectSession"/> object.</returns>
        /// <exception cref="SessionNotFoundException">
        /// </exception>
        private VocollectSession GetCurrentSession(MultiPartMessage msg)
        {
            VocollectSession session = SessionManager.Instance[msg.Properties.ReadAsString("SerialNumber")];

            if ((session == null) && (sessionPolicy == SessionPolicy.Required))
                throw new SessionNotFoundException(string.Format("Session object not found for Endpoint: \"{1}\" ({0}).", msg.Metadata.Read("ReceiveUri").ToString(), msg.Properties.ReadAsString("SerialNumber")));

            return session;
        }

        /// <summary>
        /// Creates an empty message part with the specified number of properties.
        /// </summary>
        /// <param name="propertyCount">The number of empty properties to create.</param>
        /// <returns>A <see cref="MessagePart"/> object.</returns>
        protected static MessagePart CreateEmptyMessagePart(int propertyCount)
        {
            MessagePart part = new VocollectMessagePart();

            for (int i = 0; i < propertyCount -2; i++)
            {
                part.Properties.Write(i.ToString(), "");
            }

            part.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            part.Properties.Write("Message", "");
            
            return part;
        }
        /// <summary>
        /// Trims any leading zeros from the container id sent by the terminal.
        /// </summary>
        /// <param name="containerId">The container id sent by the terminal.</param>
        /// <returns>A new container id without any leading zeros which will equal PBROW.SEQNUM.</returns>
        protected static string TrimContainerId(string containerId)
        {
            return containerId.TrimStart(new char[] { '0' });
        }

        protected static string ExtractPickPlaceKey(string location)
        {
            if (location.Contains("|"))
                return location.Substring(0, location.IndexOf('|'));
            else
                return location;
        }

        protected static string GetCachedAlarmText(string alarmId, VocollectSession session)
        {
            return GetCachedAlarmText(alarmId, session, null);
        }

        protected static string GetCachedAlarmText(string alarmId, string languageCode)
        {
            return GetCachedAlarmText(alarmId, null, languageCode);
        }

        private static string GetCachedAlarmText(string alarmId, VocollectSession session, string languageCode)
        {
            if ((session == null) || (!session.Contains(alarmId)))
            {
                CorrelationContext context;

                if (session != null)
                    languageCode = session.ReadAsString("NLANGCOD");

                MultiPartMessage whMsg = CreateRequestMessage("wlsystem", "getalmtxt", null);
                whMsg.Properties.Write("ALMID_I", alarmId);
                whMsg.Properties.Write("NLANGCOD_I", languageCode);
                whMsg.Properties.Write("ALMTXT_O", "");

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                string alarmText = context.ResponseMessages[0].Properties.ReadAsString("ALMTXT_O");

                if (session != null)
                    session.Write(alarmId, alarmText);

                return alarmText;
            }
            else
            {
                return session.ReadAsString(alarmId);
            }
        }
        
    }
        
}
