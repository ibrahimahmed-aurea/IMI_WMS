// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://claimsbasedwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace Imi.Framework.UX.Identity
{
    public class CachedClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        public CachedClientCredentialsSecurityTokenManager(CachedClientCredentials clientCredentials)
            : base(clientCredentials)
        {
        }

        public override System.IdentityModel.Selectors.SecurityTokenProvider CreateSecurityTokenProvider(System.IdentityModel.Selectors.SecurityTokenRequirement tokenRequirement)
        {
            if (!this.IsIssuedSecurityTokenRequirement(tokenRequirement))
                return base.CreateSecurityTokenProvider(tokenRequirement);

            IssuedSecurityTokenProvider provider = base.CreateSecurityTokenProvider(tokenRequirement) as IssuedSecurityTokenProvider;
            CachedIssuedSecurityTokenProvider cachedProvider = new CachedIssuedSecurityTokenProvider(provider.IssuerBinding, provider.IssuerAddress, provider.TargetAddress, (CachedClientCredentials)this.ClientCredentials);
            return cachedProvider;
        }
    }
}
