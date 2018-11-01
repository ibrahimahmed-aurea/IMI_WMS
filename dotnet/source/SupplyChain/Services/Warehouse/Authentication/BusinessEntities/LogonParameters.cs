using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessEntities
{
    public class LogonParameters
    {
        public string UserIdentity { get; set; }
        public string WarehouseIdentity { get; set; }
        public string CompanyIdentity { get; set; }
        public string TerminalIdentity { get; set; }
        public string ApplicationIdentity { get; set; }
    }
}
