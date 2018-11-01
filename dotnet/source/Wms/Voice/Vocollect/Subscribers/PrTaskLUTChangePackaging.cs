using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTChangePackaging.
    /// </summary>
    public class PrTaskLUTChangePackaging : VocollectSubscriber
    {
        private const int VocollectErrorCodeNoPackaging = 101;

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
            
            try
            {
                int addQty = msg.Properties.ReadAsInt("PackagingQty");
                
                CorrelationContext context = null;
                
                switch (msg.Properties.ReadAsString("PackagingTransaction"))
                {
                    case "0":

                        MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pbcarpm", session);
                        whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                        whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                        whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                        whMsg.Properties.Write("PBCARPM_Cur_O", new object());
                        whMsg.Properties.Write("ALMID_O", "");
                        
                        MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                        if (context.ResponseMessages[0].Parts.Count > 0)
                        {
                            foreach (MessagePart part in context.ResponseMessages[0].Parts)
                            {
                                VocollectMessagePart responsePart = new VocollectMessagePart();

                                responsePart.Properties.Write("PackNumber", part.Properties.Read("PMTYPID"));
                                responsePart.Properties.Write("PackQuantity", part.Properties.Read("ADDQTY"));
                                responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                                responsePart.Properties.Write("Message", "");

                                responseMsg.Parts.Add(responsePart);
                            }
                        }
                        else
                        {
                            MessagePart part = CreateEmptyMessagePart(4);
                            part.Properties.Write("ErrorCode", VocollectErrorCodeNoPackaging);
                            part.Properties.Write("Message", "");
                            responseMsg.Parts.Add(part);
                        }

                        break;

                    case "1":

                        whMsg = CreateRequestMessage("wlvoicepick", "modify_pm", session);
                        whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                        whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                        whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                        whMsg.Properties.Write("PMTYPID_I", msg.Properties.Read("PackNumber").ToString());
                        whMsg.Properties.Write("ADDQTY_I", addQty);
                        whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                        whMsg.Properties.Write("NEWQTY_O", 0);
                        whMsg.Properties.Write("ALMID_O", "");
                        
                        MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);
                                                
                        goto case "0";

                    case "2":

                        addQty = -addQty;

                        goto case "1";
                }
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(4);
                
                if (ex.AlarmId == "PBCARPM003")
                    part.Properties.Write("ErrorCode", VocollectErrorCodeNoPackaging);
                else if (ex.AlarmId == "PMTYP001")
                    part.Properties.Write("ErrorCode", VocollectErrorCodeNoPackaging);
                else
                    part.Properties.Write("ErrorCode", WarehouseAlarm);

                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(4);
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

    }
}
