using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using System.IO;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.Framework.UX.Services;

namespace Imi.SupplyChain.UX.Shell
{
    public class ConfigHelper
    {
        public static string LoadClientConfig(IChannelFactoryService channelFactoryService)
        {
            string configFileName = null;
                                    
            FileService fileService = new FileService();
            fileService.SettingsService = channelFactoryService.CreateChannel(typeof(ISettingsService)) as ISettingsService;

            if (fileService.SettingsService != null)
            {
                string configFileText = fileService.GetFile("client.config");
                configFileName = CreateTempFile(configFileText);
            }
            
            return configFileName;
        }


        private static string CreateTempFile(string configFileText)
        {
            string configFileName = Path.GetTempFileName();

            using (StreamWriter w = new StreamWriter(configFileName, false, Encoding.UTF8))
            {
                w.Write(configFileText);
                w.Close();
            }

            return configFileName;
        }
    }
}
