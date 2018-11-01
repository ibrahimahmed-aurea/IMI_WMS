using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OrderManagement.Services.DataContracts/Beta v0.1.0")]
    public class OMSUser
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string UserId { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string LegalEntity { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public string WarehouseId { get; set; }
    }

    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.OrderManagement.Services.DataContracts/Beta v0.1.0")]
    public class OMSUserCollection : List<OMSUser>
    {
        public OMSUserCollection()
        {
        }

        public OMSUserCollection(IEnumerable<OMSUser> collection)
            : base(collection)
        {
        }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.OrderManagement.Services.DataContracts/Beta v0.1.0")]
    public class OMSUsersComboBoxData
    {
        [DataMember(Order = 1, IsRequired = false)]
        public OMSUserCollection OMSUsers { get; set; }
    }
}
