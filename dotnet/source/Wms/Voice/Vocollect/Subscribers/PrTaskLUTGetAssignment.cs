using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;


namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Pick Order Hold Type status codes.
    /// </summary>
    public enum PickOrderHoldType
    {
        /// <summary>
        /// The pick order is temporarily stopped.
        /// </summary>
        Temporarily = 'T',
        /// <summary>
        /// The pick order is definitely stopped.
        /// </summary>
        Definitely = 'D',
        /// <summary>
        /// The pick order is not stopped.
        /// </summary>
        None
    }

    /// <summary>
    /// Subscriber for PrTaskLUTGetAssignment.
    /// </summary>
    public class PrTaskLUTGetAssignment : VocollectSubscriber
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
            
            responseMsg.Properties.Write("AssignmentId", 0);
            responseMsg.Properties.Write("IsChase", 0);
            responseMsg.Properties.Write("WorkId", "1");
            responseMsg.Properties.Write("Position", "1");
            responseMsg.Properties.Write("TotalItems", 0);
            responseMsg.Properties.Write("ItemsNotPicked", 0);
            responseMsg.Properties.Write("BaseItemsNotPicked", 0);
            responseMsg.Properties.Write("Cube", 0);
            responseMsg.Properties.Write("GoalTime", 0);
            responseMsg.Properties.Write("Route", "");
            responseMsg.Properties.Write("ActiveContainer", "00");
            responseMsg.Properties.Write("PassAssignment", "0");
            responseMsg.Properties.Write("PreviousWork", "0");
            responseMsg.Properties.Write("PickLines", 0);
            responseMsg.Properties.Write("PickedLines", 0);
            responseMsg.Properties.Write("CustomerNumber", "");
            responseMsg.Properties.Write("PreviousWorkVehicle", "");
            responseMsg.Properties.Write("AllowPackaging", "1");
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    PropertyCollection properties = null;

                    /* Check if pick order is already fetched */
                    if (session.ReadAsString("PBHEADID").Length == 0)
                    {
                        CorrelationContext context = RequestPickOrder(PickOrderHoldType.None, session);
                        properties = context.ResponseMessages[0].Parts[0].Properties;
                    }
                    else /* Use data from session */
                        properties = session.CurrentAssignmentData;


                    responseMsg.Properties.Write("AssignmentId", session.ReadAsString("PBHEADID"));
                    responseMsg.Properties.Write("TotalItems", 0);
                    responseMsg.Properties.Write("ItemsNotPicked", properties.ReadAsDouble("TOTQTY") - properties.ReadAsDouble("PIKQTY"));

                    responseMsg.Properties.Write("GoalTime", ((TimeSpan)session.Read("PICKTIME")).Minutes);

                    if (string.IsNullOrEmpty(session.ReadAsString("HOLD_TYPE")))
                        responseMsg.Properties.Write("PreviousWork", 0);
                    else
                    {
                        if (string.IsNullOrEmpty(properties.ReadAsString("TUID")))
                            responseMsg.Properties.Write("PreviousWork", 1);
                        else
                            responseMsg.Properties.Write("PreviousWork", 2);
                    }

                    responseMsg.Properties.Write("PickLines", properties.Read("NOROWS"));
                    responseMsg.Properties.Write("PickedLines", properties.Read("NOPIKROW"));


                    //responseMsg.Properties.Write("AllowPackaging", properties.Read("CONFIRM_PM"));
                    responseMsg.Properties.Write("PreviousWorkVehicle", properties.Read("TUID"));

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

        /// <summary>
        /// Requests a pick order from the WMS.
        /// </summary>
        /// <param name="holdType">The desired hold type of the pick order.</param>
        /// <param name="session">The current session.</param>
        /// <returns></returns>
        public static CorrelationContext RequestPickOrder(PickOrderHoldType holdType, VocollectSession session)
        {
            MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pbhead", session);
            whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
            whMsg.Properties.Write("TUID_I", session.ReadAsString("TUID"));
            whMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
            
            if (holdType == PickOrderHoldType.Temporarily)
            {
                whMsg.Properties.Write("PZID_I", "");
                whMsg.Properties.Write("PZGRPID_I", "");
            }
            else
            {
                whMsg.Properties.Write("PZID_I", session.ReadAsString("PZID"));
                whMsg.Properties.Write("PZGRPID_I", session.ReadAsString("PZGRPID"));
            }

            whMsg.Properties.Write("NLANGCOD_I", session.ReadAsString("NLANGCOD"));
            whMsg.Properties.Write("PBHEAD_Cur_O", new object());
            whMsg.Properties.Write("HOLD_TYPE_O", "");
            whMsg.Properties.Write("LOCATION_O", "");
            whMsg.Properties.Write("ALMID_O", "");
            
            if (holdType == PickOrderHoldType.Definitely)
                whMsg.Properties.Write("DEF_ON_HOLD_I", "1");
            else
                whMsg.Properties.Write("DEF_ON_HOLD_I", "0");
            
            CorrelationContext context;
            
            MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

            //Get properties from cursor
            PropertyCollection properties = context.ResponseMessages[0].Parts[0].Properties;

            if ((!string.IsNullOrEmpty(context.ResponseMessages[0].Properties.ReadAsString("HOLD_TYPE_O")))
                && (!string.IsNullOrEmpty(properties.ReadAsString("TUID"))))
            {
                VerifyAndConnectPickTruck(session, properties.ReadAsString("PBHEADID"), session.ReadAsString("TUID"), properties.ReadAsString("TUID"));
            }

            //Write data to session
            session.Write("PBHEADID", properties.ReadAsString("PBHEADID"));
            session.Write("PZID", properties.ReadAsString("PZID"));
            session.Write("HOLD_TYPE", context.ResponseMessages[0].Properties.ReadAsString("HOLD_TYPE_O"));
            session.Write("CARCODE_TYPE", properties.ReadAsString("CARCODE_TYPE"));
            session.Write("PBROWID", "");
            session.Write("DROPSEQNUM", "");
            session.Write("DROPWSID", "");
            session.Write("DROPWPADR", "");
            session.Write("UPDATE_PBCAR", "0");
            session.Write("NOCARS", properties.ReadAsInt("NOCARS"));
            session.Write("PRINTED", "0");
            session.Write("PRINTERNUMBER", "");
            session.CurrentAssignmentData = properties;

            TimeSpan pickTime = new TimeSpan(0, 0,
                Convert.ToInt32(properties.ReadAsInt("ESTPICKTME")
                + properties.ReadAsInt("ESTMOVTIMEPICK")
                + properties.ReadAsInt("ESTDRVTME")
                + properties.ReadAsInt("ESTPACKTME")));

            session.Write("PICKTIME", pickTime);
            session.Write("LPDTM_SHIPWS", properties.Read("LPDTM_SHIPWS"));
                                                
            return context;
        }

        /// <summary>
        /// Verifies and connects the pick truck of a definitely stopped pick order.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <param name="verifyVehicleId">The vehicle id to verify.</param>
        public static void VerifyAndConnectPickTruck(VocollectSession session, string assignmentId, string vehicleId, string verifyVehicleId)
        {
            MultiPartMessage tuMsg = CreateRequestMessage("wlvoicepick", "verify_and_connect_tu", session);
            tuMsg.Properties.Write("PBHEADID_I", assignmentId);
            tuMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
            tuMsg.Properties.Write("TUID_I", vehicleId);
            tuMsg.Properties.Write("VERIFY_TUID_I", verifyVehicleId);
            tuMsg.Properties.Write("VERIFY_TUID_O", "");
            tuMsg.Properties.Write("ALMID_O", "");

            CorrelationContext context;

            MessageEngine.Instance.TransmitRequestMessage(tuMsg, tuMsg.MessageId, out context);
            
            //Update session with new vehicle id
            session.Write("TUID", context.ResponseMessages[0].Properties.ReadAsString("VERIFY_TUID_O"));
        }
    }
}
