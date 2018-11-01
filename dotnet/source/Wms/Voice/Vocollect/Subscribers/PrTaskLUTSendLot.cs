using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    public class PrTaskLUTSendLot : VocollectSubscriber
    {
        private static int ErrorInvalidEntry = 101;

        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            MultiPartMessage responseMsg = CreateResponseMessage(msg);

            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                ProductionLot lot = new ProductionLot();

                lot.LotId = msg.Properties.ReadAsString("LotId");
                lot.Quantity = msg.Properties.ReadAsDouble("Qty");
                session.ProductionLots.Add(lot);
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Properties.Write("ErrorCode", ErrorInvalidEntry);
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
    }
}
