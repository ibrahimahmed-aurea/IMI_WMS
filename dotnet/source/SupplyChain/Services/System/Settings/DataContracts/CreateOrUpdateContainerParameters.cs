// Generated from template: .\DataContracts\DataContractTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Services.Settings.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Services.Settings.DataContracts/2011/09", IsReference = false)]
    public class CreateOrUpdateContainerParameters
    {
        [DataMember(Order = 1, IsRequired = false)]
        public Container Container { get; set; }
		

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "Container", Container));
            return result.ToString();
        }	
    }
}
