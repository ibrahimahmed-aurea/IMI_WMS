using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.IdentityModel.Claims;

namespace Imi.SupplyChain.Services.IdentityModel.Extensions
{
    public static partial class IClaimsPrincipalExtensions
    {
        public static IClaimsIdentity First(this IClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);
            Contract.Requires(principal.Identities.Count > 0);
            Contract.Ensures(Contract.Result<IClaimsIdentity>() != null);
            
            return principal.Identities[0];
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, Predicate<Claim> predicate)
        {
            Contract.Requires(principal != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);
            
            foreach (IClaimsIdentity identity in principal.Identities)
            {
                foreach (Claim claim in identity.FindClaims(predicate))
                {
                    yield return claim;
                }
            }
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, string claimType)
        {
            Contract.Requires(principal != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);


            return principal.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, string claimType, string issuer)
        {
            Contract.Requires(principal != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Requires(!String.IsNullOrEmpty(issuer));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);


            return principal.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, string claimType, string issuer, string value)
        {
            Contract.Requires(principal != null);
            Contract.Requires(!String.IsNullOrEmpty(claimType));
            Contract.Requires(!String.IsNullOrEmpty(issuer));
            Contract.Requires(!String.IsNullOrEmpty(value));
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);


            return principal.FindClaims(c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, Claim claim)
        {
            Contract.Requires(principal != null);
            Contract.Requires(claim != null);
            Contract.Ensures(Contract.Result<IEnumerable<Claim>>() != null);


            return principal.FindClaims(c =>
                c.ClaimType.Equals(claim.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(claim.Issuer, StringComparison.OrdinalIgnoreCase));
        }
    }
}