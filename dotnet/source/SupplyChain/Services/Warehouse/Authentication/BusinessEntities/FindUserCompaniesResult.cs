using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessEntities
{
    public class FindUserCompaniesResult
    {
        public string CompanyIdentity { get; set; }
        public string CompanyName { get; set; }
        public bool? IsClientInterfaceWebServices { get; set; }
        public bool? IsClientInterfaceHAPI { get; set; }
        public bool? IsClientInterfaceEDI { get; set; }
        public string WarehouseIdentity { get; set; }
    }
}
