using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Imi.SupplyChain.Services.OrderManagement.Menu.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Menu.ServiceContracts;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.IdentityModel.Claims;
using System.Threading;
using Imi.SupplyChain.Services.OrderManagement.Utilities.ServiceImplementation;

/*
 * Menu service implementation of the Order Management System service proxy.
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Menu.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OrderManagement")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GetMenuService : IGetMenuService
    {
        public Response[] GetMenu(GetMenuUSER[] users, bool useDomainInUserID)
        {
            Authorizer authorizer = Authorizer.GetInstance();
            try
            {
                if (users == null || users.Length == 0)
                {
                    return new Response[0];
                }
                GetMenuServiceReal.GetMenuServiceClient client = new GetMenuServiceReal.GetMenuServiceClient();
                GetMenuServiceReal.GetMenuUSER[] realUsers = new GetMenuServiceReal.GetMenuUSER[users.Length];
                for (int i = 0; i < users.Length; i++)
                {
                    GetMenuUSER user = users[i];
                    GetMenuServiceReal.GetMenuUSER realUser = new GetMenuServiceReal.GetMenuUSER();
                    if (user.LoginId == null)
                    {
                        return errorResponse("loginId is null");
                    }
                    else if (!authorizer.IsAuthorized(user.LoginId, useDomainInUserID))
                    {
                        return errorResponse("unauthorized query for login id: " + user.LoginId);
                    }
                    realUser.LoginId = user.LoginId;
                    realUser.Language = user.Language;
                    realUsers[i] = realUser;
                }
                object[] realObjects = client.GetMenu(realUsers);
                int n = realObjects.Length;
                Response[] responses = new Response[n];
                for (int i = 0; i < n; i++)
                {
                    object o = realObjects[i];
                    Response response = new Response();
                    responses[i] = response;
                    if (o is GetMenuServiceReal.ResponseSuccess)
                    {
                        GetMenuServiceReal.ResponseSuccess realSuccess = (GetMenuServiceReal.ResponseSuccess)o;
                        response.Description = realSuccess.Description;
                        response.MenuId = realSuccess.MenuId;
                        response.MenuLineType = realSuccess.MenuLineType;
                        response.RoutineId = realSuccess.RoutineId;
                        response.Success = true;
                    }
                    else if (o is GetMenuServiceReal.ResponseFailure)
                    {
                        GetMenuServiceReal.ResponseFailure realFailure = (GetMenuServiceReal.ResponseFailure)o;
                        response.ErrorText = realFailure.ErrorText;
                        response.Success = false;
                    }
                }
                return responses;
            }
            catch (Exception e)
            {
                Console.WriteLine("got an Exception in GetMenu: {0}", e.Message);
                throw new Exception("GetMenu Exception", e);
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
