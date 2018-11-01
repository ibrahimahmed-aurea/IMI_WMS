using System;
using System.Collections.Generic;
using System.Text;
using Cdc.Framework.Services;
using Imi.Framework.Shared.Security;
using Cdc.SupplyChain.Transportation.BusinessLogic.Authentication.Entities;

namespace Cdc.SupplyChain.Transportation.BusinessLogic.Authentication.BusinessLogic
{
    internal class CredentialProxy
    {
        private string userName;
        private string password;
        private DateTime timeOut;

        // TODO replace hard coded 5 minute timeout
        private readonly TimeSpan defaultTimeout = new TimeSpan(0, 5, 0);

        public CredentialProxy(string userName, string password)
        {
            this.userName = userName;
            this.password = password;

            timeOut = DateTime.Now + defaultTimeout;
        }

        public static bool IsValid(string userName, string password, CredentialProxy p)
        {
            if (p.timeOut >= DateTime.Now)
            {
                if ((p.userName == userName) && (p.password == password))
                {
                    return true;
                }
            }

            return false;
        }
        
    }

    public class ValidateUserNamePasswordAction 
    {
        private static Dictionary<string, CredentialProxy> credentialCache = new Dictionary<string, CredentialProxy>();
        private static object lockObject = new object();

        private const string productName = "Transportation";

        public void Execute(ValidateUserNamePasswordParams parameters) {

            CredentialProxy credentials;


            lock (lockObject)
            {
                credentialCache.TryGetValue(parameters.UserName, out credentials);
            }

            if (credentials != null)
            {
                if (CredentialProxy.IsValid(parameters.UserName, parameters.Password, credentials))
                {
                    return;
                }
                else
                {
                    lock (lockObject)
                    {
                        credentialCache.Remove(parameters.UserName);
                    }
                }
            }

            ValidateUser(parameters);

            lock (lockObject)
            {
                credentialCache.Add(parameters.UserName, new CredentialProxy(parameters.UserName, parameters.Password));
            }
        }

        private static void ValidateUser(ValidateUserNamePasswordParams parameters)
        {
            AuthenticationProvider provider = new AuthenticationProvider();
            provider.Initialize(parameters.Password);

            AuthenticateUserAction aua = new AuthenticateUserAction();
            AuthenticateUserParams p = new AuthenticateUserParams();

            p.UserName = parameters.UserName;
            p.Salt = provider.Salt;
            p.Data = provider.SessionData;
            p.Product = productName;
            p.NodeIdentity = ""; //TODO loose this
            p.TerminalIdentity = "";  //TODO loose this

            // Authenticate
            AuthenticateUserResultParams r = aua.Execute(p);
            if (!String.IsNullOrEmpty(r.Almid))
                throw new AlarmException(r.Almid, r.AlarmText1, null);

            AuthenticationSession authenticationSession = provider.DecryptSession(r.SessionData);

            // Logon application
            LogonUserParams lp = new LogonUserParams();
            lp.TerminalIdentity = "";  //TODO loose this
            lp.UserName = parameters.UserName;
            lp.SessionIdentity = authenticationSession.SessionId;

            LogonUserAction lua = new LogonUserAction();
            LogonUserResultParams lr = lua.Execute(lp);

            if (!String.IsNullOrEmpty(lr.Almid))
                throw new AlarmException(lr.Almid, lr.AlarmText1,null);
        }

    }
}
