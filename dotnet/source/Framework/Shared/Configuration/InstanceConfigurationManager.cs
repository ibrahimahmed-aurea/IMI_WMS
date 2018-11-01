using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Imi.Framework.Shared.Configuration
{
    public class InstanceConfigurationManager
    {
        private InstanceConfigurationManager()
        { 
            
        }

        public static InstanceDataCollection Instances
        {
            get
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenMachineConfiguration();
                return ((InstanceDataSection)config.GetSection("imi.framework.shared.configuration.instance")).Instances;                
            }
        }

        public static System.Configuration.Configuration GetConfiguration(string instanceName)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenMachineConfiguration();

            InstanceDataSection instanceCollection = (InstanceDataSection)config.GetSection("imi.framework.shared.configuration.instance");
            

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

            return null;
        }
    }
}
