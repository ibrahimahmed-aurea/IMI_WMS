using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using Imi.Framework.Shared.IO;

namespace Imi.Framework.Shared.Configuration
{
    public sealed class InstanceConfigurationProvider
    {
        private InstanceConfigurationProvider()
        { 
        
        }

        private static System.Configuration.Configuration OpenExeConfiguration(ConfigurationUserLevel level)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(level);

            if (!config.HasFile || (config.HasFile && (config.GetSection("imi.framework.shared.configuration") == null)))
            {
                string fileName = FileIO.FindConfigFile("instance.config");
                if (!string.IsNullOrEmpty(fileName))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = fileName;
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, level);
                }
            }

            return (config);
        }

        public static InstanceDataCollection Instances
        {
            get
            {
                System.Configuration.Configuration config = OpenExeConfiguration(ConfigurationUserLevel.None);
                return ((SharedConfigurationSection)config.GetSection("imi.framework.shared.configuration")).Instances;
            }
        }

        public static InstanceDataElement GetInstance(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentNullException("instanceName");

            System.Configuration.Configuration config = OpenExeConfiguration(ConfigurationUserLevel.None);
            SharedConfigurationSection section = (SharedConfigurationSection)config.GetSection("imi.framework.shared.configuration");

            if (section == null)
            {
                throw new ConfigurationErrorsException("There are no instances configured in machine.config or app.config.");
            }

            if (section.Instances[instanceName] == null)
                throw new ConfigurationErrorsException(string.Format("Could not load configuration for instance: \"{0}\".", instanceName));

            if (!File.Exists(section.Instances[instanceName].ConfigurationFile))
                throw new FileNotFoundException(section.Instances[instanceName].ConfigurationFile);

            return section.Instances[instanceName];
        }


        public static System.Configuration.Configuration GetConfiguration(string instanceName)
        {
            InstanceDataElement instance = GetInstance(instanceName);

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = instance.ConfigurationFile;
            
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }
    }
}
