using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts/2011/09")]
    public class OutputManager
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string OutputManagerIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string OutputManagerName { get; set; }
    }


    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts/2011/09")]
    public class OutputManagerCollection : List<OutputManager>
    {
        public OutputManagerCollection()
        {
        }

        public OutputManagerCollection(IEnumerable<OutputManager> collection)
            : base(collection)
        {
        }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts/2011/09")]
    public class FindOutputManagerResult
    {
        
        [DataMember(Order = 1, IsRequired = true)]
        public OutputManagerCollection OutputManagers { get; set; }
                
    }
}
