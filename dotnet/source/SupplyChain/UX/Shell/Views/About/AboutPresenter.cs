using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using System.Diagnostics;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI;
using System.Windows;
using System.Reflection;
using System.IO;
using Imi.SupplyChain.UX.Shell;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Shell.Services;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class AboutPresenter : Presenter<IAboutView>
    {
        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }

        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }

        public override void OnViewSet()
        {
            base.OnViewSet();

            List<string> moduleIdentifiers = GetModuleIdentifiers();

            View.SetModules(moduleIdentifiers);
            View.Version = ((UserSessionService)UserSessionService).CurrentVersion.ToString();
        }

        private List<string> GetModuleIdentifiers()
        {
            List<string> moduleIdentifiers = new List<string>();

            foreach (IShellModule module in ShellModuleService.Modules)
            {
                moduleIdentifiers.Add(string.Format("{0} {1}", module.Title, module.Version));
            }
            return moduleIdentifiers;
        }

        public override void CloseView()
        {
            base.CloseView();
            View.Close();
        }

        public void CopyInfo()
        {
            try
            {
                View.SetWaitCursor();
                CreateReport();
            }
            finally
            {
                View.SetNormalCursor();
            }
        }

        public void StartSystemInfo()
        {
            ProcessStartInfo info = new ProcessStartInfo("msinfo32.exe");

            info.UseShellExecute = true;
            info.CreateNoWindow = true;

            Process processChild = Process.Start(info);
        }

        public void CreateReport()
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.AppendLine(StringResources.Title);
            sb.AppendLine(((UserSessionService)UserSessionService).CurrentVersion.ToString());
            sb.AppendLine(StringResources.Copyright);
            sb.AppendLine(StringResources.AllRightsReserved); sb.AppendLine(string.Empty);
            
            // Hosted Applications
            sb.AppendLine(StringResources.About_HostedApplications);
            sb.AppendLine("".PadLeft(StringResources.About_HostedApplications.Length, '='));

            List<string> appIdentifiers = GetModuleIdentifiers();

            foreach (string appIdentifier in appIdentifiers)
            {
                sb.AppendLine(appIdentifier);
            }

            sb.AppendLine(string.Empty);
                        
            // List all Dll's with versions
            sb.AppendLine(StringResources.About_FilesAndVersions);
            sb.AppendLine("".PadLeft(StringResources.About_FilesAndVersions.Length, '='));
            sb.AppendLine(string.Empty);

            string applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string[] files = Directory.GetFiles(applicationDirectory);
            List<string> configFiles = new List<string>();
            List<string> otherFiles = new List<string>();

            foreach (string fileName in files)
            {
                string compareName = fileName.ToUpper();

                if ((compareName.EndsWith(".DLL")) || (compareName.EndsWith(".EXE")))
                {
                    sb.AppendLine(string.Format(" * {0}", Path.GetFileName(fileName)));
                    FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName);
                    sb.AppendLine(string.Format("   {0}, {1}", version.FileDescription, version.CompanyName));

                    if(!string.IsNullOrEmpty(version.Comments))
                    {
                        sb.AppendLine(string.Format("   {0}",version.Comments));
                    }

                    sb.AppendLine(string.Format("   {0}/{1}", version.FileVersion, version.ProductVersion));
                    sb.AppendLine(string.Empty);
                }
                else
                {
                    if ((compareName.EndsWith(".CONFIG")) || (compareName.EndsWith(".XML")))
                    {
                        configFiles.Add(fileName);
                    }
                    else
                    {
                        otherFiles.Add(fileName);
                    }
                }
            }

            sb.AppendLine(StringResources.About_OtherFiles);
            sb.AppendLine("".PadLeft(StringResources.About_OtherFiles.Length, '='));

            foreach (string otherFileName in otherFiles)
            {
                sb.AppendLine(Path.GetFileName(otherFileName));
            }

            sb.AppendLine(string.Empty);

            // Contents of all .config files

            sb.AppendLine(StringResources.About_ConfigFileContent);
            sb.AppendLine("".PadLeft(StringResources.About_ConfigFileContent.Length, '='));
            sb.AppendLine(string.Empty);

            foreach (string configFilename in configFiles)
            {
                string fileHead = string.Format("== {0} ", Path.GetFileName(configFilename)).PadRight(100, '=');
                sb.AppendLine(fileHead);
                using (StreamReader r = new StreamReader(configFilename))
                {
                    sb.AppendLine(r.ReadToEnd());
                }

                sb.AppendLine(string.Empty);
            }
            
            Clipboard.SetData(System.Windows.DataFormats.Text, sb.ToString());
        }
    }
}
