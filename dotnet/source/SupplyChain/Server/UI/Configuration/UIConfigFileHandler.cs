using System;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Reflection;
using Imi.Framework.Shared.IO;
using Imi.Framework.Shared.Xml;
using Imi.SupplyChain.Server.UI.Configuration;
using System.Collections.Generic;

namespace Imi.SupplyChain.Server.UI
{
    /// <summary>
    /// Summary description for UIConfig.
    /// </summary>
    public class UIConfigFileHandler 
    {
        private static UIConfigFileHandler instance;
        private ServerConfig ServerConfig;
        private const string defaultNameSpace = "Imi.SupplyChain.Server.UI.Configuration";
        private String activeConfigFileName;

        private UIConfigFileHandler(String fileName)
        {
            activeConfigFileName = fileName;
            this.Load();
            instance = this;
        }

        private UIConfigFileHandler() : this(FileIO.FindAppConfigFile("InstanceRepositoryFile", "client.config"))
        {
        }

        public static UIConfigFileHandler Instance()
        {
            if (instance == null)
            {
                instance = new UIConfigFileHandler();
            }

            return (instance);
        }

        public ServerConfig Config
        {
            get { return ServerConfig; }
            set { ServerConfig = value; }
        }

        private List<ServerType> GetConnections(FolderType folder)
        {
            List<ServerType> l = new List<ServerType>();

            if (folder.Items != null)
            {
                foreach (object o in folder.Items)
                {
                    if (o is ServerType)
                    {
                        l.Add(o as ServerType);
                    }
                    else if (o is FolderType)
                    {
                        FolderType f = (o as FolderType);
                        List<ServerType> cl = GetConnections(f);
                        l.AddRange(cl);
                    }
                }
            }

            return (l);
        }

        public List<ServerType> Connections
        {
            get
            {
                return (GetConnections(ServerConfig.RootFolder));
            }
        }

        public void Save(ServerConfig version)
        {
            XmlSerializer s = new XmlSerializer(typeof(ServerConfig));
            StreamWriter writer = null;

            try
            {
                try
                {
                    writer = new StreamWriter(activeConfigFileName);
                }
                catch (Exception e)
                {
                    throw (new ConfigurationErrorsException(
                      String.Format(
                      "Problems opening the Instance Repository File, file = \"{0}\"", activeConfigFileName), e));
                }

                try
                {
                    // make sure refreshradte is always specified
                    // always add broadcast if missing ?
                    s.Serialize(writer, version);
                    writer.Close();
                    ServerConfig = version;
                }
                catch (Exception e)
                {
                    throw (new ConfigurationErrorsException(
                      String.Format(
                      "Problems writing the instance repository file, file = \"{0}\"", activeConfigFileName), e));
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }

        public void Save()
        {
            Save(ServerConfig);
        }

        public void Load()
        {
            //
            // Check if instance file exists
            //
            if (!File.Exists(activeConfigFileName))
            {
                throw (new ConfigurationErrorsException(
                  String.Format(
                  "The Instance Repository File is missing, please check the configuration. File = \"{0}\"", activeConfigFileName)));
            }
            else
            {
                // Read content
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerConfig));

                StreamReader reader = null;

                try
                {
                    try
                    {
                        reader = new StreamReader(activeConfigFileName);
                    }
                    catch (Exception ex)
                    {
                        throw (new ConfigurationErrorsException(
                          String.Format(
                          "Problems reading the Instance Repository File, file = \"{0}\"", activeConfigFileName), ex));
                    }

                    // Validate the contents
                    String schemaFileName = String.Format("{0}.xsd.ServerConfig.xsd", defaultNameSpace);
                    StreamReader schemaReader;

                    try
                    {
                        schemaReader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(schemaFileName));
                    }
                    catch (Exception ex)
                    {
                        throw (new ConfigurationErrorsException(
                          String.Format(
                          "Failed to load embedded schema file, cannot validate instance repository file. Schema file = \"{0}\"", schemaFileName), ex));
                    }

                    // Validate file, get 10 errors max
                    XmlValidator xmlValidator = new XmlValidator();
                    String errors = xmlValidator.ValidateStream(reader.BaseStream, schemaReader.BaseStream, 10);

                    if (errors != "")
                    {
                        throw (new ConfigurationErrorsException(
                          String.Format(
                          "The instance repository file contains syntax errors, file = \"{0}\"\n{1}", activeConfigFileName, errors)));
                    }

                    try
                    {
                        reader.BaseStream.Position = 0;
                        ServerConfig = xmlSerializer.Deserialize(reader) as ServerConfig;

                        #region CompensateBadConfig
                        // make sure refreshrate is always specified
                        if (!ServerConfig.RefreshRateSpecified)
                            ServerConfig.RefreshRate = ServerConfigRefreshRate.Normal;

                        // Make sure at least one Broadcast entry exists
                        if ((ServerConfig.Broadcast == null) || (ServerConfig.Broadcast.Length == 0))
                        {
                            ServerConfig.Broadcast = new BroadcastType[1];
                            ServerConfig.Broadcast[0].Ip = "239.255.255.255";
                            ServerConfig.Broadcast[0].Port = 11000;
                            ServerConfig.Broadcast[0].Timeout = 5000;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw (new ConfigurationErrorsException(
                          String.Format(
                          "Problems reading the instance repository file, file = \"{0}\"", activeConfigFileName), ex));
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }
    }
}