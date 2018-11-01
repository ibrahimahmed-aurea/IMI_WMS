using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Security;
using Cdc.SupplyChain.Services.System.MobileAuthentication.DataAccess.SessionService;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.DataAccess.ServiceAgents
{
    public class PasswordValidationAgent
    {
        public void Validate(string username, string password, string domain)
        {
            // TODO maybe not hardcode this
            // TODO use securestring
            using (var client = new SessionServiceClient("WSHttpBinding_ISessionService"))
            {
                client.ClientCredentials.UserName.UserName = string.Format(@"{0}\{1}", domain, username);
                client.ClientCredentials.UserName.Password = password;
                
                var parameters = new CreateSessionParams()
                                          {
                                              LanguageCode = "EN"
                                          };

                client.CreateSession(parameters);
            }
        }
    }
}
