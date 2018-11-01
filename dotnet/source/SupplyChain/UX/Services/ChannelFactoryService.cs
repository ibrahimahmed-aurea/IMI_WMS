using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Services;
using Imi.Framework.Services;
using Imi.Framework.UX;
using Imi.Framework.UX.Identity;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Services
{
    public class ChannelFactoryService : IDisposable, IChannelFactoryService
    {
        private IUserSessionService _userSessionService;
        private IDictionary<Type, object> _channelPoolDictionary;
        private bool _isDisposed;
        private const int _maxChannelPoolSize = 2;
        private SecurityTokenCache _tokenCache;

        private class SetMaxFaultSizeBehavior : IEndpointBehavior
        {
            int size;

            public SetMaxFaultSizeBehavior(int size)
            {
                this.size = size;
            }

            public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
            {
                clientRuntime.MaxFaultSize = size;
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
            {
            }

            public void Validate(ServiceEndpoint endpoint)
            {
            }
        }
                                
        [InjectionConstructor]
        public ChannelFactoryService([ServiceDependency] IUserSessionService userSessionService, SecurityTokenCache tokenCache)
        {
            _userSessionService = userSessionService;
            _channelPoolDictionary = new Dictionary<Type, object>();
            _tokenCache = tokenCache;
            
            if (_tokenCache == null)
            {
                _tokenCache = new SecurityTokenCache();
            }

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    return true;
                }
                else if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) 
                {
                    if (chain.ChainStatus.Count() == 1)
                    {
                        if (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot || chain.ChainStatus[0].Status == X509ChainStatusFlags.PartialChain)
                        {
                            //Accept self-signed certificates
                            return true;
                        }
                    }
                }

                return false;
            });
        }

        public ChannelFactoryService([ServiceDependency] IUserSessionService userSessionService)
            : this(userSessionService, null)
        { 
        }

        ~ChannelFactoryService()
        {
            Dispose(false);
        }
                        
        public object CreateChannel(Type channelType)
        {
            return CreateProxy(channelType).GetTransparentProxy();
        }
                
        private object CreateChannelPool(Type channelType)
        {
            lock (_channelPoolDictionary)
            {
                if (!_channelPoolDictionary.ContainsKey(channelType))
                {
                    string serviceName = channelType.Name;
                    string baseAddress = null;

                    object[] attributes = channelType.GetCustomAttributes(typeof(ServiceApplicationNameAttribute), false);

                    if (attributes.Count() > 0)
                    {
                        ServiceApplicationNameAttribute applicationNameAttribute = attributes[0] as ServiceApplicationNameAttribute;
                        baseAddress = applicationNameAttribute.ApplicationName;
                    }

                    Type factoryType = typeof(CustomChannelFactory<>);
                    Type[] typeArgs = { channelType };

                    Type genericFactoryType = factoryType.MakeGenericType(typeArgs);
                    object[] args = { serviceName, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, baseAddress, _userSessionService.HostName, _userSessionService.HostPort };
                        
                    /* Create the ChannelFactory */
                    ChannelFactory factory = Activator.CreateInstance(genericFactoryType, args) as ChannelFactory;

                    factory.Credentials.UserName.UserName = _userSessionService.DomainUser;
                    
                    if ( _userSessionService.Password != null)
                    {
                        factory.Credentials.Windows.ClientCredential.UserName = _userSessionService.UserId;
                        factory.Credentials.Windows.ClientCredential.Domain = _userSessionService.Domain;
                        factory.Credentials.Windows.ClientCredential.SecurePassword = _userSessionService.Password;

                        IntPtr password = SecureStringHelper.GetString(_userSessionService.Password);
                        factory.Credentials.UserName.Password = Marshal.PtrToStringAuto(password);
                        SecureStringHelper.FreeString(password);
                    }
                                        
                    ClientCredentials clientCredentials = factory.Endpoint.Behaviors.Remove<ClientCredentials>();
                    CachedClientCredentials cachedClientCredentials = new CachedClientCredentials(_tokenCache, clientCredentials);
                    factory.Endpoint.Behaviors.Add(cachedClientCredentials);

                    //Set behavior for FaultException size.
                    //To be able to handle large exception messages.
                    factory.Endpoint.Behaviors.Add(new SetMaxFaultSizeBehavior(100000000));

                    Type channelPoolType = typeof(ChannelPool<>);

                    Type genericChannelPoolType = channelPoolType.MakeGenericType(typeArgs);
                    object[] args2 = { _maxChannelPoolSize, factory };

                    /* Create the ChannelFactory */
                    object channelPool = Activator.CreateInstance(genericChannelPoolType, args2);

                    _channelPoolDictionary[channelType] = channelPool;
                }

                return _channelPoolDictionary[channelType];
            }
        }
               
        private RealProxy CreateProxy(Type channelType)
        {
            Type genericProxyType = typeof(WcfServiceProxy<>);
            Type[] typeArgs = { channelType };

            Type proxyType = genericProxyType.MakeGenericType(typeArgs);
            object[] args = { CreateChannelPool(channelType), _userSessionService };

            /* Create and return the WcfServiceProxy */
            return Activator.CreateInstance(proxyType, args) as RealProxy;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    foreach (IDisposable disposable in _channelPoolDictionary.Values.Cast<IDisposable>())
                    {
                        disposable.Dispose();
                    }
                }
            }

            _isDisposed = true;
        }
    }
}
