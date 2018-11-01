using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
/*    public partial class baseParameters : ISyncWSParameter
    {
        public bool GetReturnDetails()
        {
            return returnDetails;
        }

        public Nullable<int> GetSkipNoFirstRows()
        {
            return firstResult;
        }

        public Nullable<int> GetMaxRows()
        {
            return maxResult;
        }
    }
*/

    public partial class internalFindDepartureRouteResult : ISyncWSResult
    {
    }

    /*public partial class internalFindDepartureRouteStopsResult : searchResult, ISyncWSResult
    {
        public RouteStop[] list;

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
    }*/

    public partial class internalFindDepartureRouteSearchParameters : baseParameters, ISyncWSParameter
    {
        public string DepartureIdentity;

        public bool GetReturnDetails()
        {
            return false;
        }

        public int? GetSkipNoFirstRows()
        {
            return null;
        }

        public int? GetMaxRows()
        {
            return null;
        }
    }
}
