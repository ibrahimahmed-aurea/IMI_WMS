
using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTDiscrepancyCode.
    /// </summary>
    public class PrTaskLUTDiscrepancyCode : VocollectSubscriber
    {
        private const string AqtyDiscNegative = "N";
        private const string AqtyDiscPositive = "P";
        private static int ErrorDefinitelyStoppedPickOrderFound = 2;

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
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    CorrelationContext context;

                    if (!string.IsNullOrEmpty(msg.Properties.ReadAsString("RegionNumber")))
                    {
                        //Check if region is a pick zone or a pick zone group
                        MultiPartMessage pzMsg = CreateRequestMessage("wlvoicepick", "select_zone_or_group", session);
                        pzMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                        pzMsg.Properties.Write("PZID_I", msg.Properties.ReadAsString("RegionNumber"));
                        pzMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                        pzMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
                        pzMsg.Properties.Write("TYPE_O", "");
                        pzMsg.Properties.Write("ALMID_O", "");

                        MessageEngine.Instance.TransmitRequestMessage(pzMsg, pzMsg.MessageId, out context);

                        if (context.ResponseMessages[0].Properties.ReadAsString("TYPE_O") == "PZ")
                        {
                            session.Write("PZID", msg.Properties.ReadAsString("RegionNumber"));
                            session.Write("PZGRPID", "");
                        }
                        else
                        {
                            session.Write("PZID", "");
                            session.Write("PZGRPID", msg.Properties.ReadAsString("RegionNumber"));
                        }
                    }

                    MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_restcod", session);
                    whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    whMsg.Properties.Write("NLANGCOD_I", session.ReadAsString("NLANGCOD"));
                    whMsg.Properties.Write("RESTCOD_Cur_O", new object());
                    whMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
                    whMsg.Properties.Write("PZID_I", session.ReadAsString("PZID"));

                    MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                    foreach (MessagePart part in context.ResponseMessages[0].Parts)
                    {
                        int temp;

                        /* Code must be numeric in order to be valid */
                        if (!int.TryParse(part.Properties.ReadAsString("OLACOD"), out temp))
                            continue;

                        MessagePart responsePart = new VocollectMessagePart();

                        responsePart.Properties.Write("DiscrepancyCode", part.Properties.Read("OLACOD"));
                        responsePart.Properties.Write("DiscrepancyCodeDescription", part.Properties.Read("RESTCODTXT"));

                        if (part.Properties.ReadAsString("AQTYDISC") == AqtyDiscPositive)
                            responsePart.Properties.Write("AllowOverpick", 1);
                        else if (part.Properties.ReadAsString("AQTYDISC") == AqtyDiscNegative)
                            responsePart.Properties.Write("AllowOverpick", 0);
                        else
                            continue;

                        responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                        responsePart.Properties.Write("Message", "");

                        responseMsg.Parts.Add(responsePart);
                    }

                    if (string.IsNullOrEmpty(session.ReadAsString("PBHEADID")))
                    {
                        try
                        {
                            PrTaskLUTGetAssignment.RequestPickOrder(PickOrderHoldType.Definitely, session);

                            if (responseMsg.Parts.Count == 0)
                            {
                                MessagePart part = CreateEmptyMessagePart(5);
                                responseMsg.Parts.Add(part);
                            }

                            responseMsg.Parts[0].Properties.Write("ErrorCode", ErrorDefinitelyStoppedPickOrderFound);
                        }
                        catch (WarehouseAdapterException ex)
                        {
                            if (ex.AlarmId != "PBHEAD030" && ex.AlarmId != "PBHEAD057")
                                throw;
                        }
                    }

                    transactionScope.Complete();
                }
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(5);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);
             
                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(5);
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
