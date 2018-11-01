// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://claimsbasedwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Security.Tokens;
using System.ServiceModel.Description;
using System.Xml;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using Microsoft.IdentityModel.Protocols.WSTrust;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.SecurityTokenService;
using System.ServiceModel.Channels;
using System.Net;
using System.Net.Security;

namespace Imi.Framework.UX.Identity
{
    public class CachedIssuedSecurityTokenProvider: IssuedSecurityTokenProvider, ICommunicationObject, IDisposable
    {
        private CachedClientCredentials _clientCredentials;
        private WSTrustChannelFactory _factory;

        public CachedIssuedSecurityTokenProvider(Binding issuerBinding, EndpointAddress issuerAddress, EndpointAddress targetAddress, CachedClientCredentials clientCredentials)
            : base()
        {
            _clientCredentials = clientCredentials;
            this.IssuerAddress = issuerAddress;
            this.IssuerBinding = issuerBinding;
            this.TargetAddress = targetAddress;
                        
            _factory = new WSTrustChannelFactory(IssuerBinding, IssuerAddress);
            _factory.Endpoint.Behaviors.Find<ClientCredentials>().ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            _factory.TrustVersion = TrustVersion.WSTrust13;
            _factory.Credentials.UserName.UserName = _clientCredentials.UserName.UserName;
            _factory.Credentials.UserName.Password = _clientCredentials.UserName.Password;
            _factory.Credentials.Windows.ClientCredential.Domain = _clientCredentials.Windows.ClientCredential.Domain;
            _factory.Credentials.Windows.ClientCredential.UserName = _clientCredentials.Windows.ClientCredential.UserName;
            _factory.Credentials.Windows.ClientCredential.Password = _clientCredentials.Windows.ClientCredential.Password;
        }

        protected override System.IdentityModel.Tokens.SecurityToken GetTokenCore(TimeSpan timeout)
        {
            SecurityToken securityToken = null;

            if (this.ValidTokenInCache(_clientCredentials.TokenCache.Token))
            {
                securityToken = _clientCredentials.TokenCache.Token;
            }
            else
            {
                RequestSecurityTokenResponse rstr;
                                
                var rst = new RequestSecurityToken
                {
                    RequestType = RequestTypes.Issue,
                    KeyType = KeyTypes.Symmetric,
                    AppliesTo = TargetAddress
                };

                 securityToken = _factory.CreateChannel().Issue(rst, out rstr);
                _clientCredentials.TokenCache.Token = securityToken;
                _clientCredentials.TokenCache.RawToken = rstr;
            }

            return securityToken;
        }

         private bool ValidTokenInCache(SecurityToken token) 
        { 
            if (token == null)
                return false;

             return (DateTime.UtcNow <= token.ValidTo.ToUniversalTime()); 
        }

         protected override IAsyncResult BeginGetTokenCore(TimeSpan timeout, AsyncCallback callback, object state)
         {
             throw new NotImplementedException("BeginGetTokenCore() not implemented");
         }

         protected override System.IdentityModel.Tokens.SecurityToken EndGetTokenCore(IAsyncResult result)
         {
             throw new NotImplementedException("EndGetTokenCore() not implemented");
         }

         #region IDisposable Members

         void IDisposable.Dispose()
         {
             _factory.Close();
         }

         #endregion

         #region ICommunicationObject Members

         void ICommunicationObject.Abort()
         {
             _factory.Abort();
         }

         IAsyncResult ICommunicationObject.BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
         {
             return _factory.BeginClose(timeout, callback, state);
         }

         IAsyncResult ICommunicationObject.BeginClose(AsyncCallback callback, object state)
         {
             return _factory.BeginClose(callback, state);
         }

         IAsyncResult ICommunicationObject.BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
         {
             return _factory.BeginOpen(timeout, callback, state);
         }

         IAsyncResult ICommunicationObject.BeginOpen(AsyncCallback callback, object state)
         {
             return _factory.BeginOpen(callback, state);
         }

         void ICommunicationObject.Close(TimeSpan timeout)
         {
             _factory.Close(timeout);
         }

         void ICommunicationObject.Close()
         {
             _factory.Close();
         }

         event EventHandler ICommunicationObject.Closed 
         {
             add { _factory.Closed += value; }
             remove { _factory.Closed -= value; }
         }

         event EventHandler ICommunicationObject.Closing
         {
             add { _factory.Closing += value; }
             remove { _factory.Closing -= value; }
         }

         void ICommunicationObject.EndClose(IAsyncResult result)
         {
             _factory.EndClose(result);
         }

         void ICommunicationObject.EndOpen(IAsyncResult result)
         {
             _factory.EndOpen(result);
         }

         event EventHandler ICommunicationObject.Faulted
         {
             add { _factory.Faulted += value; }
             remove { _factory.Faulted -= value; }
         }

         void ICommunicationObject.Open(TimeSpan timeout)
         {
             _factory.Open(timeout);
         }

         void ICommunicationObject.Open()
         {
             _factory.Open();
         }

         event EventHandler ICommunicationObject.Opened
         {
             add { _factory.Opened += value; }
             remove { _factory.Opened -= value; }
         }

         event EventHandler ICommunicationObject.Opening
         {
             add { _factory.Opening += value; }
             remove { _factory.Opening -= value; }
         }

         CommunicationState ICommunicationObject.State
         {
             get { return _factory.State; }
         }

         #endregion
    }
}
