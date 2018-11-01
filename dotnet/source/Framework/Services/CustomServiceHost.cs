using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Imi.Framework.Services
{
    public class CustomServiceHost : ServiceHost
    {
        ServiceModelSectionGroup group;
        public static string ImplementationNamespace;
        public static string ContractNamespace;
        
        public CustomServiceHost(Type serviceType, bool useCompression = true)
            : base(serviceType, new Uri[] {})
        {
            //Add behavior for data compression
            foreach (ServiceEndpoint ep in this.Description.Endpoints)
            {
                foreach (OperationDescription od in ep.Contract.Operations)
                {
                    od.Behaviors.Add(new CompressionOperationBehavior(useCompression));
                }
            }
        }

        public CustomServiceHost(object serviceInstance, bool useCompression = true)
            : base(serviceInstance, new Uri[] { })
        {
            //Add behavior for data compression
            foreach (ServiceEndpoint ep in this.Description.Endpoints)
            {
                foreach (OperationDescription od in ep.Contract.Operations)
                {
                    od.Behaviors.Add(new CompressionOperationBehavior(useCompression));
                }
            }
        }

        private void LoadConfiguration()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            group = ServiceModelSectionGroup.GetSectionGroup(config);
        }

        private void LoadFromTemplate()
        {
            LoadConfiguration();
            
            ServiceElement template = null;

            Type serviceType = Description.ServiceType;
                        
            if (group.Services.Services.Count > 0)
            {
                ServiceElement namedTemplate = null;

                foreach (ServiceElement element in group.Services.Services)
                {
                    if (element.Name.Equals("template"))
                    {
                        template = element;
                        continue;
                    }

                    if (element.Name.Equals(serviceType.Name))
                    {
                        namedTemplate = element;
                        break;
                    }
                }

                if (namedTemplate != null)
                {
                    template = namedTemplate;
                }
                else
                {
                    template.Name = serviceType.FullName;

                    foreach (ServiceEndpointElement endpoint in template.Endpoints)
                    {
                        endpoint.Contract = serviceType.FullName.Replace(string.Format("{0}.{1}", ImplementationNamespace, serviceType.Name), string.Format("{0}.I{1}", ContractNamespace, serviceType.Name));
                    }

                    foreach (BaseAddressElement baseAdress in template.Host.BaseAddresses)
                    {
                        object[] attributes = serviceType.GetCustomAttributes(typeof(ServiceApplicationNameAttribute), false);

                        string serviceName = serviceType.Name;

                        if (attributes.Length > 0)
                        {
                            ServiceApplicationNameAttribute applicationNameAttribute = attributes[0] as ServiceApplicationNameAttribute;
                            serviceName = string.Format("{0}/{1}", applicationNameAttribute.ApplicationName, serviceName);
                        }

                        // Build new uri
                        string uri = baseAdress.BaseAddress;
                        uri = uri.Replace("/serviceName", "/" + serviceName);

                        baseAdress.BaseAddress = uri.ToString();
                    }
                }
            }

            LoadConfigurationSection(template);
        }
        
        protected override void ApplyConfiguration()
        {
            LoadFromTemplate();
        }
    }

}
