using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Deployment
{
    public static class Constants
    {
        // Default Virtual Directory Name
        public const string DefaultVirtualDirectoryName = "IMIMenu";

        // Mage.exe filename (Application for ClickOnce deployment)
        public const string MageExeFilename = "Mage.exe";

        // cert.pfx filename (Certificate file)
        public const string CertificateFilename = "cert.pfx";

        // Path to website template
        public const string WebsiteTemplatePath = "Web";

        // Default Webserver Port
        public const string DefaultWebserverPort = "80";

        // SubPath to WebsiteTemplatePath for all static files 
        public const string WebsiteStaticFilesFolder = "Static";

        // Filename to main webpage template
        public const string WebsiteMainTemplateFile = "Main.template.htm";

        // Filename to redirect template html file
        public const string WebsiteRedirectTemplateFile = "Redirect.template.htm";

        // Website main web filename
        public const string WebsiteMainWebpage = "default.htm";
                
        // Subpath to versions below product directory
        public const string SubPathToVersions = "Versions";
                
        // Configuration file suffix
        public const string ConfigurationfileName = "Imi.SupplyChain.Deployment.config";
                
        // Constant for finding the deploy template in the assembly
        public const string DeployManifestTemplateResource = "Imi.SupplyChain.Deployment.Resources.DeployTemplate.application";
                
        // Name of deploy template file in temp directory
        public const string DeployManifestTemplateTemporaryName = "DeployTemplate.application";

        // Regex pattern to know if deploy manifest was successfully signed
        // Should be: <instancename>.application successfully signed\r\n
        public const string DeployManifestSuccessfulPattern = @"(?n:^\w+\.application successfully signed\r\n$)";

        // Regex pattern to know if application manifest was successfully signed
        // Should be: <executable file>.manifest successfully signed\r\n
        public const string ApplicationManifestSuccessfulPattern = @"(?n:.manifest successfully signed\r\n$)";
    
        // The valid extensions in a zip-files version directory
        public const string ValidZIPVersionDirExtension_Deploy = @".DEPLOY";
        public const string ValidZIPVersionDirExtension_Manifest = @".MANIFEST";

        // Default stage area directory in Common Application Data directory
        public const string DefaultStageAreaDirectoryName = @"Aptean\StagingArea";
    }
}
