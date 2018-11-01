using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.IdentityModel.Claims;

namespace Imi.SupplyChain.Services.IdentityModel.Extensions
{
    public static partial class IClaimsIdentityExtensions
    {
        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, Predicate<Claim> predicate)
        {
            Contract.Requires(identity != null);
            Contract.Requires(identity.Claims != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);
            
            return from claim in identity.Claims
                   where predicate(claim)
                   select claim;
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, string claimType)
        {
            Contract.Requires(identity != null);
            Contract.Requires(identity.Claims != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);
            
            return identity.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, string claimType, string issuer)
        {
            Contract.Requires(identity != null);
            Contract.Requires(identity.Claims != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Requires(!String.IsNullOrEmpty(issuer));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);

            return identity.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, string claimType, string issuer, string value)
        {
            Contract.Requires(identity != null);
            Contract.Requires(identity.Claims != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Requires(!String.IsNullOrEmpty(issuer));
            Contract.Requires(!String.IsNullOrEmpty(value));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);
            
            return identity.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, Claim claim)
        {
            Contract.Requires(identity != null);
            Contract.Requires(identity.Claims != null);
            Contract.Requires(claim != null);
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);
            
            return identity.FindClaims(c =>
                c.ClaimType.Equals(claim.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(claim.Issuer, StringComparison.OrdinalIgnoreCase));
        }
    }
}