// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://claimsbasedwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.IdentityModel.Protocols.WSFederation;
using System.ServiceModel.Security.Tokens;
using System.IO;

namespace Imi.Framework.UX.Identity
{
    public class SecurityTokenCache : IXmlSerializable
    {
        private SecurityToken _token;
        private RequestSecurityTokenResponse _rawToken;

        public SecurityToken Token 
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;

                if (TokenUpdated != null)
                {
                    TokenUpdated(this, null);
                }
            }
        }

        public event EventHandler TokenUpdated;
                
        public RequestSecurityTokenResponse RawToken
        {
            get
            {
                return _rawToken;
            }
            set
            {
                _rawToken = value;
            }
        }

        public string GetTokenXmlAsString()
        {
            return new WSFederationSerializer().GetResponseAsString(_rawToken, new WSTrustSerializationContext());
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            string securityTokenXml = reader.ReadString();

            RequestSecurityTokenResponse rstr = new WSFederationSerializer().CreateResponse(
                    new SignInResponseMessage(new Uri("http://notused"), securityTokenXml),
                    new WSTrustSerializationContext());

            _token = new GenericXmlSecurityToken(
                rstr.RequestedSecurityToken.SecurityTokenXml,
                new BinarySecretSecurityToken(
                    rstr.RequestedProofToken.ProtectedKey.GetKeyBytes()),
                rstr.Lifetime.Created.HasValue ? rstr.Lifetime.Created.Value : DateTime.MinValue,
                rstr.Lifetime.Expires.HasValue ? rstr.Lifetime.Expires.Value : DateTime.MaxValue,
                rstr.RequestedAttachedReference,
                rstr.RequestedUnattachedReference,
                null);

            _rawToken = rstr;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            string securityTokenXml = new WSFederationSerializer().GetResponseAsString(_rawToken, new WSTrustSerializationContext());

            writer.WriteString(securityTokenXml);
        }

        #endregion

        public static string Serialize(SecurityTokenCache tokenCache)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SecurityTokenCache));

            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, tokenCache);
                return sw.ToString();
            }
        }

        public static SecurityTokenCache Deserialize(string securityTokenXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Imi.Framework.UX.Identity.SecurityTokenCache));

            using (StringReader sr = new StringReader(securityTokenXml))
            {
                return serializer.Deserialize(sr) as SecurityTokenCache;
            }
        }

        public void Flush()
        {
            _token = null;
            _rawToken = null;
        }
    }
}
