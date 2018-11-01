using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTContainer.
    /// </summary>
    public class PrTaskLUTContainer : VocollectSubscriber
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
            WarehouseAdapterException printException = null;

            try
            {
                CorrelationContext context;

                MultiPartMessage whMsg;

                switch (msg.Properties.ReadAsInt("Operation"))
                { 
                    case 0:
                        break;
                    case 4:
                        break;
                    case 2:
                        //Operatoion = Create new container

                        whMsg = CreateRequestMessage("wlvoicepick", "new_loadcarrier", session);
                        whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                        whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                        whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                        whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("TargetContainer")));
                        whMsg.Properties.Write("SEQNUM_O", "");
                        whMsg.Properties.Write("ALMID_O", "");
                                            
                        MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                        try
                        {
                            /* Automatically print label on default printer */
                            if ((session.ReadAsString("CPL_PRINTCOD") == "BP") || (session.ReadAsString("PLCP_PRINTCOD") == "BP"))
                                PrTaskLUTPrint.Print(null, context.ResponseMessages[0].Properties.ReadAsString("SEQNUM_O"), session);
                        }
                        catch (WarehouseAdapterException ex)
                        {
                            printException = ex;
                        }

                        break;
                }

                whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pbcar", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));                
                whMsg.Properties.Write("PBCAR_Cur_O", new object());
                whMsg.Properties.Write("ALMID_O", "");
                                                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                if ((msg.Properties.ReadAsString("Operation") == "4") || (msg.Properties.ReadAsString("Operation") == "2"))
                {
                    /* Send container list in reverse order when dropping or when a new container has been created */
                    for (int i = context.ResponseMessages[0].Parts.Count -1; i >= 0; i--)
                    {
                        /* Do not send empty load carriers when dropping */
                        if ((context.ResponseMessages[0].Parts[i].Properties.ReadAsDouble("PIKNOPAKS") > 0) 
                            || (msg.Properties.ReadAsString("Operation") == "2")
                            || (session.ReadAsString("UPDATE_PBCAR") == "1"))
                        {
                            responseMsg.Parts.Add(CreateResponsePart(context.ResponseMessages[0].Parts[i], session));
                        }
                    }
                }
                else   
                {
                    /* Send container list in default order */
                    for (int i = 0; i < context.ResponseMessages[0].Parts.Count; i++)
                    {
                        responseMsg.Parts.Add(CreateResponsePart(context.ResponseMessages[0].Parts[i], session));
                    }
                }

                if (printException != null)
                {
                    if (responseMsg.Parts.Count > 0)
                    {
                        /* Report print error as information, user may continue */
                        responseMsg.Parts[responseMsg.Parts.Count - 1].Properties.Write("ErrorCode", VocollectErrorCodeInformationalError);
                        responseMsg.Parts[responseMsg.Parts.Count - 1].Properties.Write("Message", printException.Message);
                    }
                }

                session.Write("NOCARS", context.ResponseMessages[0].Parts.Count);
                session.Write("UPDATE_PBCAR", "0");
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(13);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(13);
                part.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                part.Properties.Write("Message", GetCriticalErrorMessageText(msg));
                responseMsg.Parts.Add(part);
                                
                throw;
            }
            finally
            {
                TransmitResponseMessage(responseMsg, session);
            }
        }

        private MessagePart CreateResponsePart(MessagePart part, VocollectSession session)
        {
            MessagePart responsePart = new VocollectMessagePart();

            responsePart.Properties.Write("SystemContainerId", part.Properties.ReadAsString("SEQNUM").PadLeft(session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"), '0'));
            responsePart.Properties.Write("ScannedContainerValidation", part.Properties.Read("CARCODE"));

            string carCode = part.Properties.ReadAsString("CARCODE");

            if (carCode.Length > session.ReadAsDecimal("VOICE_MIN_DIGITS_CARCODE"))
                carCode = carCode.Substring(carCode.Length - session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"), session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"));
            else if (carCode.Length < session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"))
                carCode = carCode.PadLeft(session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"), '0');

            responsePart.Properties.Write("SpokenContainerValidation", carCode);
            responsePart.Properties.Write("WorkId", "1");
            responsePart.Properties.Write("TargetContainer", "");
            responsePart.Properties.Write("ContainerStatus", "O");
            responsePart.Properties.Write("Printed", "1");
            responsePart.Properties.Write("ContainerType", part.Properties.Read("CARNAME"));
            responsePart.Properties.Write("Area", part.Properties.Read("WSID"));
            responsePart.Properties.Write("Location", part.Properties.Read("WPADR"));
            responsePart.Properties.Write("CustomerNumber", GetCotainerHashCode(part).ToString());
            responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responsePart.Properties.Write("Message", "");

            return responsePart;
        }

        /// <summary>
        /// Calculates a unique hash code for the container based on customer, client and shipping location data.
        /// The hash code is used during the split and exchange operations to determine if the target container
        /// is valid. For the target cotainer to be valid it must have the same hash code as the source container.
        /// </summary>
        /// <param name="part">A <see cref="MessagePart"/> containing container information.</param>
        /// <returns>A hash code value.</returns>
        public static int GetCotainerHashCode(MessagePart part)
        {
            int hashCode = 17;

            hashCode = 37 * hashCode + part.Properties.ReadAsString("SHIPTOCUSID").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("SHIPTOCUSQUALIFIER").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("SHIPTOPARTYID").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("SHIPTOPARTYQUALIFIER").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("COMPANY_ID").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("TOWSID").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("TOWPADR").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("COID_SINGLE").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("COSEQ_SINGLE").GetHashCode();
            hashCode = 37 * hashCode + part.Properties.ReadAsString("COSUBSEQ_SINGLE").GetHashCode();

            return hashCode;
            
        }
    }
}
