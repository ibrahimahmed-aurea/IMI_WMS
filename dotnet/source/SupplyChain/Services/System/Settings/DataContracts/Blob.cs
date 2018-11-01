// Generated from template: .\DataContracts\DataContractTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Services.Settings.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Services.Settings.DataContracts/2011/09", IsReference = false)]
    public class Blob
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string Name { get; set; }
		
        [DataMember(Order = 2, IsRequired = false)]
        public DateTime LastModified { get; set; }
		
        [DataMember(Order = 3, IsRequired = false)]
        public BlobMetaDataCollection MetaData { get; set; }
		
        [DataMember(Order = 4, IsRequired = false)]
        public string Data { get; set; }
		

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "Name", Name));
            result.Append(string.Format("\r\n{0}={1}", "LastModified", LastModified));
            result.Append(string.Format("\r\n{0}={1}", "MetaData", MetaData));
            result.Append(string.Format("\r\n{0}={1}", "Data", Data));
            return result.ToString();
        }	
    }
}
