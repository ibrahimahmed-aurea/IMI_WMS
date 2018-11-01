using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceContracts;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using System.ServiceModel;
using Imi.SupplyChain.Services.OrderManagement.Utilities.ServiceImplementation;
using Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation;
//using Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal;

/*
 * Configuration service implementation of the Order Management System service proxy.
 * 
 * The Smart Client Backend server hosts proxy services for a couple of services residing in the Order Management System and accessed through CSF OMS Web Services.
 * The reason for this is that it simplifies the security measures needed to be undertaken in CSF since Windows/SCBE supports WS-Trust/SAML out of the box.
 * 
 * Note that this implementation is designed to be API compatible with the underlying "real" implementation in order to be able to switch between the two.
 * 
 * Author: Peter.Tornqvist@aptean.com
 * Date:   2013-01-31
 */
namespace Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OrderManagement")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class GetGuiconfigurationService : IGetGuiconfigurationService
    {
        public Response[] GetGuiconfiguration(GetGuiconfigurationUSER[] users, bool useDomainInUserID)
        {
            Authorizer authorizer = Authorizer.GetInstance();
            try
            {
                if (users == null || users.Length == 0)
                {
                    return new Response[0];
                }
                GetGuiconfigurationServiceReal.GetGuiconfigurationServiceClient client = new GetGuiconfigurationServiceReal.GetGuiconfigurationServiceClient();
                GetGuiconfigurationServiceReal.GetGuiconfigurationUSER[] realUsers = new GetGuiconfigurationServiceReal.GetGuiconfigurationUSER[users.Length];
                for (int i = 0; i < users.Length; i++)
                {
                    GetGuiconfigurationUSER user = users[i];
                    GetGuiconfigurationServiceReal.GetGuiconfigurationUSER realUser = new GetGuiconfigurationServiceReal.GetGuiconfigurationUSER();
                    if (user.LoginId == null)
                    {
                        return errorResponse("loginId is null");
                    }
                    else if (user.UserId == null)
                    {
                        return errorResponse("userId may not be null");
                    }
                    else if (!authorizer.IsAuthorized(user.LoginId, useDomainInUserID))
                    {
                        return errorResponse("unauthorized query for login id: " + user.LoginId);
                    }
                    else if (!verifyLogicalUser(user.LoginId, user.UserId))
                    {
                        return errorResponse("logical user: " + user.UserId + " is not connected to login id: " + user.LoginId);
                    }
                    realUser.UserId = user.UserId;
                    realUser.PortNumber = user.PortNumber;
                    realUsers[i] = realUser;
                }
                object[] realObjects = client.GetGuiconfiguration(realUsers);
                int n = realObjects.Length;
                Response[] reponses = new Response[n];
                for (int i = 0; i < n; i++)
                {
                    object o = realObjects[i];
                    Response response = new Response();
                    if (o is GetGuiconfigurationServiceReal.ResponseSuccess)
                    {
                        GetGuiconfigurationServiceReal.ResponseSuccess realSuccess = (GetGuiconfigurationServiceReal.ResponseSuccess)o;
                        response.auto_start = realSuccess.auto_start;
                        response.clientprogram = realSuccess.clientprogram;
                        response.env_vars = realSuccess.env_vars;
                        response.help_url = realSuccess.help_url;
                        response.host = realSuccess.host;
                        response.language = realSuccess.language;
                        response.parameters = realSuccess.parameters;
                        response.port = realSuccess.port;
                        response.program = realSuccess.program;
                        response.systemname = realSuccess.systemname;
                        response.decimal_key = realSuccess.decimal_key;
                        response.working_directory = realSuccess.working_directory;
                        response.Success = true;
                    }
                    else if (o is GetGuiconfigurationServiceReal.ResponseFailure)
                    {
                        GetGuiconfigurationServiceReal.ResponseFailure realFailure = (GetGuiconfigurationServiceReal.ResponseFailure)o;
                        response.ErrorText = realFailure.ErrorText;
                        response.Success = false;
                    }
                    reponses[i] = response;
                }
                return reponses;
            }
            catch (Exception e)
            {
                Console.WriteLine("got an Exception in GetGuiconfiguration: {0}", e.Message);
                throw new Exception("GetGuiconfiguration Exception", e);
            }
        }

        private bool verifyLogicalUser(string loginId, string userId)
        {
            /* programmer incompetence: couldn't get the using statements to work
             */
            Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersServiceClient client;
            client = new Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersServiceClient();
            Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser user;
            user = new Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser();
            user.LoginId = loginId;
            Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser[] users;
            users = new Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser[1];
            users[0] = user;
            object[] objects = client.GetUsers(users);
            bool verified = false;
            foreach (object o in objects)
            {
                if (o is Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.ResponseSuccess)
                {
                    Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.ResponseSuccess success;
                    success = (Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.ResponseSuccess)o;
                    if (success.UserId == userId && success.LoginId == loginId)
                    {
                        verified = true;
                        break;
                    }
                }
                else if (o is Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.ResponseFailure)
                {
                    break;
                }
            }
            return verified;
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
