using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Services
{
    public class OMSLogicalUserData
    {
        public OMSLogicalUserData(string pLoginId, string pUserId, decimal pWarehouseNumber,
            decimal pLegalEntity, string pUserName, string pOrgUnit, string pEmployNumber)
        {
            omsLoginUserId = pLoginId;
            omsLogicalUserId = pUserId;
            warehouseNumber = pWarehouseNumber;
            legalEntity = pLegalEntity;
            userName = pUserName;
            orgUnit = pOrgUnit;
            employNumber = pEmployNumber;
        }
        public string omsLoginUserId;
        public string omsLogicalUserId;
        public decimal warehouseNumber;
        public decimal legalEntity;
        public string userName;
        public string orgUnit;
        public string envVariables;
        public string omsLangCode;
        public string employNumber;
    }
}
