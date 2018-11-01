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
    /// Subscriber for PrTaskLUTGetPicks.
    /// </summary>
    public class PrTaskLUTGetPicks : VocollectSubscriber
    {
        private static string PBROWSTAT_Started = "3";
        private static string PBROWSTAT_Planned = "2";
        private static string PBROWSTAT_Registrated = "1";
        private static string PBROWSTAT_Finished = "4";

        private static string PPVERIFY_ARTID = "A";
        private static string PPVERIFY_EANDUN = "E";

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
                CorrelationContext context;

                if (!string.IsNullOrEmpty(session.ReadAsString("PBROWID")))
                {
                    /* Refresh rows in status Registrated before the shorts pass */
                    MultiPartMessage refreshMsg = CreateRequestMessage("wlvoicepick", "refresh_pickorderrows", session);
                    refreshMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    refreshMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    refreshMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                    refreshMsg.Properties.Write("ALMID_O", "");

                    MessageEngine.Instance.TransmitRequestMessage(refreshMsg, refreshMsg.MessageId, out context);
                }
                
                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "get_mt_pbrow", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                whMsg.Properties.Write("NLANGCOD_I", session.ReadAsString("NLANGCOD"));
                whMsg.Properties.Write("PBROW_Cur_O", new object());
                whMsg.Properties.Write("ALMID_O", "");
                                
                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                PropertyCollection properties = context.ResponseMessages[0].Parts[0].Properties;

                string prevContainer = "";
                string prevLocation = "";
                int rowCount = 0;

                foreach (MessagePart part in context.ResponseMessages[0].Parts)
                {
                    string status = null;
                    double pickQty = 0;

                    if ((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Finished)
                        && (part.Properties.ReadAsDouble("PICKQTY") < part.Properties.ReadAsDouble("ORDQTY")))
                    {
                        rowCount++;
                        status = "G"; //Go back  for shortage
                        pickQty = part.Properties.ReadAsDouble("ORDQTY") - part.Properties.ReadAsDouble("PICKQTY");
                    }
                    else if (((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Started) 
                        || (part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated))
                        && (!string.IsNullOrEmpty(session.ReadAsString("PBROWID"))))
                    {
                        rowCount++;
                        status = "S"; //Skipped
                        pickQty = part.Properties.ReadAsDouble("ORDQTY");
                    }
                    else if ((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Started)
                        || (part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated))
                    {
                        rowCount++;
                        status = "N"; //Not picked
                        pickQty = part.Properties.ReadAsDouble("ORDQTY");
                    }
                    else
                        status = "P"; //Picked
                                        
                    MessagePart responsePart = new VocollectMessagePart();

                    responsePart.Properties.Write("Status", status);
                    responsePart.Properties.Write("BaseItem", "0");
                    responsePart.Properties.Write("Sequence", part.Properties.Read("PBROWID"));
                    
                    /* Check if multiple picks for same location and pick load carrier */
                    if ((prevLocation == part.Properties.ReadAsString("PPKEY"))
                        && (prevContainer == part.Properties.ReadAsString("SEQNUM"))
                        && (session.ReadAsInt("NOCARS") == 1))
                    {
                        /* Append PBROWID to make location unique to prevent terminal from merging the lines */
                        responsePart.Properties.Write("LocationId", string.Format("{0}|{1}|{2}", part.Properties.Read("PPKEY"), part.Properties.ReadAsString("ITEID"), part.Properties.Read("PBROWID")));
                    }
                    else
                    {
                        /*Append PAKID to make location unique for each pick package. Will prevent terminal from merging different pick packages*/
                        responsePart.Properties.Write("LocationId", string.Format("{0}|{1}|{2}", part.Properties.Read("PPKEY"), part.Properties.ReadAsString("ITEID"), part.Properties.Read("PAKID")));
                    }
                    
                    responsePart.Properties.Write("Region", "0");

                    /* VOICEPICK013 = 'Go to area {0}' */
                    string preAisle = GetCachedAlarmText("VOICEPICK013", session);
                    preAisle = string.Format(preAisle, part.Properties.ReadAsString("WSNAME").PadRight(35, ' '));
                    responsePart.Properties.Write("PreAisleDirection", preAisle);

                    responsePart.Properties.Write("Aisle", part.Properties.ReadAsString("AISLE").PadRight(3, ' '));
                    responsePart.Properties.Write("PostAisleDirection", "");

                    string slot = part.Properties.ReadAsString("WPADR");
                    
                    string itemLoadId = part.Properties.ReadAsString("ITEID");

                    if (!string.IsNullOrEmpty(itemLoadId))
                    {
                        if (itemLoadId.Length > session.ReadAsInt("VOICE_DIGITS_ITEID"))
                            itemLoadId = itemLoadId.Substring(itemLoadId.Length - session.ReadAsInt("VOICE_DIGITS_ITEID"), session.ReadAsInt("VOICE_DIGITS_ITEID"));
                    }

                    if ((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated)
                        || (((part.Properties.ReadAsString("ITEVERIFY") == "1") || (part.Properties.ReadAsString("ITETRACE") == "1"))
                        && (string.IsNullOrEmpty(part.Properties.ReadAsString("ITEID")))))
                    {
                        /* No item load found */
                        slot += "," + GetCachedAlarmText("VOICEPICK010", session);
                    }
                    else if (part.Properties.ReadAsString("ITEVERIFY") == "1")
                    {
                        /* Append verify item load id to slot */
                        slot += string.Format("," + GetCachedAlarmText("VOICEPICK008", session), itemLoadId);
                    }
                    else if (part.Properties.ReadAsString("ITEVERIFY") == "2")
                    {
                        /* Append verify lot to slot */
                        string lot = part.Properties.ReadAsString("PRODLOT");

                        if (!string.IsNullOrEmpty(lot))
                        {
                            if (lot.Length > session.ReadAsInt("VOICE_CHARS_PRODLOT"))
                                lot = lot.Substring(lot.Length - session.ReadAsInt("VOICE_CHARS_PRODLOT"), session.ReadAsInt("VOICE_CHARS_PRODLOT"));
                        }
                                                
                        slot += string.Format("," + GetCachedAlarmText("VOICEPICK017", session), lot);
                    }
                    else if (!string.IsNullOrEmpty(part.Properties.ReadAsString("ITEID")))
                    {
                        /* Append item load id to slot */
                        slot += string.Format("," + GetCachedAlarmText("VOICEPICK009", session), itemLoadId);
                    }
                    
                    responsePart.Properties.Write("Slot", slot.PadRight(64, ' '));

                    responsePart.Properties.Write("QuantityToPick", pickQty);
                    responsePart.Properties.Write("UnitOfMeasure", part.Properties.ReadAsString("PAKNAME").Replace('"', ' '));
                    responsePart.Properties.Write("ItemNumber", part.Properties.Read("ARTID"));
                    responsePart.Properties.Write("VariableWeight", part.Properties.Read("MEASURE"));

                    /* Convert weight tolerances from per LUM to per package */
                    double baseQty = part.Properties.ReadAsDouble("BASEQTY");
                                        
                    if (string.IsNullOrEmpty(part.Properties.ReadAsString("MEASQTYLOTOL")))
                    {
                        responsePart.Properties.Write("VariableWeightMinimum", "000000.0000");
                    }
                    else
                    {
                        double weightMinimum = part.Properties.ReadAsDouble("MEASQTYLOTOL") * baseQty;
                        responsePart.Properties.Write("VariableWeightMinimum", weightMinimum);
                    }

                    if (string.IsNullOrEmpty(part.Properties.ReadAsString("MEASQTYUPTOL")))
                    {
                        responsePart.Properties.Write("VariableWeightMaximum", "999999.0000");
                    }
                    else
                    {
                        double weightMaximum = part.Properties.ReadAsDouble("MEASQTYUPTOL") * baseQty;
                        responsePart.Properties.Write("VariableWeightMaximum", weightMaximum);
                    }

                    responsePart.Properties.Write("QuantityPicked", "".PadLeft(5, '0'));

                    #region Check digits, product verification id and item load id

                    string checkDigits = part.Properties.ReadAsString("PPCODE");

                    if (part.Properties.ReadAsString("ITEVERIFY") == "1")
                    {
                        /* Verify pick location by item load id */

                        string verifyItemLoadId = part.Properties.ReadAsString("ITEID");

                        if (!string.IsNullOrEmpty(verifyItemLoadId))
                        {
                            if (verifyItemLoadId.Length > session.ReadAsInt("VOICE_MIN_DIGITS_ITEVERIFY"))
                                verifyItemLoadId = verifyItemLoadId.Substring(verifyItemLoadId.Length - session.ReadAsInt("VOICE_MIN_DIGITS_ITEVERIFY"), session.ReadAsInt("VOICE_MIN_DIGITS_ITEVERIFY"));
                        }

                        if ((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated)
                            || ((part.Properties.ReadAsString("ITEVERIFY") == "1") 
                            && (string.IsNullOrEmpty(part.Properties.ReadAsString("ITEID")))))
                        {
                            responsePart.Properties.Write("CheckDigits", "");
                            responsePart.Properties.Write("ScanProductId", part.Properties.ReadAsString("ITEID"));
                            responsePart.Properties.Write("SpokenProductId", "00000");
                        }
                        else
                        {
                            responsePart.Properties.Write("CheckDigits", "0");
                            responsePart.Properties.Write("ScanProductId", part.Properties.ReadAsString("ITEID"));
                            responsePart.Properties.Write("SpokenProductId", verifyItemLoadId);
                        }
                    }
                    else if (part.Properties.ReadAsString("ITEVERIFY") == "2")
                    {
                        /* Verify pick location by lot */

                        string verifyLot = part.Properties.ReadAsString("PRODLOT");

                        if (!string.IsNullOrEmpty(verifyLot))
                        {
                            if (verifyLot.Length > session.ReadAsInt("VOICE_MIN_CHARS_PRODLOTVERIFY"))
                                verifyLot = verifyLot.Substring(verifyLot.Length - session.ReadAsInt("VOICE_MIN_CHARS_PRODLOTVERIFY"), session.ReadAsInt("VOICE_MIN_CHARS_PRODLOTVERIFY"));
                        }
                        
                        responsePart.Properties.Write("CheckDigits", "0");
                        responsePart.Properties.Write("ScanProductId", part.Properties.ReadAsString("PRODLOT"));
                        responsePart.Properties.Write("SpokenProductId", verifyLot);
                    }
                    else if ((part.Properties.ReadAsString("PPVERIFY") == PPVERIFY_ARTID)
                        || (part.Properties.ReadAsString("PPVERIFY") == PPVERIFY_EANDUN))
                    {
                        /* Verify pick location by ARTID or EANDUN */

                        if (!string.IsNullOrEmpty(checkDigits))
                        {
                            if (checkDigits.Length > session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"))
                                checkDigits = checkDigits.Substring(checkDigits.Length - session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"), session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"));
                        }

                        responsePart.Properties.Write("CheckDigits", "0");

                        /* If the system hasn't found any item load dont verify anything */
                        if ((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated)
                            && (string.IsNullOrEmpty(part.Properties.ReadAsString("ITEID"))))
                        {
                            responsePart.Properties.Write("CheckDigits", "");
                        }

                        responsePart.Properties.Write("ScanProductId", checkDigits);
                        responsePart.Properties.Write("SpokenProductId", checkDigits);
                    }
                    else
                    {
                        /* Verify pick location by pick location code */

                        if (!string.IsNullOrEmpty(checkDigits))
                        {
                            if (checkDigits.Length > session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"))
                                checkDigits = checkDigits.Substring(checkDigits.Length - session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"), session.ReadAsInt("VOICE_MIN_DIGITS_PPCODE"));

                            long temp;

                            if (!long.TryParse(checkDigits, out temp))
                                checkDigits = "";
                        }

                        responsePart.Properties.Write("CheckDigits", checkDigits);
                        responsePart.Properties.Write("ScanProductId", "");
                        responsePart.Properties.Write("SpokenProductId", "");

                        /* If the system hasn't found any item load dont verify anything */
                        if (((part.Properties.ReadAsString("PBROWSTAT") == PBROWSTAT_Registrated) || (part.Properties.ReadAsString("ITETRACE") == "1"))
                            && (string.IsNullOrEmpty(part.Properties.ReadAsString("ITEID"))))
                        {
                            responsePart.Properties.Write("CheckDigits", "");
                            responsePart.Properties.Write("ScanProductId", part.Properties.ReadAsString("ITEID"));
                            responsePart.Properties.Write("SpokenProductId", "00000");
                        }

                    }
                                        
                    #endregion

                    responsePart.Properties.Write("Description", part.Properties.ReadAsString("ARTNAME1").Replace('"', ' '));
                    responsePart.Properties.Write("Size", "");

                    if (part.Properties.ReadAsString("PPVERIFY") == PPVERIFY_EANDUN)
                        responsePart.Properties.Write("UniversalProductCode", part.Properties.Read("PPCODE"));
                    else
                        responsePart.Properties.Write("UniversalProductCode", "");

                    responsePart.Properties.Write("WorkId", "1");
                    responsePart.Properties.Write("DeliveryLocation", "");
                    responsePart.Properties.Write("Dummy", "");
                    responsePart.Properties.Write("Store", "");
                    responsePart.Properties.Write("CaseLabelCheckDigit", "");

                    responsePart.Properties.Write("TargetContainer", part.Properties.ReadAsString("SEQNUM").PadLeft(session.ReadAsInt("VOICE_MIN_DIGITS_CARCODE"), '0'));

                    if (part.Properties.ReadAsString("SERIAL") == "1")
                    {
                        responsePart.Properties.Write("CaptureLot", "1");
                        responsePart.Properties.Write("LotText", GetCachedAlarmText("VOICEPICK006", session));
                    }
                    else if ((part.Properties.ReadAsString("PRODLOT_VER") == "1") 
                        && (string.IsNullOrEmpty(part.Properties.ReadAsString("PRODLOTREQ"))))
                    {
                        responsePart.Properties.Write("CaptureLot", "1");
                        responsePart.Properties.Write("LotText", GetCachedAlarmText("VOICEPICK004", session));
                    }
                    else
                    {
                        responsePart.Properties.Write("CaptureLot", "0");
                        responsePart.Properties.Write("LotText", "");
                    }
                                                           
                    responsePart.Properties.Write("PickMessage", part.Properties.ReadAsString("INSTRUCTIONS"));
                    
                    string verifyLocation = "0";

                    if (session.ReadAsString("VOICE_VERIFY_PP_BALANCE") == "1")
                    {
                        verifyLocation = "1";
                    }
                    else if ((session.ReadAsString("VOICE_VERIFY_PP_BALANCE") == "2")
                        && (status == "G"))
                    {
                        verifyLocation = "1";
                    }

                    responsePart.Properties.Write("VerifyLocation", verifyLocation);

                    responsePart.Properties.Write("CycleCount", "0");
                    responsePart.Properties.Write("CustomerId", PrTaskLUTContainer.GetCotainerHashCode(part));

                    if (part.Properties.ReadAsString("SERIAL") == "1")
                    {
                        responsePart.Properties.Write("PutLotPrompt", GetCachedAlarmText("VOICEPICK005", session));
                    }
                    else if (part.Properties.ReadAsString("PRODLOT_VER") == "1")
                    {
                        responsePart.Properties.Write("PutLotPrompt", GetCachedAlarmText("VOICEPICK003", session));
                    }
                    else
                    {
                        responsePart.Properties.Write("PutLotPrompt", "");
                    }

                    responsePart.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
                    responsePart.Properties.Write("Message", "");

                    responseMsg.Parts.Add(responsePart);

                    prevLocation  = part.Properties.ReadAsString("PPKEY");
                    prevContainer = part.Properties.ReadAsString("SEQNUM");
                }

                //Reset session values
                session.Write("PBROW_COUNT", rowCount);
                session.Write("SPLIT_PBROWID", "");
                session.Write("SPLIT_STATUS", "0");
                session.Write("PREV_PPKEY", "");
                session.Write("RESTCOD", "");
                session.Write("PRODLOT", "");
                session.Write("PRODLOTQTY", 0d);
            }
            catch (WarehouseAdapterException ex)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(35);
                part.Properties.Write("ErrorCode", WarehouseAlarm);
                part.Properties.Write("Message", ex.Message);
                responseMsg.Parts.Add(part);

                throw;
            }
            catch (Exception)
            {
                responseMsg.Parts.Clear();
                MessagePart part = CreateEmptyMessagePart(35);
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
