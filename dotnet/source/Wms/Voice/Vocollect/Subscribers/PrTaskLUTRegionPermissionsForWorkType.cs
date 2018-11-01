using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTRegionPermissionsForWorkType.
    /// </summary>
    public class PrTaskLUTRegionPermissionsForWorkType : VocollectSubscriber
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

            try
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_emp_pz", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("EMP_PZ_Cur_O", new object());
                whMsg.Properties.Write("ALMID_O", "");

                CorrelationContext context;
                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                foreach (MessagePart part in context.ResponseMessages[0].Parts)
                {
                    VocollectMessagePart responsePart = new VocollectMessagePart();

                    responsePart.Properties.Write("RegionNumber", part.Properties.Read("PZID"));
                    responsePart.Properties.Write("RegionName", part.Properties.Read("PZNAME"));
                    
                    if (part.Properties.ReadAsString("FROM_TABLE") == "PZGRP")
                        responsePart.Properties.Write("GroupRegion", 1);
                    else
                        responsePart.Properties.Write("GroupRegion", 0);

                    if ((part.Properties.ReadAsString("PICKTRUCK") == "1")
                        && (string.IsNullOrEmpty(session.ReadAsString("TUID"))))
                        responsePart.Properties.Write("UseVehicle", 1);
                    else
                        responsePart.Properties.Write("UseVehicle", 0);

                    responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                    responsePart.Properties.Write("Message", "");

                    responseMsg.Parts.Add(responsePart);
                }
            
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(6);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(6);
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

