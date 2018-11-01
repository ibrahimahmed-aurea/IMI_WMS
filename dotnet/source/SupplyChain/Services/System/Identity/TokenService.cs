//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Imi.SupplyChain.Services.IdentityModel.Configuration;
using Imi.SupplyChain.Services.IdentityModel.Extensions;

namespace Imi.SupplyChain.Services.IdentityModel
{
    public class TokenService : SecurityTokenService
    {
        static bool enableAppliesToValidation = false;
        static readonly string[] ActiveClaimsAwareApps = { /*"https://localhost/ActiveClaimsAwareWebApp"*/ };
        
        private IdentityConfigurationSection _config;
        
        public TokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
            _config = ConfigurationManager.GetSection(IdentityConfigurationSection.SectionKey) as IdentityConfigurationSection;
        }

        void ValidateAppliesTo(EndpointAddress appliesTo)
        {
            if (appliesTo == null)
            {
                throw new ArgumentNullException("appliesTo");
            }

            if (enableAppliesToValidation)
            {
                bool validAppliesTo = false;

                foreach (string rpUrl in ActiveClaimsAwareApps)
                {
                    if (appliesTo.Uri.Equals(new Uri(rpUrl)))
                    {
                        validAppliesTo = true;
                        break;
                    }
                }

                if (!validAppliesTo)
                {
                    throw new InvalidRequestException(String.Format("The 'appliesTo' address '{0}' is not valid.", appliesTo.Uri.OriginalString));
                }
            }
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            ValidateAppliesTo(request.AppliesTo);

            Scope scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);
                        
            string encryptingCertificateName = _config.EncryptingCertificateName;
                        
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            return scope;
        }
        
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (null == principal)
            {
                throw new ArgumentNullException("principal");
            }

            var name = principal.FindClaims(ClaimTypes.Name).First().Value;
            var nameId = new Claim(ClaimTypes.NameIdentifier, name);

            var userClaims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, name),
                nameId,
                new Claim(ClaimTypes.AuthenticationMethod, principal.FindClaims(ClaimTypes.AuthenticationMethod).First().Value),
                new Claim(ClaimTypes.AuthenticationInstant, XmlConvert.ToString(DateTime.UtcNow, "yyyy-MM-ddTHH:mm:ss.fffZ"), ClaimValueTypes.Datetime),
                new Claim(ClaimTypes.Upn, principal.FindClaims(ClaimTypes.Upn).First().Value),
                new Claim(ClaimTypes.Sid, principal.FindClaims(ClaimTypes.Sid).First().Value),
            };

            userClaims.AddRange(principal.FindClaims(ClaimTypes.GroupSid));
                                               
            var outputIdentity = new ClaimsIdentity(userClaims);
                        
            return outputIdentity;
        }
    }
}