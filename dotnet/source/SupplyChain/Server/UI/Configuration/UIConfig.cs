using System;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Reflection;
using Imi.Framework.Shared.IO;
using Imi.Framework.Shared.Xml;
using Imi.Wms.Server.UI.Configuration;
using System.Collections.Generic;

namespace Imi.Wms.Server.UI
{
    /// <summary>
    /// Summary description for UIConfig.
    /// </summary>
    public class UIConfigFileHandler : ServerList
    {
        private static UIConfigFileHandler instance;
        private ServerList serverList;
        private const string defaultNameSpace = "Imi.Wms.Server.UI.Configuration";
        private String activeConfigFileName;

        private UIConfigFileHandler(String fileName)
        {
            activeConfigFileName = fileName;
            instance = this;
        }

        private UIConfigFileHandler()
        {
            String repoFileName = FileIO.FindAppConfigFile("InstanceRepositoryFile", "client.config");
            return(new UIConfigFileHandler(repoFileName));
        }

        public static UIConfigFileHandler Instance()
        {
            if (instance == null)
            {
                instance = new UIConfigFileHandler();
            }

            return (instance);
        }

        public ServerList Config
        {
            get { return serverList; }
            set { serverList = value; }
        }

        public List<ServerType> GetConnections(FolderType folder)
        {
            List<ServerType> l = new List<ServerType>();

            foreach (object o in folder.Items)
            {
                if (o is ServerType)
                {
                    l.Add(o as ServerType);
                }
                else if (o is FolderType)
                {
                    FolderType f = (o as FolderType);
                    List<ServerType> cl = GetConnections(f.Items);
                    l.AddRange(cl);
                }
            }
        }

        public List<ServerType> Connections
        {
            get
            {
                return (serverList.Folder);
            }
        }

        public void Save(ServerList version)
        {
            XmlSerializer s = new XmlSerializer(typeof(ServerList));
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
                    serverList = version;
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

        public void Load()
        {
            //
            // Check if instance file exists
            //
            if (!File.Exists(activeConfigFileName))
            {
                return;
            }
            else
            {
                // Read content
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerList));

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
                        serverList = xmlSerializer.Deserialize(reader) as ServerList;

                        // make sure refreshrate is always specified

                        //if (serverList.RefreshRateSpecified)
                        //    refreshRate = serverList.RefreshRate;
                        //else
                        //    refreshRate = ServerListRefreshRate.Normal;

                        // Set default values for broadcast if missing

                        //if ((serverList.Broadcast != null) && (serverList.Broadcast.Length > 0))
                        //else
                        //{
                        //    broadcast.Ip = "239.255.255.255";
                        //    broadcast.Port = 11000;
                        //    broadcast.Timeout = 5000;
                        //}
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