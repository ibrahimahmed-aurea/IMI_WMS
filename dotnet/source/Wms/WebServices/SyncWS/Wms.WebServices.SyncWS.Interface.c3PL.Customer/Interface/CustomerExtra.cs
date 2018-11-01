using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Customer
{
    public partial class customerSearchResult : searchResult
    {
        public customer[] list;
    }
    public partial class partySearchResult : searchResult
    {
        public customer[] list;
    }
    public partial class addressSearchResult : searchResult
    {
        public address[] list;
    }
}
