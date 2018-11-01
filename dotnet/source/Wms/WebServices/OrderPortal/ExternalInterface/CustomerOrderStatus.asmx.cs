using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Imi.Wms.WebServices.OrderPortal.ExternalInterface
{
    [WebService(Namespace = "http://im.se/wms/webservices/orderportal", Description = "IMI WMS Order Portal")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class InboundInterface : System.Web.Services.WebService
    {
        public InboundInterface()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string WhoAmI()
        {
            string s = Imi.Framework.Shared.CurrentVersion.VersionName;
            return s;
        }

        [WebMethod]
        public CustomerOrderHeadInfoDoc GetCustomerOrderHeadInfo(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderHeadInfo(ChannelId, LanguageId, ClientIdentity, OrderNumber, OrderSequence);
        }

        [WebMethod]
        public CustomerOrderInfoDoc GetCustomerOrderInfo(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderInfo(ChannelId, LanguageId, ClientIdentity, OrderNumber, OrderSequence);
        }

        [WebMethod]
        public CustomerOrderLineRangeDoc GetCustomerOrderLineRange(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence,
          Nullable<int> FirstLinePosition,
          Nullable<int> LastLinePosition)
        {
            OrderPortal op = new OrderPortal();

            return op.GetCustomerOrderLineRange(ChannelId, LanguageId, ClientIdentity, OrderNumber, OrderSequence, FirstLinePosition, LastLinePosition);
        }
    }
}
