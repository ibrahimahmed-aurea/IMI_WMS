/*
  File           : 

  Description    : Public interface class for WebService interface for inbound data.
                   This code was generated, do not edit.

*/
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
//using Imi.Framework.Shared;

using Imi.Wms.WebServices.ExternalInterface;
using System.Text;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
    [WebService(Namespace = "http://im.se/wms/webservices/", Description = "WMS Outbound (Sender) interface")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ReceiveInterface : System.Web.Services.WebService
    {

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        [WebMethod]
        public string WhoAmI()
        {
            string s = "Outbound Mapper";
            return s;
        }

        [WebMethod]
        public void TestDeliveryReceipt()
        {
            DeliveryReceiptHeadDoc dr = new DeliveryReceiptHeadDoc();
            dr.EmployeeIdentity = "Tester";
            dr.aDeliveryReceiptLineDocs = new DeliveryReceiptLineDoc[1];
            dr.aDeliveryReceiptLineDocs[0] = new DeliveryReceiptLineDoc();
            dr.aDeliveryReceiptLineDocs[0].PurchaseOrderNumber = "PO123";
            int transactionid = new Random().Next();
            string transid = Convert.ToString(transactionid);
            DeliveryReceipt("TestChannel", "TestChannelRef", transid, dr);
        }

        [WebMethod]
        public void TestPickReceipt()
        {
            PickReceiptHeadDoc pr = new PickReceiptHeadDoc();
            pr.WarehouseIdentity = "TEST";
            pr.CustomerOrderNumber = "CO123";
            pr.aPickReceiptLineDocs = new PickReceiptLineDoc[1];
            pr.aPickReceiptLineDocs[0] = new PickReceiptLineDoc();
            pr.aPickReceiptLineDocs[0].CustomerOrderNumber = pr.CustomerOrderNumber;
            pr.aPickReceiptLineDocs[0].CustomerOrderLinePosition = 10;
            pr.aPickReceiptLineDocs[0].ShipDate = DateTime.Now;
            pr.aPickReceiptLineDocs[0].ProductIdentity = "åäöprod";
            pr.aPickReceiptLineDocs[0].OrderedQuantity = 1234.567;
            pr.aPickReceiptLineDocs[0].PickQuantity = 1233.567;
            int transactionid = new Random().Next();
            string transid = Convert.ToString(transactionid);
            PickReceipt("TestChannel", "TestChannelRef", transid, pr);
        }

        [WebMethod]
        public void TestInventoryChange()
        {
            InventoryChangeLineDoc ic = new InventoryChangeLineDoc();
            ic.EmployeeId = "Tester åäö\"";
            //ic.FreeText = char [] {
            UnicodeEncoding unicode = new UnicodeEncoding();
            Char[] chars = new Char[] { 'u', 'n', 'i', '\uFE94', 'c' };
            Byte[] bytes = unicode.GetBytes(chars);
            ic.FreeText = unicode.GetString(bytes);

            //InventoryChange("TestChannel", "TestChannelRef", Guid.NewGuid().ToString(), ic);
            int transactionid = new Random().Next();
            string transid = Convert.ToString(transactionid);
            InventoryChange("TestChannel", "TestChannelRef", transid, ic);
        }

        [WebMethod]
        public void TestASN()
        {
            ASNHeadDoc asn = new ASNHeadDoc();
            asn.Instructions = "Instructions tester";
            int transactionid = new Random().Next();
            string transid = Convert.ToString(transactionid);
            ASN("TestChannel", "TestChannelRef", transid, asn);
        }

        [WebMethod]
        public void TestBalanceAnswer()
        {
            BalanceAnswerLineDoc ba = new BalanceAnswerLineDoc();
            ba.FreeQuantity = 456;
            ba.PickLocationQuantity = 100;
            ba.TopickQuantity = 123;
            ba.ProductNumber = "1234567890";
            int transactionid = new Random().Next();
            string transid = Convert.ToString(transactionid);
            BalanceAnswer("TestChannel", "TestChannelRef", transid, ba);
        }

        [WebMethod]
        public void DeliveryReceipt(string ChannelId, string ChannelRef, string TransactionId, DeliveryReceiptHeadDoc aDeliveryReceiptHeadDoc)
        {
            DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
            deliveryReceipt.aDeliveryReceiptHeadDoc = aDeliveryReceiptHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "DR");
                forwardHelper.Forward(deliveryReceipt);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void PickReceipt(string ChannelId, string ChannelRef, string TransactionId, PickReceiptHeadDoc aPickReceiptHeadDoc)
        {
            PickReceipt pickReceipt = new PickReceipt();
            pickReceipt.aPickReceiptHeadDoc = aPickReceiptHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "PR");
                forwardHelper.Forward(pickReceipt);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }


        [WebMethod]
        public void VendorReturnReceipt(string ChannelId, string ChannelRef, string TransactionId, ReturnReceiptHeadDoc aReturnReceiptHeadDoc)
        {
            VendorReturnReceipt vendorReturnReceipt = new VendorReturnReceipt();
            vendorReturnReceipt.aReturnReceiptHeadDoc = aReturnReceiptHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "RR");
                forwardHelper.Forward(vendorReturnReceipt);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void InspectionReceipt(string ChannelId, string ChannelRef, string TransactionId, InspectionReceiptHeadDoc aInspectionReceiptHeadDoc)
        {
            InspectionReceipt inspectionReceipt = new InspectionReceipt();
            inspectionReceipt.aInspectionReceiptHeadDoc = aInspectionReceiptHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "IR");
                forwardHelper.Forward(inspectionReceipt);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void InventoryChange(string ChannelId, string ChannelRef, string TransactionId, InventoryChangeLineDoc aInventoryChangeLineDoc)
        {
            InventoryChange inventoryChange = new InventoryChange();
            inventoryChange.aInventoryChangeLineDoc = aInventoryChangeLineDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "IC");
                forwardHelper.Forward(inventoryChange);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void BalanceAnswer(string ChannelId, string ChannelRef, string TransactionId, BalanceAnswerLineDoc aBalanceAnswerLineDoc)
        {
            BalanceAnswer balanceAnswer = new BalanceAnswer();
            balanceAnswer.aBalanceAnswerLineDoc = aBalanceAnswerLineDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "BA");
                forwardHelper.Forward(balanceAnswer);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }


        [WebMethod]
        public void ReturnedPackingMaterial(string ChannelId, string ChannelRef, string TransactionId, ReturnedPackingMaterialHeadDoc aReturnedPackingMaterialHeadDoc)
        {
            ReturnedPackingMaterial returnedPackingMaterial = new ReturnedPackingMaterial();
            returnedPackingMaterial.aReturnedPackingMaterialHeadDoc = aReturnedPackingMaterialHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "PM");
                forwardHelper.Forward(returnedPackingMaterial);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void ASN(string ChannelId, string ChannelRef, string TransactionId, ASNHeadDoc aASNHeadDoc)
        {
            ASN asn = new ASN();
            asn.aASNHeadDoc = aASNHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "SR");
                forwardHelper.Forward(asn);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void ConfirmationOfReceipt(string ChannelId, string ChannelRef, string TransactionId, ConfirmationOfReceiptHeadDoc aConfirmationOfReceiptHeadDoc)
        {
            ConfirmationOfReceipt confirmationOfReceipt = new ConfirmationOfReceipt();
            confirmationOfReceipt.aConfirmationOfReceiptHeadDoc = aConfirmationOfReceiptHeadDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "COR");
                forwardHelper.Forward(confirmationOfReceipt);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void InboundOrderCompleted(string ChannelId, string ChannelRef, string TransactionId, InboundOrderCompletedDoc aInboundOrderCompletedDoc)
        {
            InboundOrderCompleted inboundOrderCompleted = new InboundOrderCompleted();
            inboundOrderCompleted.aInboundOrderCompletedDoc = aInboundOrderCompletedDoc;

            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "IOC");
                forwardHelper.Forward(inboundOrderCompleted);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }

        [WebMethod]
        public void TransportInstruction(string ChannelId, string ChannelRef, string TransactionId, TransportInstructionDoc aTransportInstructionDoc)
        {
            TransportInstruction transportInstruction = new TransportInstruction();
            transportInstruction.aTransportInstructionDoc = aTransportInstructionDoc;
            ForwardHelper forwardHelper = new ForwardHelper();

            try
            {
                forwardHelper.CreateContext(ChannelId, ChannelRef, TransactionId, "TI");
                forwardHelper.Forward(transportInstruction);
            }
            catch (Exception e)
            {
                try
                {
                    forwardHelper.Abort();
                }
                catch
                {
                    // ignore exceptions in exception handler
                }
                Exception InternalError = new Exception("DataError: Error processing data", e);
                throw (InternalError);
            }
            finally
            {
                forwardHelper.ReleaseContext();
            }

            return;
        }
    }
}
