using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

/*
 * Data Contract for the configuration request in the Order Management System service
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts/2011/09")]
    public class Response
    {
        [DataMember]
        public string auto_start { get; set; }

        [DataMember]
        public string clientprogram { get; set; }

        [DataMember]
        public string env_vars { get; set; }

        [DataMember]
        public string help_url { get; set; }

        [DataMember]
        public string host { get; set; }

        [DataMember]
        public string language { get; set; }

        [DataMember]
        public string parameters { get; set; }

        [DataMember]
        public decimal port { get; set; }

        [DataMember]
        public string program { get; set; }

        [DataMember]
        public string systemname { get; set; }

        [DataMember]
        public string working_directory { get; set; }

        [DataMember]
        public string decimal_key { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorText { get; set; }
    }
}
