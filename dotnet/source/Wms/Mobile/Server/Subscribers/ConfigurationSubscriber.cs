using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Messaging.Adapter.Warehouse;
using Imi.Wms.Mobile.Server.Adapter;
using Imi.Wms.Mobile.Server.Interface;
using Imi.Wms.Mobile.Server.Configuration;
using System.Configuration;

namespace Imi.Wms.Mobile.Server.Subscribers
{
    [SessionPolicy(SessionPolicy.None)]
    public class ConfigurationSubscriber : MessageSubscriber<ConfigurationRequest, ConfigurationResponse>
    {
        public override void Invoke(ConfigurationRequest request)
        {
            ServerSection config = ConfigurationManager.GetSection(ServerSection.SectionKey) as ServerSection;

            ConfigurationResponse response = new ConfigurationResponse();

            var apps = new List<Application>();

            foreach (ApplicationElement e in config.ApplicationCollection)
            {
                apps.Add(new Application() { Name = e.Name });
            }

            response.Applications = apps.ToArray();

            TransmitResponseMessage(response);
        }
    }
}
