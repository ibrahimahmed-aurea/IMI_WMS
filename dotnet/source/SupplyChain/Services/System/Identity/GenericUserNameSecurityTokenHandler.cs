/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Imi.SupplyChain.Services.IdentityModel
{
    public abstract class GenericUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
    {
        protected GenericUserNameSecurityTokenHandler()
        { 
        }
                        
        protected abstract bool ValidateUserNameCredential(string userName, string password, out List<Claim> claims);
        
        public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            LogEntry entry = new LogEntry();
            entry.Severity = TraceEventType.Verbose;
            entry.Priority = -1;
                        
            if (Logger.ShouldLog(entry))
            {
                entry.Message = "Validating SecurityToken...";
                Logger.Write(entry);
            }

            try
            {
                if (token == null)
                {
                    throw new ArgumentNullException("token");
                }

                if (base.Configuration == null)
                {
                    throw new InvalidOperationException("No Configuration set");
                }

                UserNameSecurityToken unToken = token as UserNameSecurityToken;

                if (unToken == null)
                {
                    throw new ArgumentException("SecurityToken is no UserNameSecurityToken");
                }

                List<Claim> claims;

                if (!ValidateUserNameCredential(unToken.UserName, unToken.Password, out claims))
                {
                    throw new SecurityTokenValidationException(string.Format("Invalid credentials for user \"{0}\".", unToken.UserName));
                }

                claims.Add(new Claim(ClaimTypes.Name, unToken.UserName));
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password));

                var identity = new ClaimsIdentity(claims);

                if (base.Configuration.SaveBootstrapTokens)
                {
                    if (this.RetainPassword)
                    {
                        identity.BootstrapToken = unToken;
                    }
                    else
                    {
                        identity.BootstrapToken = new UserNameSecurityToken(unToken.UserName, null);
                    }
                }

                entry = new LogEntry();
                entry.Severity = TraceEventType.Verbose;
                entry.Priority = -1;

                if (Logger.ShouldLog(entry))
                {
                    entry.Message = "SecurityToken successfully validated.";
                    Logger.Write(entry);
                }
               
                return new ClaimsIdentityCollection(new IClaimsIdentity[] { identity });
            }
            catch (Exception ex)
            {
                entry = new LogEntry();
                entry.Severity = TraceEventType.Error;
                entry.Priority = -1;
                
                if (Logger.ShouldLog(entry))
                {
                    entry.Message = string.Format("Error validating SecurityToken: {0}.", ex.ToString());
                    Logger.Write(entry);
                }

                throw;
            }
        }
              
        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }
    }
}
