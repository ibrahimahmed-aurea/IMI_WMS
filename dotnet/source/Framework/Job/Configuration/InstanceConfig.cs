using System;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using Imi.Framework.Shared.Xml;
using Imi.Framework.Shared.IO;
using Imi.Framework.Shared.Configuration;

namespace Imi.Framework.Job.Configuration
{
    public delegate void ConfigChangedEvent();

    public class InstanceConfig 
    {
        private static System.Configuration.Configuration currentConfig;
        private static ServerInstanceSection currentInstance;
        private static string loadedInstanceName;
        private static FileSystemWatcher configFileWatcher;
        private static string loadErrorMessage;
        public  static ConfigChangedEvent ConfigChanged;

        private InstanceConfig() { }

        public static ServerInstanceSection CurrentInstance
        {
            get
            {
                return (currentInstance);
            }
        }

        public static string GetConnectionString()
        {
            // Get the conectionStrings section.
            ConnectionStringsSection csSection = currentConfig.ConnectionStrings;

            foreach (ConnectionStringSettings cs in csSection.ConnectionStrings)
            {
                if (cs.Name == currentInstance.Database)
                {
                    return (cs.ConnectionString);
                }
            }

            return (null);
        }
	
        public static string LoadErrorMessage
        {
            get
            {
                return (loadErrorMessage);
            }
        }

        public static ConfigurationSection GetSection(string configNamespace)
        {
            return( currentConfig.GetSection(configNamespace) );
            
        }


        public static void LoadInstance(string name)
        {
            currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (currentConfig == null)
            {
                throw (new ConfigurationErrorsException(
                  string.Format(
                  "Failed to load instance information for instance {0}", name)));
            }

            currentInstance = GetSection(ServerInstanceSection.SectionKey) as ServerInstanceSection;

            if (currentConfig == null || currentInstance == null)
            {
                throw (new ConfigurationErrorsException(
                  string.Format(
                  "Failed to load instance information for instance {0} section {1}", name, ServerInstanceSection.SectionKey)));
            }

            loadedInstanceName = name;

            // set up a watch for changes
            if (currentConfig.HasFile)
            {
                if (configFileWatcher == null)
                {
                    FileInfo currentFile = new FileInfo(currentConfig.FilePath);
                    configFileWatcher = new FileSystemWatcher();
                    configFileWatcher.IncludeSubdirectories = false;
                    configFileWatcher.Path = currentFile.DirectoryName;
                    configFileWatcher.Filter = currentFile.Name;
                    configFileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    configFileWatcher.Changed += new FileSystemEventHandler(ConfigFileChanged);
                    configFileWatcher.EnableRaisingEvents = true;
                    currentFile = null;
                }
            }
        }

        private static void ConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;

            try
            {
                configFileWatcher.EnableRaisingEvents = false;

                loadErrorMessage = "";

                if (currentInstance != null)
                {
                    try
                    {
                        LoadInstance(loadedInstanceName);
                    }
                    catch (Exception ex)
                    {
                        loadErrorMessage = string.Format("The server configuration file was changed, the file contains errors.\nThe old configuration will be used until the error has been corrected\nor the server has been restarted.\nError: {0}", ex.Message);
                    }
                }

                if (ConfigChanged != null)
                {
                    ConfigChanged();
                }
            }
            finally
            {
                configFileWatcher.EnableRaisingEvents = true;
            }
        }

    }
}
