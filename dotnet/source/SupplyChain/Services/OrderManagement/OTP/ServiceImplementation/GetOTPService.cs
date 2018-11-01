using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Imi.SupplyChain.Services.OrderManagement.OTP.ServiceContracts;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.IdentityModel.Claims;
using System.Threading;
using Imi.SupplyChain.Services.OrderManagement.Utilities.ServiceImplementation;

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
namespace Imi.SupplyChain.Services.OrderManagement.OTP.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OrderManagement")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GetOTPService : IGetOTPService
    {
        public string GetOTP(string userId)
        {
            OTPClient.OtpPortTypeClient client = new OTPClient.OtpPortTypeClient();
            OTPClient.OtpRequest request = new OTPClient.OtpRequest();
            request.loginId = Utilities.ServiceImplementation.Authorizer.GetInstance().getLoginId();
            request.userId = userId;
            OTPClient.OtpResponse response = client.Otp(request);
            return response.otp;
        }
    }
}