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
using Imi.Framework.Versioning;

namespace Imi.Wms.WebServices.ExternalInterface
{
    [WebService(Namespace = "http://im.se/wms/webservices/", Description = "WMS Inbound (Receiver) interface 5.0.18")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class InboundInterface : WSBase
    {
        public string _Debug()
        {
            return "Generated on   : 2005-09-16 11:44:06\r\n" +
                   "Generated by   : IMINT1\\olla@IMIPC1091\r\n" +
                   "Generated in   : C:\\project\\views\\olla_temp_ss\\client\\C#\\wscc\r\n";
        }

        private void Log(bool Enter)
        {

            string path = @"C:\log\Inbound.log";

            using (StreamWriter w = File.AppendText(path))
            {
                w.Write("{0} {1}.{2}", System.DateTime.Now.ToShortDateString(),
                  System.DateTime.Now.ToLongTimeString(), System.Convert.ToString(System.DateTime.Now.Millisecond));

                StackTrace st = new StackTrace(1, true);

                if (st.FrameCount > 1)
                {
                    StackFrame sf = st.GetFrame(1);

                    w.Write("{0}{1}{2}", '\t', sf.GetMethod(), '\t');
                }

                if (Enter)
                    w.WriteLine("Enter");
                else
                    w.WriteLine("Leave");
            }
        }

        private void EnterProc()
        {
            try
            {
                // Log( true );
            }
            catch
            {
            }
        }

        private void ExitProc()
        {
            try
            {
                // Log( false );
            }
            catch
            {
            }
        }

        [WebMethod]
        public string WhoAmI()
        {
            EnterProc();

            string s = "5.0.18 interface running in "+  CurrentVersion.VersionName;

            ExitProc();

            return s;
        }


        [WebMethod]
        public void ProductStockGroup(string ChannelId, string ChannelRef, string TransactionId, ProductStockGroupDoc aProductStockGroupDoc)
        {
            EnterProc();

            ProductStockGroupInsert aProductStockGroupHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "PSG");

                try
                {
                    aProductStockGroupHandler = new ProductStockGroupInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aProductStockGroupDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aProductStockGroupHandler.Process(ref mt, null, aProductStockGroupDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void Product(string ChannelId, string ChannelRef, string TransactionId, ProductDoc aProductDoc)
        {
            EnterProc();

            ProductInsert aProductHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "ART");

                try
                {
                    aProductHandler = new ProductInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aProductDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aProductHandler.Process(ref mt, null, aProductDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void Party(string ChannelId, string ChannelRef, string TransactionId, PartyDoc aPartyDoc)
        {
            EnterProc();

            PartyInsert aPartyHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "PA");

                try
                {
                    aPartyHandler = new PartyInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aPartyDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aPartyHandler.Process(ref mt, null, aPartyDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void PurchaseOrder(string ChannelId, string ChannelRef, string TransactionId, PurchaseOrderHeadDoc aPurchaseOrderHeadDoc)
        {
            EnterProc();

            PurchaseOrderHeadInsert aPurchaseOrderHeadHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "PO");

                try
                {
                    aPurchaseOrderHeadHandler = new PurchaseOrderHeadInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aPurchaseOrderHeadDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aPurchaseOrderHeadHandler.Process(ref mt, null, aPurchaseOrderHeadDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void CustomerReturnOrder(string ChannelId, string ChannelRef, string TransactionId, CustomerReturnOrderHeadDoc aCustomerReturnOrderHeadDoc)
        {
            EnterProc();

            CustomerReturnOrderHeadInsert aCustomerReturnOrderHeadHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "CRO");

                try
                {
                    aCustomerReturnOrderHeadHandler = new CustomerReturnOrderHeadInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aCustomerReturnOrderHeadDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aCustomerReturnOrderHeadHandler.Process(ref mt, null, aCustomerReturnOrderHeadDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void CustomerOrder(string ChannelId, string ChannelRef, string TransactionId, CustomerOrderHeadDoc aCustomerOrderHeadDoc)
        {
            EnterProc();

            CustomerOrderHeadInsert aCustomerOrderHeadHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "CO");

                try
                {
                    aCustomerOrderHeadHandler = new CustomerOrderHeadInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aCustomerOrderHeadDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aCustomerOrderHeadHandler.Process(ref mt, null, aCustomerOrderHeadDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void VendorReturnOrder(string ChannelId, string ChannelRef, string TransactionId, VendorReturnOrderHeadDoc aVendorReturnOrderHeadDoc)
        {
            EnterProc();

            VendorReturnOrderHeadInsert aVendorReturnOrderHeadHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "VRO");

                try
                {
                    aVendorReturnOrderHeadHandler = new VendorReturnOrderHeadInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aVendorReturnOrderHeadDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aVendorReturnOrderHeadHandler.Process(ref mt, null, aVendorReturnOrderHeadDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void ASN(string ChannelId, string ChannelRef, string TransactionId, ASNHeadDoc aASNHeadDoc)
        {
            EnterProc();

            ASNHeadInsert aASNHeadHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "NTR");

                try
                {
                    aASNHeadHandler = new ASNHeadInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aASNHeadDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aASNHeadHandler.Process(ref mt, null, aASNHeadDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void InventoryStatus(string ChannelId, string ChannelRef, string TransactionId, InventoryStatusDoc aInventoryStatusDoc)
        {
            EnterProc();

            InventoryStatusInsert aInventoryStatusHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "IS");

                try
                {
                    aInventoryStatusHandler = new InventoryStatusInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aInventoryStatusDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aInventoryStatusHandler.Process(ref mt, null, aInventoryStatusDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void StockTakingOrder(string ChannelId, string ChannelRef, string TransactionId, StocktakingOrderDoc aStocktakingOrderDoc)
        {
            EnterProc();

            StocktakingOrderInsert aStocktakingOrderHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "STO");

                try
                {
                    aStocktakingOrderHandler = new StocktakingOrderInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aStocktakingOrderDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aStocktakingOrderHandler.Process(ref mt, null, aStocktakingOrderDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }


        [WebMethod]
        public void BalanceQuery(string ChannelId, string ChannelRef, string TransactionId, BalanceQueryDoc aBalanceQueryDoc)
        {
            EnterProc();

            BalanceQueryInsert aBalanceQueryHandler;

            try
            {
                MessageTransaction mt = BeginWebmethod(ChannelId, ChannelRef, TransactionId, "BQ");

                try
                {
                    aBalanceQueryHandler = new BalanceQueryInsert(this);
                }
                catch (Exception e)
                {
                    Exception InternalError = new Exception("InternalError: Building insert handler", e);
                    throw (InternalError);
                }

                try
                {
                    if (aBalanceQueryDoc == null)
                    {
                        Exception InternalError = new Exception("DataError: Root object cannot be null");
                        throw (InternalError);
                    }
                    aBalanceQueryHandler.Process(ref mt, null, aBalanceQueryDoc);
                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            ExitProc();

            return;
        }

    }
}
