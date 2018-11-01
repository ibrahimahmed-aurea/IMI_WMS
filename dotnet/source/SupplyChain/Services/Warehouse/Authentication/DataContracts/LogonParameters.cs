using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class LogonParameters
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string UserIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public string WarehouseIdentity { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string CompanyIdentity { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public string TerminalIdentity { get; set; }

        [DataMember(Order = 5, IsRequired = true)]
        public string ApplicationIdentity { get; set; }

    }
}
