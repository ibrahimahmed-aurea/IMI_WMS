using System;
using System.Linq;
using System.Text;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessEntities;
using Cdc.SupplyChain.Services.System.MobileAuthentication.DataAccess.ServiceAgents;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessLogic
{
    [ActionDomain("Mobile")]
    public class LoginAction : MarshalByRefObject
    {
        private PasswordValidationAgent agent;

        public LoginAction()
        {
            agent = new PasswordValidationAgent();
        }

        public LoginResult Execute(LoginParameters parameters)
        {
            LoginResult result = new LoginResult();
            try
            {
                agent.Validate(parameters.Username, parameters.Password, parameters.Domain);
                result.Success = true;
            }
            catch (Exception)
            {
                // TODO localize
                result.Success = false;
                result.ErrorMessage = "Username or password is wrong.";
            }
            return result;
        }

    }
}
