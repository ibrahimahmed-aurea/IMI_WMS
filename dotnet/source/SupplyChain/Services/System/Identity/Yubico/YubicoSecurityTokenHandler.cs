using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Configuration;
using Imi.SupplyChain.Services.IdentityModel.Configuration;
using Microsoft.IdentityModel.Claims;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Imi.SupplyChain.Services.IdentityModel.Yubico
{
    public class YubicoSecurityTokenHandler : WindowsUserNameSecurityTokenHandler
    {
        private IdentityConfigurationSection _config;
        private string[] _apiUrls;

        public YubicoSecurityTokenHandler()
        {
            _config = ConfigurationManager.GetSection(IdentityConfigurationSection.SectionKey) as IdentityConfigurationSection;

            _apiUrls = (from ValidationServerElement vse in _config.YubicoSettings.ValidationServers
                        where !string.IsNullOrEmpty(vse.Url)
                        select vse.Url).ToArray();
        }

        protected override bool ValidateUserNameCredential(string userName, string password, out List<Claim> claims)
        {
            claims = new List<Claim>();

            int otpLength = 32 + _config.YubicoSettings.PublicIdLength;

            if (password.Length >= otpLength)
            {
                string otp = password.Substring(password.Length - otpLength, otpLength);

                if (YubicoClient.IsOtpValidFormat(otp))
                {
                    string windowsPassword = password.Substring(0, password.Length - otpLength);

                    List<Claim> windowsClaims;

                    if (base.ValidateUserNameCredential(userName, windowsPassword, out windowsClaims))
                    {
                        using (PrincipalContext context = GetContext(userName, windowsPassword))
                        {
                            using (Principal principal = Principal.FindByIdentity(context, userName))
                            {
                                using (DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry)
                                {
                                    string publicId = null;

                                    if (directoryEntry.Properties.Contains(_config.YubicoSettings.PublicIdAttributeName))
                                    {
                                        publicId = directoryEntry.Properties[_config.YubicoSettings.PublicIdAttributeName].Value.ToString();
                                    }

                                    if (!string.IsNullOrEmpty(publicId))
                                    {
                                        YubicoClient client = new YubicoClient(_config.YubicoSettings.ClientId, _config.YubicoSettings.APIKey);
                                        client.SetUrls(_apiUrls);

                                        YubicoResponse response = client.Validate(otp);

                                        if (response != null)
                                        {
                                            if (response.GetStatus() == YubicoResponseStatus.OK && response.GetPublicId() == publicId)
                                            {
                                                claims.AddRange(windowsClaims);
                                                return true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogEntry entry = new LogEntry();
                                        entry.Severity = TraceEventType.Error;
                                        entry.Priority = -1;

                                        if (Logger.ShouldLog(entry))
                                        {
                                            entry.Message = string.Format("Unable to find the user's PublicId. PublicIdAttributeName=\"{0}\".", _config.YubicoSettings.PublicIdAttributeName);
                                            Logger.Write(entry);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogEntry entry = new LogEntry();
                    entry.Severity = TraceEventType.Error;
                    entry.Priority = -1;

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = "Invalid OTP Format.";
                        Logger.Write(entry);
                    }
                }
            }
            else
            {
                LogEntry entry = new LogEntry();
                entry.Severity = TraceEventType.Error;
                entry.Priority = -1;
                
                if (Logger.ShouldLog(entry))
                {
                    entry.Message = "Invalid OTP Length.";
                    Logger.Write(entry);
                }
            }

            return false;
        }
    }
}
