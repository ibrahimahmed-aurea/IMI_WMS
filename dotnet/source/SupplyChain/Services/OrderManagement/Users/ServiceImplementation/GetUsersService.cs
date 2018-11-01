using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Services.OrderManagement.Users.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Users.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Imi.Framework.Services;
using System.ServiceModel;
using Imi.SupplyChain.Services.OrderManagement.Utilities.ServiceImplementation;

/*
 * User service implementation of the Order Management System service proxy.
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OrderManagement")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GetUsersService : IGetUsersService
    {
        public Response[] GetUsers(GetUsersUser[] users, bool useDomainInUserID)
        {
            Authorizer authorizer = Authorizer.GetInstance();
            try
            {
                if (users == null || users.Length == 0)
                {
                    return new Response[0];
                }
                GetUsersServiceReal.GetUsersServiceClient client = new GetUsersServiceReal.GetUsersServiceClient();
                GetUsersServiceReal.GetUsersUser[] realUsers = new GetUsersServiceReal.GetUsersUser[users.Length];
                for (int i = 0; i < users.Length; i++)
                {
                    GetUsersUser user = users[i];
                    GetUsersServiceReal.GetUsersUser realUser = new GetUsersServiceReal.GetUsersUser();
                    if (user.LoginId == null)
                    {
                        return errorResponse("loginId is null");
                    }
                    else if (!authorizer.IsAuthorized(user.LoginId, useDomainInUserID))
                    {
                        return errorResponse("unauthorized query for login id: " + user.LoginId);
                    }
                    realUser.LoginId = user.LoginId;
                    realUsers[i] = realUser;
                }
                object[] realObjects = client.GetUsers(realUsers);
                int n = realObjects.Length;
                Response[] responses = new Response[n];
                for (int i = 0; i < n; i++)
                {
                    object o = realObjects[i];
                    Response response = new Response();
                    if (o is GetUsersServiceReal.ResponseSuccess)
                    {
                        GetUsersServiceReal.ResponseSuccess realSuccess = (GetUsersServiceReal.ResponseSuccess)o;
                        response.EmployNumber = realSuccess.EmployNumber;
                        response.LegalEntity = realSuccess.LegalEntity;
                        response.LegalEntitySpecified = realSuccess.LegalEntitySpecified;
                        response.LoginId = realSuccess.LoginId;
                        response.OrgUnit = realSuccess.OrgUnit;
                        response.UserId = realSuccess.UserId;
                        response.UserName = realSuccess.UserName;
                        response.WarehouseNumber = realSuccess.WarehouseNumber;
                        response.Success = true;
                        responses[i] = response;
                    }
                    else if (o is GetUsersServiceReal.ResponseFailure)
                    {
                        GetUsersServiceReal.ResponseFailure realFailure = (GetUsersServiceReal.ResponseFailure)o;
                        response.ErrorText = realFailure.ErrorText;
                        response.Success = false;
                        responses[i] = response;
                    }
                }
                return responses;
            }
            catch (Exception e)
            {
                Console.WriteLine("got an Exception in GetUsers: {0}", e.Message);
                throw new Exception("GetUsers Exception", e);
            }
        }

        private Response[] errorResponse(string msg)
        {
            Response response = new Response();
            response.Success = false;
            response.ErrorText = msg;
            Response[] responses = new Response[1];
            responses[0] = response;
            return responses;
        }
    }
}
