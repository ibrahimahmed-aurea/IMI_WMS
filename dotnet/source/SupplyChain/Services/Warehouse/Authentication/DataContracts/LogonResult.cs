using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class LogonResult
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string AlarmId { get; set; }
    }
       
}
