using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Imi.Wms.WebServices.SyncWS.Interface.OrderPortal
{
    [WebService(Namespace = "http://im.se/wms/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class OrderPortalService : System.Web.Services.WebService
    {
        public OrderPortalService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string WhoAmI()
        {
            //string s = Imi.Framework.Shared.CurrentVersion.VersionName;
            //return s;
            return null;
        }

        [WebMethod]
        public CustomerOrderHeadInfoDoc GetCustomerOrderHeadInfo(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderHeadInfo(PartnerName, LanguageId, ClientIdentity, OrderNumber, OrderSequence);
        }

        [WebMethod]
        public CustomerOrderInfoDoc GetCustomerOrderInfo(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderInfo(PartnerName, LanguageId, ClientIdentity, OrderNumber, OrderSequence);
        }

        [WebMethod]
        public CustomerOrderLineRangeDoc GetCustomerOrderLineRange(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence,
          Nullable<int> FirstLinePosition,
          Nullable<int> LastLinePosition)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderLineRange(PartnerName, LanguageId, ClientIdentity, OrderNumber, OrderSequence, FirstLinePosition, LastLinePosition);
        }
    }
}