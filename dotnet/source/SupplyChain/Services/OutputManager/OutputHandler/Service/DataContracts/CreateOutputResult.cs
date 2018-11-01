using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OM.Services.OutputHandler.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class CreateOutputResult
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string OutputJobIdentity { get; set; }

        [DataMember(Order = 1, IsRequired = true)]
        public int OutputJobSequenceNumber { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string ErrorDescription { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public List<CreateOutputResultProperty> ResultProperties { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class CreateOutputResultProperty
    {
        public CreateOutputResultProperty()
        {
        }

        public CreateOutputResultProperty(string name, object value)
        {
            PropertyName = name;
            PropertyValue = value;
        }

        [DataMember(Order = 0, IsRequired = true)]
        public string PropertyName { get; set; }

        [DataMember(Order = 1, IsRequired = false)]
        public object PropertyValue { get; set; }
    }
}
