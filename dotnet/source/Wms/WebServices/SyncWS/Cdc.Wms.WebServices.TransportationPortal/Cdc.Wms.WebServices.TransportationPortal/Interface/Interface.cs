using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    public partial class internalFindDepartureRouteResult : searchResult
    {
        public Route[] list;

        public void SetTotalRows(Nullable<int> totalRows)
        {
            if (totalRows != null)
            {
                totalHits = (int)totalRows;
            }
            else
            {
                totalHits = -1;
            }
        }

        public Nullable<int> GetTotalRows()
        {
            return totalHits;
        }
    }
}
