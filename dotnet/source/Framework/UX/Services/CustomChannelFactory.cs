using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Security.Cryptography;
//using System.IdentityModel.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace Imi.Framework.UX.Services
{
    /// <summary>
    /// Custom client channel. Allows to specify a different configuration file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomChannelFactory<T> : ChannelFactory<T>
    {
        string configurationPath;
        string hostString;
        string baseAddress;
        string endpointConfigurationName;
        Dictionary<string, ChannelEndpointElement> endPoints = new Dictionary<string, ChannelEndpointElement>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(string configurationPath, string hostName, int port)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            base.InitializeEndpoint((string)null, null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(Binding binding, string configurationPath, string baseAddress, string hostName, int port)
            : this(binding, (EndpointAddress)null, configurationPath, baseAddress, hostName, port)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(ServiceEndpoint serviceEndpoint, string configurationPath, string baseAddress,string hostName, int port)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.hostString =  string.Format("{0}:{1}", hostName, port);
            this.baseAddress = baseAddress;
            base.InitializeEndpoint(serviceEndpoint);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(string endpointConfigurationName, string configurationPath, string baseAddress, string hostName, int port)
            : this(endpointConfigurationName, null, configurationPath, baseAddress, hostName, port)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="endpointAddress"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(Binding binding, EndpointAddress endpointAddress, string configurationPath, string baseAddress, string hostName, int port)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.hostString = string.Format("{0}:{1}", hostName, port);
            this.baseAddress = baseAddress;
            base.InitializeEndpoint(binding, endpointAddress);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(Binding binding, string remoteAddress, string configurationPath, string baseAddress, string hostName, int port)
            : this(binding, new EndpointAddress(remoteAddress), configurationPath, baseAddress, hostName, port)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="endpointAddress"></param>
        /// <param name="configurationPath"></param>
        public CustomChannelFactory(string endpointConfigurationName, EndpointAddress endpointAddress, string configurationPath, string baseAddress, string hostName, int port)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.hostString = string.Format("{0}:{1}", hostName, port);
            this.baseAddress = baseAddress;
            this.endpointConfigurationName = endpointConfigurationName;
            base.InitializeEndpoint(endpointConfigurationName, endpointAddress);
        }

        /// <summary>
        /// Loads the serviceEndpoint description from the specified configuration file
        /// </summary>
        /// <returns></returns>
        protected override ServiceEndpoint CreateDescription()
        {
            ServiceEndpoint serviceEndpoint = base.CreateDescription();

            if (endpointConfigurationName != null)
                serviceEndpoint.Name = endpointConfigurationName;

            //Add behavior for data decompression
            foreach (OperationDescription od in serviceEndpoint.Contract.Operations)
            {
                od.Behaviors.Add(new DecompressionOperationBehavior());
            }

            ChannelEndpointElement selectedEndpoint = null;

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = this.configurationPath;

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ServiceModelSectionGroup group = ServiceModelSectionGroup.GetSectionGroup(config);

            if (group.Client.Endpoints.Count > 0)
            {
                ChannelEndpointElement template = null;
                ChannelEndpointElement specific = null;

                foreach (ChannelEndpointElement endPointElement in group.Client.Endpoints)
                {
                    if (endPointElement.Name.Equals("serviceInterface"))
                        template = endPointElement;

                    // Match contract name i.e. f.ex. ISessionService
                    if (endPointElement.Name.Equals(serviceEndpoint.Name))
                    {
                        specific = endPointElement;
                        break; // got what we wanted
                    }
                }

                if (specific == null)
                {
                    // Use template
                    ChannelEndpointElement endpoint = template;

                    // Assume the service name starts with I f.ex. ISessionService, remove the I
                    string serviceName = serviceEndpoint.Name.Substring(1);

                    if (baseAddress != null)
                        serviceName = string.Format("{0}/{1}", baseAddress, serviceName);

                    // Build new uri
                    string uri = endpoint.Address.ToString();
                    uri = uri.Replace("hostname", hostString).Replace("/serviceName", "/" + serviceName);

                    // Modify endpoint object
                    endpoint.Address = new Uri(uri);
                    endpoint.Contract = serviceEndpoint.Contract.ContractType.FullName;
                    endpoint.Name = serviceEndpoint.Name;

                    selectedEndpoint = endpoint;
                }
                else
                {
                    // Use specific config
                    selectedEndpoint = specific;
                }
            }

            if (selectedEndpoint != null)
            {
                if (serviceEndpoint.Binding == null)
                {
                    serviceEndpoint.Binding = CreateBinding(selectedEndpoint.Binding, group);
                    
                    WS2007FederationHttpBinding federatedBinding = serviceEndpoint.Binding as WS2007FederationHttpBinding;

                    if (federatedBinding != null)
                    { 
                        if (federatedBinding.Security.Mode == WSFederationHttpSecurityMode.TransportWithMessageCredential)
                        {
                            // Build new uri
                            string uri = federatedBinding.Security.Message.IssuerAddress.ToString();
                            uri = uri.Replace("hostname", hostString);

                            federatedBinding.Security.Message.IssuerAddress = new EndpointAddress(new Uri(uri), federatedBinding.Security.Message.IssuerAddress.Identity, federatedBinding.Security.Message.IssuerAddress.Headers);
                        }
                    }
                }

                if (serviceEndpoint.Address == null)
                {
                    serviceEndpoint.Address = new EndpointAddress(selectedEndpoint.Address, GetIdentity(selectedEndpoint.Identity), selectedEndpoint.Headers.Headers);
                }

                if (serviceEndpoint.Behaviors.Count == 0 && selectedEndpoint.BehaviorConfiguration != null)
                {
                    AddBehaviors(selectedEndpoint.BehaviorConfiguration, serviceEndpoint, group);
                }

                serviceEndpoint.Name = selectedEndpoint.Contract;
            }

            return serviceEndpoint;

        }

        /// <summary>
        /// Configures the binding for the selected endpoint
        /// </summary>
        /// <param name="bindingName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private Binding CreateBinding(string bindingName, ServiceModelSectionGroup group)
        {
            BindingCollectionElement bindingElementCollection = group.Bindings[bindingName];
            if (bindingElementCollection.ConfiguredBindings.Count > 0)
            {
                IBindingConfigurationElement be = bindingElementCollection.ConfiguredBindings[0];

                Binding binding = GetBinding(be);
                if (be != null)
                {
                    be.ApplyConfiguration(binding);
                }

                return binding;
            }

            return null;
        }

        /// <summary>
        /// Helper method to create the right binding depending on the configuration element
        /// </summary>
        /// <param name="configurationElement"></param>
        /// <returns></returns>
        private Binding GetBinding(IBindingConfigurationElement configurationElement)
        {
            if (configurationElement is CustomBindingElement)
                return new CustomBinding();
            else if (configurationElement is BasicHttpBindingElement)
                return new BasicHttpBinding();
            else if (configurationElement is NetMsmqBindingElement)
                return new NetMsmqBinding();
            else if (configurationElement is NetNamedPipeBindingElement)
                return new NetNamedPipeBinding();
            else if (configurationElement is NetPeerTcpBindingElement)
                return new NetPeerTcpBinding();
            else if (configurationElement is NetTcpBindingElement)
                return new NetTcpBinding();
            else if (configurationElement is WSDualHttpBindingElement)
                return new WSDualHttpBinding();
            else if (configurationElement is WSHttpBindingElement)
                return new WSHttpBinding();
            else if (configurationElement is WS2007FederationHttpBindingElement)
                return new WS2007FederationHttpBinding();
            else if (configurationElement is WSFederationHttpBindingElement)
                return new WSFederationHttpBinding();

            return null;
        }

        /// <summary>
        /// Adds the configured behavior to the selected endpoint
        /// </summary>
        /// <param name="behaviorConfiguration"></param>
        /// <param name="serviceEndpoint"></param>
        /// <param name="group"></param>
        private void AddBehaviors(string behaviorConfiguration, ServiceEndpoint serviceEndpoint, ServiceModelSectionGroup group)
        {
            if (!string.IsNullOrEmpty(behaviorConfiguration))
            {
                EndpointBehaviorElement behaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
                for (int i = 0; i < behaviorElement.Count; i++)
                {
                    BehaviorExtensionElement behaviorExtension = behaviorElement[i];
                    object extension = behaviorExtension.GetType().InvokeMember("CreateBehavior",
                        BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                        null, behaviorExtension, null);
                    if (extension != null)
                    {
                        serviceEndpoint.Behaviors.Add((IEndpointBehavior)extension);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the endpoint identity from the configuration file
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private EndpointIdentity GetIdentity(IdentityElement element)
        {
            EndpointIdentity identity = null;
            PropertyInformationCollection properties = element.ElementInformation.Properties;
            if (properties["userPrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
            }
            if (properties["servicePrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
            }
            if (properties["dns"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
            }
            if (properties["rsa"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
            }
            if (properties["certificate"].ValueOrigin != PropertyValueOrigin.Default)
            {
                X509Certificate2Collection supportingCertificates = new X509Certificate2Collection();
                supportingCertificates.Import(Convert.FromBase64String(element.Certificate.EncodedValue));
                if (supportingCertificates.Count == 0)
                {
                    throw new InvalidOperationException("UnableToLoadCertificateIdentity");
                }
                X509Certificate2 primaryCertificate = supportingCertificates[0];
                supportingCertificates.RemoveAt(0);
                return EndpointIdentity.CreateX509CertificateIdentity(primaryCertificate, supportingCertificates);
            }

            return identity;
        }


        protected override void ApplyConfiguration(string configurationName)
        {
            //base.ApplyConfiguration(configurationName);
        }
    }

}
