using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Configuration;
using System.Security.Principal;
using System.Security;
using System.Diagnostics;
using Microsoft.IdentityModel.Claims;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Imi.SupplyChain.Services.IdentityModel.Configuration;

namespace Imi.SupplyChain.Services.IdentityModel
{
    public class WindowsUserNameSecurityTokenHandler : GenericUserNameSecurityTokenHandler
    {
        private IdentityConfigurationSection _config;
        private ContextType? _contextType;
        private ContextOptions? _contextOptions;
        private string _container;
        private string _contextName;
        private string _userName;
        private string _password;
                
        public WindowsUserNameSecurityTokenHandler()
        {
            _config = ConfigurationManager.GetSection(IdentityConfigurationSection.SectionKey) as IdentityConfigurationSection;
            
            if (!string.IsNullOrEmpty(_config.DirectorySettings.ContextType))
            {
                _contextType = (ContextType)Enum.Parse(typeof(ContextType), _config.DirectorySettings.ContextType, true);
            }

            _container = string.IsNullOrEmpty(_config.DirectorySettings.Container) ? null : _config.DirectorySettings.Container;
            _contextName = string.IsNullOrEmpty(_config.DirectorySettings.ContextName) ? null : _config.DirectorySettings.ContextName;
            _userName = string.IsNullOrEmpty(_config.DirectorySettings.UserName) ? null : _config.DirectorySettings.UserName;
            _password = string.IsNullOrEmpty(_config.DirectorySettings.Password) ? null : _config.DirectorySettings.Password;
            
            if (!string.IsNullOrEmpty(_config.DirectorySettings.ContextOptions))
            {
                _contextOptions = (ContextOptions)Enum.Parse(typeof(ContextOptions), _config.DirectorySettings.ContextOptions, true);
            }
        }

        protected override bool ValidateUserNameCredential(string userName, string password, out List<Claim> claims)
        {
            claims = new List<Claim>();

            using (PrincipalContext context = GetContext(userName, password))
            {
                if (context.ValidateCredentials(GetUserNoDomain(userName), password))
                {
                    claims.AddRange(GetOutputClaims(userName, context));

                    return true;
                }
            }

            return false;
        }

        private static IList<Claim> GetOutputClaims(string userName, PrincipalContext context)
        {
            List<Claim> claims = new List<Claim>();

            UserPrincipal user = UserPrincipal.FindByIdentity(context, GetUserNoDomain(userName));
            
            string upn = GetUserPrincipalName(user);

            claims.Add(new Claim(ClaimTypes.Upn, upn));
            claims.Add(new Claim(ClaimTypes.Sid, user.Sid.ToString()));

            LogEntry entry;

            bool foundGroups = false;

            if (context.ContextType != ContextType.Machine)
            {
                entry = new LogEntry();
                entry.Severity = TraceEventType.Information;
                entry.Priority = -1;

                if (Logger.ShouldLog(entry))
                {
                    entry.Message = "Getting authorization groups from WindowsIdentity...";
                    Logger.Write(entry);
                }

                try
                {
                    using (WindowsIdentity identity = new WindowsIdentity(upn))
                    {
                        foreach (IdentityReference group in identity.Groups)
                        {
                            claims.Add(new Claim(ClaimTypes.GroupSid, group.Value));
                        }
                    }

                    foundGroups = true;
                }
                catch (UnauthorizedAccessException ex)
                {
                    entry = new LogEntry();
                    entry.Severity = TraceEventType.Error;
                    entry.Priority = -1;

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = ex.ToString();
                        Logger.Write(entry);
                    }
                }
                catch (SecurityException ex)
                {
                    entry = new LogEntry();
                    entry.Severity = TraceEventType.Error;
                    entry.Priority = -1;

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = ex.ToString();
                        Logger.Write(entry);
                    }
                }
            }

            if (!foundGroups)
            {
                entry = new LogEntry();
                entry.Severity = TraceEventType.Information;
                entry.Priority = -1;

                if (Logger.ShouldLog(entry))
                {
                    entry.Message = "Getting authorization groups from UserPrincipal...";
                    Logger.Write(entry);
                }

                try
                {
                    using (PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups())
                    {
                        var enumerator = groups.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            try
                            {
                                using (Principal p = enumerator.Current)
                                {
                                    if (p.Sid != null)
                                    {
                                        claims.Add(new Claim(ClaimTypes.GroupSid, p.Sid.ToString()));
                                    }
                                }
                            }
                            catch (NoMatchingPrincipalException)
                            {
                            }
                            catch (PrincipalOperationException)
                            {
                            }
                        }
                    }
                }
                catch (PrincipalOperationException ex)
                {
                    entry = new LogEntry();
                    entry.Severity = TraceEventType.Error;
                    entry.Priority = -1;

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = ex.ToString();
                        Logger.Write(entry);
                    }
                }
            }

            entry = new LogEntry();
            entry.Severity = TraceEventType.Verbose;
            entry.Priority = -1;

            if (Logger.ShouldLog(entry))
            {
                StringBuilder sb = new StringBuilder();
            
                foreach (Claim claim in claims)
                {
                    if (claim.ClaimType == ClaimTypes.GroupSid)
                    {
                        sb.AppendLine(string.Format("Found Group SID: {0}", claim.Value));
                    }
                }

                entry.Message = sb.ToString();

                Logger.Write(entry);
            }

            return claims;
        }

        private static string GetUserPrincipalName(UserPrincipal user)
        {
            if (string.IsNullOrEmpty(user.UserPrincipalName))
            {
                return string.Format("{0}@{1}", user.Name, user.Context.ConnectedServer);
            }
            else
            {
                return user.UserPrincipalName;
            }
        }

        private static string GetUserNoDomain(string userName)
        {
            if ((userName.Contains('\\') || userName.Contains('/')))
            {
                string[] userIdParts = userName.Split(new char[] { '\\', '/' });

                userName = userIdParts[1];
            }

            return userName;
        }

        private static string GetUserDomain(string userName)
        {
            if ((userName.Contains('\\') || userName.Contains('/')))
            {
                string[] userIdParts = userName.Split(new char[] { '\\', '/' });

                return userIdParts[0];
            }

            return null;
        }
                
        protected PrincipalContext GetContext(string userName, string password)
        {
            ContextType? contextType = _contextType;
            string contextName = _contextName;
            ContextOptions? contextOptions = _contextOptions;
            string container = _container;
            string domainName = GetUserDomain(userName);

            if (!contextType.HasValue)
            {
                if (!string.IsNullOrEmpty(domainName) && (domainName == "." || domainName.ToLower() == Environment.MachineName.ToLower()))
                {
                    contextName = null;
                    contextOptions = null;
                    container = null;
                    contextType = ContextType.Machine;
                }
                else
                {
                    contextType = ContextType.Domain;
                }
            }
            
            if (contextType == ContextType.Domain && string.IsNullOrEmpty(contextName))
            {
                contextName = domainName;
            }
                        
            if (contextOptions.HasValue)
            {
                return new PrincipalContext(contextType.Value, contextName, container, contextOptions.Value, _userName, _password);
            }
            else
            {
                return new PrincipalContext(contextType.Value, contextName, container, _userName, _password);
            }
        }
    }
}
