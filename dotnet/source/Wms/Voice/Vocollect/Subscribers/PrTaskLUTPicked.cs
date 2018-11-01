using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Voice.Vocollect;

namespace Imi.Wms.Voice.Vocollect.Subscribers
{
    /// <summary>
    /// Subscriber for PrTaskLUTPicked
    /// </summary>
    public class PrTaskLUTPicked : VocollectSubscriber
    {
        private const int NoOperation = 0;
        private const int Split = 1;
        private const int Exchange = 2;
        private const int ExchangeFollowingLines = 3;
        private const int ErrorEnterDiscrepancyCode = 3;
        private const int ErrorSplitExchangeNotAllowed = 101;
        private const int ErrorEnterQuantity = 2;
        
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

            responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeNoError);
            responseMsg.Properties.Write("Message", "");
                        
            try
            {
                /* Check if production lots should be registered */
                if ((msg.Properties.ReadAsString("PickedStatus") == "1") && (session.ProductionLots.Count > 0))
                {
                    double pickedQty = msg.Properties.ReadAsDouble("QuantityPicked");
                                        
                    foreach (ProductionLot lot in session.ProductionLots)
                    {
                        session.Write("PRODLOT", lot.LotId);
                        session.Write("PRODLOTQTY", lot.Quantity);

                        msg.Properties.Write("QuantityPicked", pickedQty);

                        /* Register production lot and acknowledge the line */
                        ProcessPickedMessage(msg, session);
                                                
                        /* Decrease picked quantity with registered lot qty */
                        pickedQty -= lot.Quantity;
                    }

                    session.ProductionLots.Clear();
                }
                else
                {
                    ProcessPickedMessage(msg, session);
                }
            }
            catch (WarehouseAdapterException ex)
            {
                if (ex.AlarmId == "PBROW038")
                {
                    responseMsg.Properties.Write("ErrorCode", ErrorEnterDiscrepancyCode);
                    responseMsg.Properties.Write("Message", ex.Message);
                }
                else if ((ex.AlarmId == "PBCAR012") 
                    || (ex.AlarmId == "PBCAR048")
                    || (ex.AlarmId == "PBROW011")
                    || (ex.AlarmId == "PBROW013"))
                {
                    responseMsg.Properties.Write("ErrorCode", ErrorSplitExchangeNotAllowed);
                    responseMsg.Properties.Write("Message", ex.Message);
                }
                else if (ex.AlarmId == "VOICEPICK011")
                {
                    /* Error meassage plus re-enter quantity */
                    responseMsg.Properties.Write("ErrorCode", ErrorEnterQuantity);
                    responseMsg.Properties.Write("Message", ex.Message + "," + GetCachedAlarmText("VOICEPICK007", session));
                }
                else
                {
                    responseMsg.Properties.Write("ErrorCode", VocollectErrorCodeCritialError);
                    responseMsg.Properties.Write("Message", ex.Message);

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

        public static void ProcessPickedMessage(MultiPartMessage msg, VocollectSession session)
        {
            CorrelationContext context;

            using (TransactionScope transactionScope = new TransactionScope())
            {
                #region Exchange Container logic

                if ((msg.Properties.ReadAsString("SplitExchange") == "2")
                    || (msg.Properties.ReadAsString("SplitExchange") == "3"))
                {
                    /* Exchange line to new PLC */
                    MultiPartMessage exchangeMsg = CreateRequestMessage("wlvoicepick", "change_loadcarrier", session);
                    exchangeMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    exchangeMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    exchangeMsg.Properties.Write("PBROWID_I", msg.Properties.ReadAsString("Sequence"));
                    exchangeMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));

                    if (msg.Properties.ReadAsString("SplitExchange") == "3")
                        exchangeMsg.Properties.Write("CHANGEREST_I", "1");
                    else
                        exchangeMsg.Properties.Write("CHANGEREST_I", "0");

                    exchangeMsg.Properties.Write("ALMID_O", "");

                    MessageEngine.Instance.TransmitRequestMessage(exchangeMsg, exchangeMsg.MessageId, out context);

                    transactionScope.Complete();

                    return;
                }

                #endregion

                #region Split Line logic

                if ((msg.Properties.ReadAsString("SplitExchange") == "1") && (session.ReadAsString("SPLIT_STATUS") == "0"))
                {
                    /* Ignore first msg during split process for original PLC */
                    session.Write("SPLIT_STATUS", "1");
                    session.Write("SPLIT_PBROWID", msg.Properties.ReadAsString("Sequence"));

                    return;
                }
                else if ((msg.Properties.ReadAsString("SplitExchange") == "0") && (session.ReadAsString("SPLIT_STATUS") == "1"))
                {
                    /* Second msg during split process for original PLC */
                    MultiPartMessage splitMsg = CreateRequestMessage("wlvoicepick", "split_pickorderrow", session);
                    splitMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    splitMsg.Properties.Write("PBROWID_I", session.ReadAsString("SPLIT_PBROWID"));
                    splitMsg.Properties.Write("PICKQTY_I", msg.Properties.ReadAsDouble("QuantityPicked"));
                    splitMsg.Properties.Write("PBROWID_O", "");
                    splitMsg.Properties.Write("ALMID_O", "");

                    MessageEngine.Instance.TransmitRequestMessage(splitMsg, splitMsg.MessageId, out context);

                    msg.Properties.Write("Sequence", session.ReadAsString("SPLIT_PBROWID"));

                    session.Write("SPLIT_PBROWID", context.ResponseMessages[0].Properties.ReadAsString("PBROWID_O"));
                    session.Write("SPLIT_STATUS", "2");

                    msg.Properties.Write("PickedStatus", "1");
                }
                else if ((msg.Properties.ReadAsString("SplitExchange") == "0")
                    && (session.ReadAsString("SPLIT_STATUS") == "2")
                    && (msg.Properties.ReadAsString("PickedStatus") == "0"))
                {
                    /* Check for discrepancy during split process for new PLC */

                    msg.Properties.Write("Sequence", session.ReadAsString("SPLIT_PBROWID"));
                }
                else if ((msg.Properties.ReadAsString("SplitExchange") == "0")
                    && (session.ReadAsString("SPLIT_STATUS") == "2")
                    && (msg.Properties.ReadAsString("PickedStatus") == "1"))
                {
                    /* Final msg during split process for new PLC */

                    /* Move splitted line to new PLC */
                    MultiPartMessage exchangeMsg = CreateRequestMessage("wlvoicepick", "change_loadcarrier", session);
                    exchangeMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                    exchangeMsg.Properties.Write("PBHEADID_I", session.ReadAsString("PBHEADID"));
                    exchangeMsg.Properties.Write("PBROWID_I", session.ReadAsString("SPLIT_PBROWID"));
                    exchangeMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                    exchangeMsg.Properties.Write("CHANGEREST_I", "0");
                    exchangeMsg.Properties.Write("ALMID_O", "");

                    MessageEngine.Instance.TransmitRequestMessage(exchangeMsg, exchangeMsg.MessageId, out context);

                    msg.Properties.Write("Sequence", session.ReadAsString("SPLIT_PBROWID"));

                    session.Write("SPLIT_STATUS", "0");
                    session.Write("SPLIT_PBROWID", "");
                }
                else if ((msg.Properties.ReadAsString("SplitExchange") == "1") && (session.ReadAsString("SPLIT_STATUS") == "2"))
                {
                    msg.Properties.Write("Sequence", session.ReadAsString("SPLIT_PBROWID"));

                    session.Write("SPLIT_STATUS", "1");
                }

                #endregion

                MultiPartMessage whMsg = CreateRequestMessage("wlvoicepick", "modify_pickorderrow", session);
                whMsg.Properties.Write("TERID_I", session.ReadAsString("TERID"));
                whMsg.Properties.Write("PBROWID_I", msg.Properties.ReadAsString("Sequence"));

                double pickedQuantity = msg.Properties.ReadAsDouble("QuantityPicked");
                string discrepancyCode = msg.Properties.ReadAsString("DiscrepancyCode");

                #region Discrepancy code logic for strike through

                if (discrepancyCode == "00")
                    discrepancyCode = "";

                if ((pickedQuantity == 0)
                    && (msg.Properties.ReadAsString("LocationId") == session.ReadAsString("PREV_PPKEY"))
                    && (string.IsNullOrEmpty(discrepancyCode)))
                {
                    discrepancyCode = session.ReadAsString("RESTCOD");
                }

                #endregion

                #region Catch Measure logic

                double measQty = 0;

                if (session.CatchMeasureEntries.Count > 0)
                {
                    for (int i = 0; i < pickedQuantity; i++)
                        measQty += session.CatchMeasureEntries[session.CatchMeasureEntries.Count - 1 - i].Measurement;
                }

                #endregion

                whMsg.Properties.Write("PICKQTY_I", pickedQuantity);
                whMsg.Properties.Write("RESTCOD_I", discrepancyCode);
                whMsg.Properties.Write("MEASQTY_I", measQty);
                whMsg.Properties.Write("PRODLOT_I", session.ReadAsString("PRODLOT"));
                whMsg.Properties.Write("PRODLOTQTY_I", session.ReadAsDouble("PRODLOTQTY"));
                whMsg.Properties.Write("EMPID_I", session.ReadAsString("EMPID"));
                whMsg.Properties.Write("SEQNUM_I", TrimContainerId(msg.Properties.ReadAsString("ContainerId")));
                whMsg.Properties.Write("Acknowledge_I", msg.Properties.ReadAsString("PickedStatus"));
                whMsg.Properties.Write("ALMID_O", "");

                MessageEngine.Instance.TransmitRequestMessage(whMsg, whMsg.MessageId, out context);

                transactionScope.Complete();

                /* Save data in session about current pick */
                session.Write("PREV_PPKEY", msg.Properties.ReadAsString("LocationId"));
                session.Write("RESTCOD", discrepancyCode);
                session.Write("PBROWID", msg.Properties.ReadAsString("Sequence"));
                session.Write("PRODLOT", "");
                session.Write("PRODLOTQTY", 0d);

                if (session.CatchMeasureEntries.Count > 0)
                {
                    for (int i = 0; i < pickedQuantity; i++)
                        session.CatchMeasureEntries.RemoveAt(session.CatchMeasureEntries.Count - 1);
                }
            }
        }
    }
}
