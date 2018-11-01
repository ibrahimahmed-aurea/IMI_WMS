using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrDialogueLUTAltDeliveryLocations
    /// </summary>
    public class PrTaskLUTAltDeliveryLocations : VocollectSubscriber
    {
        private const int VocollectErrorCodeAddressNotUnique = 2;
        private const int VocollectErrorCodeInvalidLocation = 101;

        /// <summary>
        /// Invokes the subscriber.
        /// </summary>
        /// <param name="msg">A reference to the subscribed message.</param>
        /// <param name="session">The current <see cref="VocollectSession"/> object.</param>
        /// <exception cref="WarehouseAdapterException">
        /// </exception>
        /// <exception cref="MessageEngineException">
        /// </exception>
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            MultiPartMessage responseMsg = CreateResponseMessage(msg);

            responseMsg.Properties.Write("DeliveryLocation", "");
            responseMsg.Properties.Write("DeliveryLocationCheckDigits", "");
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");
            
            try
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "verify_loc", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("WSID_IO", msg.Properties.ReadAsString("AlternateDeliveryArea"));
                whMsg.Properties.Write("WPADR_I", msg.Properties.ReadAsString("AlternateDeliveryLocation"));
                whMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
                whMsg.Properties.Write("ALMID_O", "");

                CorrelationContext context;

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                PropertyCollection properties = context.ResponseMessages[0].Properties;

                responseMsg.Properties.Write("DeliveryLocation", properties.Read("WSID_IO") + " " + msg.Properties.ReadAsString("AlternateDeliveryLocation"));
                                
                DropLoadCarrier(TrimContainerId(msg.Properties.ReadAsString("ContainerId")), properties.ReadAsString("WSID_IO"), msg.Properties.ReadAsString("AlternateDeliveryLocation"), session);

                /* Clear drop variables so the container won't be dropped in next call to PrTaskGetDeliveryLocations */
                session.Write("DROPSEQNUM", "");
                session.Write("DROPWSID", "");
                session.Write("DROPWPADR", "");
            }
            catch (WarehouseAdapterException ex)
            {
                if (ex.AlarmId == "WS001")
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeAddressNotUnique);
                else
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeInvalidLocation);

                responseMsg.Properties.Write("Message", ex.Message);
            }
            catch (Exception)
            {
                responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                responseMsg.Properties.Write("Message", GetCriticalErrorMessageText(msg));
                
                throw;
            }
            finally
            {
                TransmitResponseMessage(responseMsg, session);
            }
        }

        /// <summary>
        /// Drops a load carrier at the specified area and location.
        /// </summary>
        /// <param name="seqnum">The sequence number of the load carrier to drop.</param>
        /// <param name="dropArea">The drop area.</param>
        /// <param name="dropAddress">The drop location.</param>
        /// <param name="session">The current session.</param>
        /// <returns></returns>
        public static CorrelationContext DropLoadCarrier(string seqnum, string dropArea, string dropAddress, VocollectSession session)
        {
            MultiPartMessage dropMsg = CreateRequestMessage("wlvoicepick", "drop_loadcarrier", session);
            dropMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
            dropMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
            dropMsg.Properties.Write("SEQNUM_I", seqnum);
            dropMsg.Properties.Write("DROPWSID_I", dropArea);
            dropMsg.Properties.Write("DROPWPADR_I", dropAddress);
            dropMsg.Properties.Write("ALMID_O", "");

            CorrelationContext context;

            MessageEngine.Instance.TransmitRequestMessage(dropMsg, dropMsg.MessageId, out context);

            return context;
        }
    }
}
