using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Imi.Framework.Shared.Configuration
{
    public class ApplicationInstance : MarshalByRefObject
    {

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private string instanceName;

        public string InstanceName
        {
            get { return instanceName; }
        }

        public AppDomain AppDomain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public ApplicationInstance(string instanceName)
        {
            this.instanceName = instanceName;
        }
    }

    /// <summary>
    /// Factory class for creating a application instance isoloated in its own <see cref="AppDomain"/>.
    /// </summary>
    public static class InstanceFactory
    {
        /// <summary>
        /// Creates a application instance isolated in its own <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="assemblyName">The assembly containing the instance.</param>
        /// <param name="instanceTypeName">The type name of the instance to create.</param>
        /// <param name="instanceName">A friendly name of the instance.</param>
        /// <returns>An application proxy object which can be used to access the created instance.</returns>
        public static T CreateInstance<T>(string assemblyName, string instanceTypeName, string instanceName)
            where T : ApplicationInstance
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ConfigurationFile = InstanceConfigurationProvider.GetInstance(instanceName).ConfigurationFile;

            AppDomain domain = AppDomain.CreateDomain(instanceName, null, setup);
            return (T)domain.CreateInstanceAndUnwrap(assemblyName, instanceTypeName, true, BindingFlags.CreateInstance, null, new object[] { instanceName }, null, null);
        }

        /// <summary>
        /// Creates a application instance isolated in its own <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="assemblyName">The assembly containing the instance.</param>
        /// <param name="instanceTypeName">The type name of the instance to create.</param>
        /// <param name="instanceName">A friendly name of the instance.</param>
        /// <param name="domain">The <see cref="AppDomain"/> the instance should be created in.</param>
        /// <returns>An application proxy object which can be used to access the created instance.</returns>
        public static T CreateInstance<T>(string assemblyName, string instanceTypeName, string instanceName, AppDomain domain)
            where T : ApplicationInstance
        {
            if (domain == null)
                throw new ArgumentNullException("domain");

            return (T)domain.CreateInstanceAndUnwrap(assemblyName, instanceTypeName, true, BindingFlags.CreateInstance, null, new object[] { instanceName }, null, null);
        }

    }
}
