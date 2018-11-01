// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://claimsbasedwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;

namespace Imi.Framework.UX.Identity
{
    public class CachedClientCredentials: ClientCredentials
    {
        public SecurityTokenCache TokenCache { get; private set; }

        public CachedClientCredentials(SecurityTokenCache tokenCache): base()
        {
            this.TokenCache = tokenCache;
        }

        public CachedClientCredentials(SecurityTokenCache tokenCache, ClientCredentials clientCredentials)
            : base(clientCredentials)
        {
            this.TokenCache = tokenCache;
        }

        public CachedClientCredentials(CachedClientCredentials clientCredentials): base(clientCredentials)
        {
            this.TokenCache = clientCredentials.TokenCache;
        }

        public override System.IdentityModel.Selectors.SecurityTokenManager CreateSecurityTokenManager()
        {
            return new CachedClientCredentialsSecurityTokenManager((CachedClientCredentials)this.Clone());

        }

        protected override ClientCredentials CloneCore()
        {
            return new CachedClientCredentials(this);
        }
        
        
    }
}
