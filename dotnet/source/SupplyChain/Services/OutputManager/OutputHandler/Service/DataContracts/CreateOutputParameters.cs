using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OM.Services.OutputHandler.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class CreateOutputParameters
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string OutputJobIdentity { get; set; }

        [DataMember(Order = 1, IsRequired = true)]
        public int OutputJobSequenceNumber { get; set; }
 
        [DataMember(Order = 2, IsRequired = true)]
        public string OutputXML { get; set; }
    }
}
