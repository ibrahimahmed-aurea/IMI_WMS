/*
 * Service Contract for the OTP service in the Order Management System service
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * This particular service is a front end for a service in Anywhere Server which creates a one-time password for the given user.
 * 
 * Note that the actual user is implicitly known on the server side and thus is not part of the contract
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2014-11-07
 */
using Imi.Framework.Services;
using System.ServiceModel;
namespace Imi.SupplyChain.Services.OrderManagement.OTP.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Services.OrderManagement.OTP.ServiceContracts/2011/09")]
    [ServiceApplicationName("OrderManagement")]
    public interface IGetOTPService
    {
        [OperationContract]
        string GetOTP(string userId);
    }
}
