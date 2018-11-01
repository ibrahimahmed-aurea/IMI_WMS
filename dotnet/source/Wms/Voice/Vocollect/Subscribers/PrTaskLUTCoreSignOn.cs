using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;
using Imi.Framework.Shared.Security;
using Imi.Wms.Voice.Vocollect.Subscribers;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTCoreSignOn.
    /// </summary>
    [SessionPolicy(SessionPolicy.None)]
    public class PrTaskLUTCoreSignOn : VocollectSubscriber
    {
        private static int ErrorTempInterruptedPickOrderFound = 2;

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
            
            responseMsg.Properties.Write("Interleave", 0);
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");
            
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    /* Fetch user's language code */
                    MultiPartMessage langMsg = CreateRequestMessage("wlvoicepick", "get_user_languagecode", session);
                    langMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                    langMsg.Properties.Write("NLANGCOD_O", "");

                    CorrelationContext context;

                    MessageEngine.Instance.TransmitRequestMessage(langMsg, langMsg.MessageId, out context);

                    string languageCode = context.ResponseMessages[0].Properties.ReadAsString("NLANGCOD_O");

                    AuthenticationProvider provider = new AuthenticationProvider();
                    provider.Initialize(msg.Properties.ReadAsString("Password"));
                                        
                    MultiPartMessage authMsg = CreateRequestMessage("wlvoicepick", "authenticate", session);
                    authMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                    authMsg.Properties.Write("SALT_I", provider.Salt);
                    authMsg.Properties.Write("DATA_I", provider.SessionData);
                    authMsg.Properties.Write("WHID_I", "");
                    authMsg.Properties.Write("COMPANY_ID_I", "");
                    authMsg.Properties.Write("PRODUCT_I", "VOICE");
                    authMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                    authMsg.Properties.Write("SESSION_DATA_O", "");
                    authMsg.Properties.Write("ALMID_O", "");
                    authMsg.Properties.Write("ALMTXT1_O", "");
                    authMsg.Properties.Write("ALMTXT2_O", "");
                    authMsg.Properties.Write("ALMTXT3_O", "");

                    /* Set language code since we do not yet have a session */
                    authMsg.Metadata.Write("LanguageCode", languageCode);

                    MessageEngine.Instance.TransmitRequestMessage(authMsg, authMsg.MessageId, out context);

                    string sessionData = context.ResponseMessages[0].Properties.ReadAsString("SESSION_DATA_O");

                    AuthenticationSession authenticationSession = provider.DecryptSession(sessionData);

                    MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "logon", session);
                    whMsg.Properties.Write("TERID_I", msg.Properties.ReadAsString("SerialNumber"));
                    whMsg.Properties.Write("EMPID_I", msg.Properties.ReadAsString("Operator"));
                    whMsg.Properties.Write("SESSIONID_I", authenticationSession.SessionId);
                    whMsg.Properties.Write("WHID_O", "");
                    whMsg.Properties.Write("NLANGCOD_O", "");
                    whMsg.Properties.Write("ALMID_O", "");

                    /* Set language code since we do not yet have a session */
                    whMsg.Metadata.Write("LanguageCode", languageCode);

                    MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                    //Create session object
                    session = SessionManager.Instance.CreateSession(msg.Properties.ReadAsString("SerialNumber"));

                    /* Clear session if already exists */
                    session.Clear();

                    //Set session variables
                    session.Write("TERID", msg.Properties.ReadAsString("SerialNumber"));
                    session.Write("WHID", context.ResponseMessages[0].Properties.Read("WHID_O"));
                    session.Write("NLANGCOD", context.ResponseMessages[0].Properties.Read("NLANGCOD_O"));
                    session.Write("EMPID", msg.Properties.ReadAsString("Operator"));
                    session.Write("TUID", "");
                    session.Write("PBHEADID", "");
                    session.Write("PZID", "");
                    session.Write("PZGRPID", "");
                    session.Write("ITEID", "");
                                        
                    try
                    {
                        //Check if there is a temporarily stopped pick order available
                        PrTaskLUTGetAssignment.RequestPickOrder(PickOrderHoldType.Temporarily, session);

                        responseMsg.Properties.Write("ErrorCode", ErrorTempInterruptedPickOrderFound);
                    }
                    catch (WarehouseAdapterException ex)
                    {
                        if (ex.AlarmId != "PBHEAD030" && ex.AlarmId != "PBHEAD057")
                            throw;
                    }

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
        
    }
}
