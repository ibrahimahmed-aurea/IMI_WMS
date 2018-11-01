using System;
using System.Data;
using System.Collections;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Customer
{
    public partial class WebServices3pl : IPackage
    {
        public addressSearchResult getAddresses(ISyncWSParameter dataParam, string clientId, string customerId)
        {
            IDataReader reader = null;
            int totalRows;
            addressSearchResult res = new addressSearchResult();

            Getaddresses(
                clientId,
                customerId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new address(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(address)) as address[];
            res.SetTotalRows(totalRows);

            return res;
        }
        public addressSearchResult getPartyAddress(ISyncWSParameter dataParam, string clientId, string partyId, partyType pt)
        {
            IDataReader reader = null;
            int totalRows;
            addressSearchResult res = new addressSearchResult();

            Getpartyaddress(partyId, ConvertPartyType(pt), clientId, out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new address(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(address)) as address[];
            res.SetTotalRows(totalRows);

            return res;
        }
        public customerSearchResult findCustomerById(ISyncWSParameter dataParam, string clientId, string customerId)
        {
            IDataReader reader = null;
            int totalRows;
            customerSearchResult res = new customerSearchResult();

            Findcustomersbyid(
                clientId,
                customerId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new customer(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(customer)) as customer[];
            res.SetTotalRows(totalRows);

            return res;
        }
        public partySearchResult findPartyById(ISyncWSParameter dataParam, string clientId, string partyId, partyType pt)
        {
            IDataReader reader = null;
            int totalRows;
            partySearchResult res = new partySearchResult();

            string partyQualifier = ConvertPartyType(pt);

            Findpartybyid(partyId, partyQualifier, clientId, out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new customer(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(customer)) as customer[];
            res.SetTotalRows(totalRows);

            return res;
        }

        private string ConvertPartyType(partyType pt)
        {
            string partyQualifier;
            switch (pt)
            {
                case partyType.CA:
                    partyQualifier = "CA";
                    break;
                case partyType.CL:
                    partyQualifier = "CL";
                    break;
                case partyType.CU:
                    partyQualifier = "CU";
                    break;
                case partyType.GO:
                    partyQualifier = "GO";
                    break;
                case partyType.OT:
                    partyQualifier = "OT";
                    break;
                case partyType.SU:
                    partyQualifier = "SU";
                    break;
                case partyType.WH:
                    partyQualifier = "WH";
                    break;
                default:
                    throw new Exception("Party type not supported");
            }
            return partyQualifier;
        }
    }
}