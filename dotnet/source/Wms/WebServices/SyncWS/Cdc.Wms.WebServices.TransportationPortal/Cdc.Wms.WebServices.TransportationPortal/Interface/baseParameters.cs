using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    public partial class baseParameters
    {
        public string clientId;

        public bool returnDetails;

        public System.Nullable<int> firstResult;

        public System.Nullable<int> maxResult;

        public string stockNo;

        public string nlangcod;

        public baseParameters()
        {
            this.returnDetails = false;
        }
    }
}
