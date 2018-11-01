using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTGetDeliveryLocation.
    /// </summary>
    public class PrTaskLUTGetDeliveryLocation : VocollectSubscriber
    {
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
            responseMsg.Properties.Write("DeliveryLocationCheckDigits", 0);
            responseMsg.Properties.Write("ContainerId", 0);
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    /* Execute drop of previous container if not already dropped using the alternate delivery command */
                    if (!string.IsNullOrEmpty(session.ReadAsString("DROPSEQNUM")))
                    {
                        PrTaskLUTAltDeliveryLocations.DropLoadCarrier(session.ReadAsString("DROPSEQNUM"), session.ReadAsString("DROPWSID"), session.ReadAsString("DROPWPADR"), session);
                    }

                    MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_drop_location", session);
                    whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                    whMsg.Properties.Write("DROPWSID_O", "");
                    whMsg.Properties.Write("DROPWPADR_O", "");
                    whMsg.Properties.Write("WPCOD_O", "");
                    whMsg.Properties.Write("SLOT_O", "");
                    whMsg.Properties.Write("ALMID_O", "");

                    CorrelationContext context;

                    MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                    PropertyCollection properties = context.ResponseMessages[0].Properties;
                                        
                    if (!string.IsNullOrEmpty(properties.ReadAsString("SLOT_O")))
                    {
                        /* VOICEPICK015 = 'slot' */
                        string slot = GetCachedAlarmText("VOICEPICK015", session);

                        responseMsg.Properties.Write("DeliveryLocation", properties.Read("DROPWSID_O") + " , " + properties.Read("DROPWPADR_O") + " , " + slot + " , " + properties.Read("SLOT_O"));
                    }
                    else
                    {
                        responseMsg.Properties.Write("DeliveryLocation", properties.Read("DROPWSID_O") + " , " + properties.Read("DROPWPADR_O"));
                    }

                    if (session.ReadAsString("VERIFY_DROPWPADR") == "1")
                    {
                        string checkDigits = properties.ReadAsString("WPCOD_O");

                        if (!string.IsNullOrEmpty(checkDigits))
                        {
                            long temp;

                            if (!long.TryParse(checkDigits, out temp))
                                checkDigits = "";

                            if (checkDigits.Length > session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"))
                                checkDigits = checkDigits.Substring(checkDigits.Length - session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"), session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"));
                        }

                        responseMsg.Properties.Write("DeliveryLocationCheckDigits", checkDigits);
                    }
                    else
                        responseMsg.Properties.Write("DeliveryLocationCheckDigits", "");

                    responseMsg.Properties.Write("ContainerId", msg.Properties.ReadAsString("ContainerId"));

                    session.Write("DROPSEQNUM", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                    session.Write("DROPWSID", properties.Read("DROPWSID_O"));
                    session.Write("DROPWPADR", properties.Read("DROPWPADR_O"));

                    transactionScope.Complete();
                }
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Properties.Write("ErrorCode", WarehouseAlarm);
                responseMsg.Properties.Write("Message", ex.Message);
                
                throw;
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

    }
}
