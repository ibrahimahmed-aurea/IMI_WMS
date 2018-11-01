using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Configuration;
using System.Globalization;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace Imi.Framework.Services
{
    public sealed class ApplicationContextExtension : BehaviorExtensionElement, IDispatchMessageInspector, IServiceBehavior
    {
        public ApplicationContextExtension()
        {
        }
                                                
        private static string GetUserId(Message message)
        {
            int headerIndex = message.Headers.FindHeader("UserId", RequestMessageBase.HeaderNamespace);

            string userId = null;

            if (headerIndex > -1)
                userId = message.Headers.GetHeader<string>(headerIndex);

            return userId;
        }

        private static string GetTerminalId(Message message)
        {
            int headerIndex = message.Headers.FindHeader("TerminalId", RequestMessageBase.HeaderNamespace);

            string terminalId = null;

            if (headerIndex > -1)
                terminalId = message.Headers.GetHeader<string>(headerIndex);

            return terminalId;
        }

        private static string GetUICulture(Message message)
        {
            int headerIndex = message.Headers.FindHeader("UICulture", RequestMessageBase.HeaderNamespace);

            string uiCulture = null;

            if (headerIndex > -1)
                uiCulture = message.Headers.GetHeader<string>(headerIndex);

            return uiCulture;
        }

        private static string GetSessionId(Message message)
        {
            int headerIndex = message.Headers.FindHeader("SessionId", RequestMessageBase.HeaderNamespace);

            string sessionId = null;

            if (headerIndex > -1)
                sessionId = message.Headers.GetHeader<string>(headerIndex);

            return sessionId;
        }
        
        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            ApplicationContext current = new ApplicationContext();
            current.UserId = GetUserId(request);
            current.TerminalId = GetTerminalId(request);
            current.SessionId = GetSessionId(request);
            
            string uiCulture = GetUICulture(request);

            if (!string.IsNullOrEmpty(uiCulture))
                current.UICulture = new CultureInfo(uiCulture);

            ApplicationContext.SetContext(current);

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion

        public override Type BehaviorType
        {
            get 
            {
                return typeof(ApplicationContextExtension);
            }
        }

        protected override object CreateBehavior()
        {
            return new ApplicationContextExtension();
        }

        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase dispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher dispatcher = dispatcherBase as ChannelDispatcher;

                foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
