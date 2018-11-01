using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    public class PrTaskLUTReplenReq : VocollectSubscriber
    {
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            MultiPartMessage responseMsg = CreateResponseMessage(msg);

            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeInformationalError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "replenish_loc", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PBROWID_I", msg.Properties.ReadAsString("Sequence"));
                whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                whMsg.Properties.Write("NLANGCOD_I", session.ReadAsString("NLANGCOD"));
                whMsg.Properties.Write("ALMID_O", "");
                whMsg.Properties.Write("RESULT_O", "");

                CorrelationContext context;

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                responseMsg.Properties.Write("Message", context.ResponseMessages[0].Properties.ReadAsString("RESULT_O").ToLower());

            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeInformationalError);
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
