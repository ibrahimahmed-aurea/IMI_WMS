using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

/*
 * Data Contract for the user response in the Order Management System service
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Users.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Services.OrderManagement.Users.DataContracts/2011/09")]
    public class Response
    {
        [DataMember]
        public string EmployNumber { get; set; }

        [DataMember]
        public decimal LegalEntity { get; set; }

        [DataMember]
        public bool LegalEntitySpecified { get; set; }

        [DataMember]
        public string OrgUnit { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public decimal WarehouseNumber { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorText { get; set; }

        [DataMember]
        public string LoginId { get; set; }
    }
}
