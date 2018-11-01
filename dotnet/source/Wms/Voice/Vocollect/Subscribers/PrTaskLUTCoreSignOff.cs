using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCoreSignOff.
    /// </summary>
    public class PrTaskLUTCoreSignOff : VocollectSubscriber
    {
        private const string HoldTypeTemporary = "T";
        private const string HoldTypeDefinite = "D";

        private const int VocollectHoldTypeNormal = 0;
        private const int VocollectHoldTypeTemporary = 1;
        private const int VocollectHoldTypeDefinite = 2;
        
        private const int VocollectErrorCodeAddressNotUnique = 2;
        private const int VocollectErrorCodeInvalidLocation = 101;

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
            responseMsg.Properties.Write("ErrorCode", (int)VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                bool signOff = false;

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    CorrelationContext context;

                    string holdType = "";
                    
                    switch (msg.Properties.ReadAsInt("DefiniteStop"))
                    {
                        case VocollectHoldTypeTemporary:
                            holdType = HoldTypeTemporary;
                            break;
                        case VocollectHoldTypeDefinite:
                            holdType = HoldTypeDefinite;
                            break;
                        case VocollectHoldTypeNormal:
                            if (!string.IsNullOrEmpty(session.ReadAsString("PBHEADID")))
                            {
                                holdType = HoldTypeTemporary;
                                signOff = true;
                            }
                            break;
                    }
                    
                    if (!string.IsNullOrEmpty(holdType))
                    {
                        string area = "";
                        string location = "";

                        if (holdType == HoldTypeDefinite)
                        {

                            if ((session.ReadAsString("PRINTED") == "1") ||
                               ((session.ReadAsString("PRINTED") == "0") && (session.ReadAsString("CPL_PRINTCOD") == "AP") && (session.ReadAsString("PLCP_PRINTCOD") == "AP")) ||
                               ((session.ReadAsString("PRINTED") == "0") && (session.ReadAsString("CPL_PRINTCOD") == "N") && (session.ReadAsString("PLCP_PRINTCOD") == "N")))
                            {
                                #region Verify Location

                                MultiPartMessage locMsg = CreateRequestMessage("wlvoicepick", "verify_loc", session);
                                locMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                                locMsg.Properties.Write("WSID_IO", msg.Properties.ReadAsString("Area"));
                                locMsg.Properties.Write("WPADR_I", msg.Properties.ReadAsString("Location"));
                                locMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
                                locMsg.Properties.Write("ALMID_O", "");

                                MessageEngine.Instance.TransmitRequestMessage(locMsg, locMsg.MessageId, out context);

                                area = context.ResponseMessages[0].Properties.ReadAsString("WSID_IO");
                                location = msg.Properties.ReadAsString("Location");

                                #endregion
                            }
                            else
                            {
                                responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeInvalidLocation);
                                responseMsg.Properties.Write("Message", GetCachedAlarmText("VOICEPICK016", session));
                                return;
                            }
                        }

                        #region Stop Pick

                        MultiPartMessage stopMsg = CreateRequestMessage("wlvoicepick", "hold_rfpick", session);
                        stopMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                        stopMsg.Properties.Write("WSID_I", area);
                        stopMsg.Properties.Write("WPADR_I", location);
                        stopMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                        stopMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                        stopMsg.Properties.Write("HOLD_TYPE_I", holdType);
                        stopMsg.Properties.Write("ALMID_O", "");

                        MessageEngine.Instance.TransmitRequestMessage(stopMsg, stopMsg.MessageId, out context);

                        #endregion
                    }

                    if ((holdType != HoldTypeTemporary) || (signOff))
                    {
                        #region Disconnect Terminal From Pick Truck

                        if ((session != null) && (!string.IsNullOrEmpty(session.ReadAsString("TUID"))))
                        {
                            MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "disconnect_ter_from_pt", session);
                            whMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                            whMsg.Properties.Write("TUID_I", session.ReadAsString("TUID"));
                            whMsg.Properties.Write("KEEP_ONLINE_I", "0");
                            whMsg.Properties.Write("ALMID_O", "");

                            MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);
                        }

                        #endregion

                        #region Log Off

                        MultiPartMessage logoffMsg = CreateRequestMessage("wlvoicepick", "logoff", session);
                        logoffMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                        logoffMsg.Properties.Write("ALMID_O", "");

                        MessageEngine.Instance.TransmitRequestMessage(logoffMsg, logoffMsg.MessageId, out context);

                        SessionManager.Instance.DestroySession(msg.Properties.ReadAsString("SerialNumber"));

                        #endregion
                    }

                    transactionScope.Complete();
                }

                
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Properties.Write("Message", ex.Message);

                if (ex.AlarmId == "WS001")
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeAddressNotUnique);
                else if (ex.AlarmId == "WP001")
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeInvalidLocation);
                else
                {
                    responseMsg.Properties.Write("ErrorCode", WarehouseAlarm);
                    throw;
                }
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
