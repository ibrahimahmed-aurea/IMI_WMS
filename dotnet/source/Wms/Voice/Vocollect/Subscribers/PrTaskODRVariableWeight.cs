using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    [SessionPolicy(SessionPolicy.None)]
    public class PrTaskODRVariableWeight : VocollectSubscriber
    {
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            /* Ignore ODRs stored in flash output queue after terminal reset */
            if (session == null)
                return;

            CatchMeasure measure = new CatchMeasure();
            measure.Measurement = msg.Properties.ReadAsDouble("VariableWeight");
            session.CatchMeasureEntries.Add(measure);
   
            if (msg.Properties.ReadAsString("TransmissionStatus") == "1")
            {
                /* Final variable weight message */

                try
                {
                    MultiPartMessage pickedMsg = new MultiPartMessage();
                    pickedMsg.Properties.Write("AssignmentId", session.ReadAsString("PBHEADID"));
                    pickedMsg.Properties.Write("WorkId", "1");
                    pickedMsg.Properties.Write("LocationId", msg.Properties.ReadAsString("LocationId"));
                    pickedMsg.Properties.Write("QuantityPicked", (double)session.CatchMeasureEntries.Count);
                    pickedMsg.Properties.Write("PickedStatus", "1");
                    pickedMsg.Properties.Write("ContainerId", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                    pickedMsg.Properties.Write("Sequence", msg.Properties.ReadAsString("Sequence"));
                    pickedMsg.Properties.Write("DiscrepancyCode", session.ReadAsString("RESTCOD"));
                    pickedMsg.Properties.Write("SplitExchange", "0");

                    try
                    {
                        PrTaskLUTPicked.ProcessPickedMessage(pickedMsg, session);
                    }
                    catch (Exception ex)
                    {
                        session.Write("CatchMeasureException", ex);
                        throw;
                    }
                }
                finally
                {
                    session.CatchMeasureEntries.Clear();
                }
            }
        }
    }
}
