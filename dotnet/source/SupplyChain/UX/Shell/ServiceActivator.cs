using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;
using Imi.SupplyChain.UX.Shell.Configuration;

namespace Imi.SupplyChain.UX.Shell
{
    public class ServiceActivator
    {
        public static TService CreateInstance<TService>(params object[] list)
            where TService : class
        {
            ShellConfigurationSection settings = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;

            foreach (ServiceElement element in settings.ServiceElementCollection)
            {
                if (element.ServiceType == typeof(TService))
                {
                    return Activator.CreateInstance(element.InstanceType, list) as TService;
                }
            }

            throw new ServiceMissingException(typeof(TService));
        }
    }
}
