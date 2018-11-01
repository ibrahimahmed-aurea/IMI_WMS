using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class UserWarehouse
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string WarehouseIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string WarehouseName { get; set; }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class UserCompany
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string CompanyIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string CompanyName { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public bool? IsClientInterfaceWebServices { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public bool? IsClientInterfaceHAPI { get; set; }

        [DataMember(Order = 5, IsRequired = false)]
        public string WarehouseIdentity { get; set; }

        [DataMember(Order = 6, IsRequired = false)]
        public bool? IsClientInterfaceEDI { get; set; }
    }

    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class UserWarehouseCollection : List<UserWarehouse>
    {
        public UserWarehouseCollection()
        {
        }

        public UserWarehouseCollection(IEnumerable<UserWarehouse> collection)
            : base(collection)
        {
        }
    }

    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class UserCompanyCollection : List<UserCompany>
    {
        public UserCompanyCollection()
        {
        }

        public UserCompanyCollection(IEnumerable<UserCompany> collection)
            : base(collection)
        {
        }
    }

    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class FindUserDetailsResult
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string UserIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string UserName { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public string RecentWarehouseIdentity { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public string RecentCompanyIdentity { get; set; }

        [DataMember(Order = 5, IsRequired = false)]
        public Nullable<DateTime> LastLogonTime { get; set; }

        [DataMember(Order = 6, IsRequired = false)]
        public UserWarehouseCollection UserWarehouses { get; set; }

        [DataMember(Order = 7, IsRequired = false)]
        public UserCompanyCollection UserCompanies { get; set; }
    }
}
