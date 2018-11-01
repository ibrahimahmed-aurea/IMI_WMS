using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;
using System.Configuration;
using System.Runtime.Remoting.Proxies;

namespace Imi.Framework.UX.Services
{
    
    /// <summary>
    /// Indicates that property or parameter is a dependency on a service and
    /// should be dependency injected when the class is put into a <see cref="WorkItem"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class WcfServiceDependencyAttribute : OptionalDependencyAttribute
    {
        private Type type;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDependencyAttribute"/> class.
        /// </summary>
        public WcfServiceDependencyAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the type of the service the property expects.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
        /// </summary>
        public override IParameter CreateParameter(Type memberType)
        {
            return new ServiceDependencyParameter(type ?? memberType, Required);
        }

        class ServiceDependencyParameter : IParameter
        {
            Type serviceType;
            bool ensureExists;

            public ServiceDependencyParameter(Type serviceType, bool ensureExists)
            {
                this.serviceType = serviceType;
                this.ensureExists = ensureExists;
            }

            public Type GetParameterType(IBuilderContext context)
            {
                return serviceType;
            }

            public object GetValue(IBuilderContext context)
            {
                WorkItem workItem = (WorkItem)context.Locator.Get(new DependencyResolutionLocatorKey(typeof(WorkItem), null));

                IChannelFactoryService channelFactory = workItem.Services.Get<IChannelFactoryService>(ensureExists);

                return channelFactory.CreateChannel(serviceType);
            }
        }
    }
}