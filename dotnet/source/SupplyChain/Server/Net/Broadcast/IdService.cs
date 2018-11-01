using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Text;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Imi.Framework.Shared.IO;
using Imi.Framework.Shared.Xml;
using Imi.SupplyChain.Server.Net.Broadcast;

namespace Imi.SupplyChain.Server.Net.Broadcast
{
    public class IdService
    {
        private const string defaultNameSpace = "Imi.SupplyChain.Server.Net.Broadcast";
        // use same as client config
        // do the client config once on server and use broadvast to dstribute it to clients.
        // who of course will cache it.
        // client should display source of information as well in the dialogue.
        // use local client to populate file ? (or editor)

        // TODO: add read of configuration (steal from client code)
        // TODO: create service out of this
        // TODO: react to changes in config file.
        private const string EMPTYLIST = "<ServerList/>";
        private string idList = EMPTYLIST;
        private FileSystemWatcher fsw;
        private BroadcastServer server = null;
        private BroadcastSetup conf;

        public void Config()
        {
            string setupFileName = FileIO.FindAppConfigFile("BroadcastConfigFile", "broadcast.config");

            //
            // Check that instance file exists and is readable
            //

            // Read content
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BroadcastSetup));

            StreamReader reader = null;

            try
            {
                try
                {
                    reader = new StreamReader(setupFileName);
                }
                catch (Exception e)
                {
                    throw (new ConfigurationErrorsException(
                      string.Format(
                      "Problems reading the broadcast config File, file = \"{0}\"", setupFileName), e));
                }

                // Validate the contents
                string schemaFilename = string.Format("{0}.xsd.BroadcastSetup.xsd", defaultNameSpace);
                StreamReader schemaReader;

                try
                {
                    schemaReader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(schemaFilename));
                }
                catch (Exception e)
                {
                    throw (new ConfigurationErrorsException(
                      string.Format(
                      "Failed to load embedded schema file, cannot validate broadcast config file. Schema file = \"{0}\"", schemaFilename), e));
                }

                // Validate file, get 10 errors max
                XmlValidator xmlValidator = new XmlValidator();
                string errors = xmlValidator.ValidateStream(reader.BaseStream, schemaReader.BaseStream, 10);

                if (errors != "")
                {
                    throw (new ConfigurationErrorsException(
                      string.Format(
                      "The broadcast config file contains syntax errors, file = \"{0}\"\n{1}", setupFileName, errors)));
                }

                conf = null;

                try
                {
                    reader.BaseStream.Position = 0;
                    conf = xmlSerializer.Deserialize(reader) as BroadcastSetup;
                }
                catch (Exception e)
                {
                    throw (new ConfigurationErrorsException(
                      string.Format(
                      "Problems reading the broadcast config file, file = \"{0}\"", setupFileName), e));
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

        public IdService()
        {
            Config();

            LoadFile();

            // set up a watch for changes
            if (fsw == null)
            {
                FileInfo currentFile = new FileInfo(conf.FileName);
                fsw = new FileSystemWatcher();
                fsw.IncludeSubdirectories = false;
                fsw.Path = currentFile.DirectoryName;
                fsw.Filter = currentFile.Name;
                fsw.NotifyFilter = NotifyFilters.LastWrite;
                fsw.Changed += new FileSystemEventHandler(ConfigFileChanged);
                fsw.EnableRaisingEvents = true;
                currentFile = null;
            }
        }

        private void LoadFile()
        {
            try
            {
                string fileName = conf.FileName;
                StreamReader r = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                idList = r.ReadToEnd();
                r.Close();
            }
            catch (Exception)
            {
                idList = EMPTYLIST;
            }
        }

        private void Refresh()
        {
            if (server != null)
            {
                server.Stop();
                server.Start(idList);
            }
        }

        public void ListenForRequest()
        {
            // Start server, allow presence to span 5 routers
            server = new BroadcastServer(new Guid(BroadcastServer.serviceIdString), conf.Ip, conf.Port, conf.Ttl);
            server.Start(idList);
        }

        public void Stop()
        {
            // Shutdown!
            server.Stop();
        }

        private void ConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;

            try
            {
                fsw.EnableRaisingEvents = false;
                LoadFile();
                Refresh();
            }
            finally
            {
                fsw.EnableRaisingEvents = true;
            }

        }

        static void Main(string[] args)
        {
            IdService id = new IdService();
            id.ListenForRequest();

            Console.WriteLine("Press enter to stop server");
            Console.ReadLine();

            id.Stop();
        }
    }
}


