using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Imi.SupplyChain.Services.OrderManagement.Users.DataContracts;
using Imi.Framework.Services;

/*
 * Service Contract for the user service in the Order Management System service
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Users.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Services.OrderManagement.Users.ServiceContracts/2011/09")]
    [ServiceApplicationName("OrderManagement")]
    public interface IGetUsersService
    {
        [OperationContract]
        Response[] GetUsers(GetUsersUser[] users, bool useDomainInUserID);
    }
}
