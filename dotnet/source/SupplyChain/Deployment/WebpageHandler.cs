using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Imi.SupplyChain.Deployment.Wrappers;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment
{
    public static class WebpageHandler
    {
        static WebpageHandler() { }

        public static bool UpdateWebpage(ConfigurationSettings config, IList<ProductStandard> installedProducts)
        {
            ParseTemplate parseTemplate = new ParseTemplate(config, installedProducts);

            string webcontentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constants.WebsiteTemplatePath);
            string templateFile = Path.Combine(webcontentPath, Constants.WebsiteMainTemplateFile);
            string redirectTemplateFile = Path.Combine(webcontentPath, Constants.WebsiteRedirectTemplateFile);
            string webpage = Path.Combine(config.GetMainVirtualDirectoryPath(), Constants.WebsiteMainWebpage);
                                    
            parseTemplate.DoParse(templateFile, webpage);

            // Copy static files
            if (!FileHandler.CopyDirectory(Path.Combine(webcontentPath, Constants.WebsiteStaticFilesFolder), config.GetMainVirtualDirectoryPath()))
            {
                return false;
            }
            
            return true;
        }
    }
}
