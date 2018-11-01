using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    [SessionPolicy(SessionPolicy.None)]
    public class PrTaskODRCoreSendBreakInfo : VocollectSubscriber
    {
        public override void Invoke(MultiPartMessage msg, VocollectSession session)
        {
            /* Ignore ODR stored in flash output queue after terminal reset */
            if (session == null)
                return;

            using (TransactionScope transactionScope = new TransactionScope())
            {
                PrTaskLUTGetAssignment.RequestPickOrder(PickOrderHoldType.Temporarily, session);

                transactionScope.Complete();
            }
        }
    }
}
