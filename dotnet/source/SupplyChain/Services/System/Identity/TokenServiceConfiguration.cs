using System;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;
using Imi.SupplyChain.Services.IdentityModel.Configuration;

namespace Imi.SupplyChain.Services.IdentityModel
{
    public class TokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public TokenServiceConfiguration()
            : base()
        {
            this.SecurityTokenService = typeof(TokenService);

            IdentityConfigurationSection config = ConfigurationManager.GetSection(IdentityConfigurationSection.SectionKey) as IdentityConfigurationSection;

            TokenIssuerName = config.IssuerName;
                        
            base.SigningCredentials = new X509SigningCredentials(CertificateUtil.GetCertificate(
                        StoreName.My, StoreLocation.LocalMachine,
                        config.SigningCertificateName));

            this.DefaultTokenLifetime = TimeSpan.Parse(config.TokenLifetime);
            this.MaximumTokenLifetime = TimeSpan.Parse(config.TokenLifetime);
        }
    }
}