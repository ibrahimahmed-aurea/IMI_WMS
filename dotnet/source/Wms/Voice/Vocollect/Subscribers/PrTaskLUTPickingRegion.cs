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
    /// Subscriber for PrTaskLUTPickingRegion.
    /// </summary>
    public class PrTaskLUTPickingRegion : VocollectSubscriber
    {
        private const string CARCODE_TYPE_SSCC = "S";
        private const string CARCODE_TYPE_Cross_Bar = "B";
        private const string CARCODE_TYPE_Code = "C";

        private enum PrintLabelConfig
        { 
            Never = 0,
            BeforePick = 1,
            AfterPick = 2,
            BeforeAndAfterPick = 3
        }

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
            
            responseMsg.Properties.Write("RegionNumber", 0);
            responseMsg.Properties.Write("Description", "");
            responseMsg.Properties.Write("AutoAssign", 1);
            responseMsg.Properties.Write("NumberAssignmentsAllowed", 1);
            responseMsg.Properties.Write("SkipAisleAllowed", 0);
            responseMsg.Properties.Write("SkipSlotAllowed", 0);
            responseMsg.Properties.Write("RepickSkips", 0);
            responseMsg.Properties.Write("CpSpeakWorkIdentifier", 0);
            responseMsg.Properties.Write("PrintLabels", 0);
            responseMsg.Properties.Write("PrintChaseLabels", 0);
            responseMsg.Properties.Write("SpeakSlotDescription", 0);
            responseMsg.Properties.Write("PickPrompt", 2);
            responseMsg.Properties.Write("SpeakWorkId", 0);
            responseMsg.Properties.Write("SignOffAllowed", 0);
            responseMsg.Properties.Write("ContainerType", 1);
            responseMsg.Properties.Write("DeliverContainerAtClose", 0);
            responseMsg.Properties.Write("PassAssignment", 0);
            responseMsg.Properties.Write("Delivery", 1);
            responseMsg.Properties.Write("QuantityVerification", 1);
            responseMsg.Properties.Write("WorkIdLength", 0);
            responseMsg.Properties.Write("GoBackForShorts", 0);
            responseMsg.Properties.Write("AllowReversePicking", 0);
            responseMsg.Properties.Write("UseLut", 2);
            responseMsg.Properties.Write("CurrentPreAisle", "".PadRight(string.Format(GetCachedAlarmText("VOICEPICK013", session), "").Length + 35, 'X'));
            responseMsg.Properties.Write("CurrentAisle", "".PadRight(3, 'X'));
            responseMsg.Properties.Write("CurrentPostAisle", "");
            responseMsg.Properties.Write("CurrentSlot", "".PadRight(64, 'X'));
            responseMsg.Properties.Write("PrintMultipleLabels", 0);
            responseMsg.Properties.Write("PromptOperatorForContainerId", 0);
            responseMsg.Properties.Write("AllowMultipleOpenContainersPerWorkId", 1);
            responseMsg.Properties.Write("SpokenContainerValidationLength", 1);
            responseMsg.Properties.Write("StopLineNumber", 0);
            responseMsg.Properties.Write("AllowAlternateDelivery", 0);
            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");

            try
            {
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pz", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                whMsg.Properties.Write("PZID_I", session.ReadAsString("PZID"));
                whMsg.Properties.Write("WHID_I", session.ReadAsString("WHID"));
                whMsg.Properties.Write("PZ_Cur_O", new object());
                whMsg.Properties.Write("ALMID_O", "");

                CorrelationContext context;
                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);
                
                PropertyCollection properties = context.ResponseMessages[0].Parts[0].Properties;

                responseMsg.Properties.Write("RegionNumber", msg.Properties.Read("RegionNumber"));
                responseMsg.Properties.Write("Description", properties.Read("PZNAME"));
                responseMsg.Properties.Write("SkipAisleAllowed", properties.Read("VOICE_ALLOW_SKIP_AISLE"));
                responseMsg.Properties.Write("SkipSlotAllowed", properties.Read("VOICE_ALLOW_SKIP_PP"));
                responseMsg.Properties.Write("RepickSkips", properties.Read("VOICE_ALLOW_REPICK_PP"));
                responseMsg.Properties.Write("QuantityVerification", properties.Read("VOICE_VERIFY_QTY"));
                responseMsg.Properties.Write("GoBackForShorts", properties.Read("VOICE_REPICK_SHORTED_PBROW"));
                responseMsg.Properties.Write("SpokenContainerValidationLength", properties.Read("VOICE_MIN_DIGITS_CARCODE"));
                        
                #region Print Label Config

                PrintLabelConfig labelConfig = PrintLabelConfig.Never;

                string CPL_PRINTCOD = properties.ReadAsString("CPL_PRINTCOD");
                string PLCP_PRINTCOD = properties.ReadAsString("PLCP_PRINTCOD");

                if ((CPL_PRINTCOD == "AP") || (PLCP_PRINTCOD == "AP"))
                    labelConfig = PrintLabelConfig.AfterPick;

                if ((CPL_PRINTCOD == "BP") || (PLCP_PRINTCOD == "BP"))
                {
                    if (labelConfig == PrintLabelConfig.AfterPick)
                        labelConfig = PrintLabelConfig.BeforeAndAfterPick;
                    else
                        labelConfig = PrintLabelConfig.BeforePick;
                }

                responseMsg.Properties.Write("PrintLabels", (int)labelConfig);

                #endregion

                if (string.IsNullOrEmpty(properties.ReadAsString("NO_OF_PBROWS")))
                    responseMsg.Properties.Write("StopLineNumber", 0);
                else
                    responseMsg.Properties.Write("StopLineNumber", properties.Read("NO_OF_PBROWS"));

                responseMsg.Properties.Write("AllowAlternateDelivery", properties.Read("ALLOW_CHANGE_ON_DROPWPADR"));
                responseMsg.Properties.Write("PickPrompt", properties.Read("VOICE_PICK_PROMPT"));
                responseMsg.Properties.Write("Delivery", properties.Read("VERIFY_DROPWPADR"));

                session.Write("VOICE_VERIFY_PP_BALANCE", properties.Read("VOICE_VERIFY_PP_BALANCE"));
                session.Write("VERIFY_DROPWPADR", properties.Read("VERIFY_DROPWPADR"));
                session.Write("VOICE_MIN_DIGITS_PPCODE", properties.Read("VOICE_MIN_DIGITS_PPCODE"));
                session.Write("VOICE_MIN_DIGITS_CARCODE", properties.Read("VOICE_MIN_DIGITS_CARCODE"));
                session.Write("VOICE_DIGITS_ITEID", properties.Read("VOICE_DIGITS_ITEID"));
                session.Write("VOICE_MIN_DIGITS_ITEVERIFY", properties.Read("VOICE_MIN_DIGITS_ITEVERIFY"));
                session.Write("VOICE_CHARS_PRODLOT", properties.Read("VOICE_CHARS_PRODLOT"));
                session.Write("VOICE_MIN_CHARS_PRODLOTVERIFY", properties.Read("VOICE_MIN_CHARS_PRODLOTVERIFY"));
                session.Write("ITESWAP", properties.Read("ITESWAP"));
                session.Write("CPL_PRINTCOD", properties.ReadAsString("CPL_PRINTCOD"));
                session.Write("PLCP_PRINTCOD", properties.ReadAsString("PLCP_PRINTCOD"));
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
