using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace Imi.Wms.Mobile.UI.Configuration
{
    public class ConfigurationManager
    {
        private static UISection _config;

        private ConfigurationManager()
        {
        }

        public static UISection LoadConfiguration()
        {
            if (_config != null)
            {
                return _config;
            }

            string configFile = string.Format("{0}\\client.config", new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath.Replace("%20", " "));

            if (File.Exists(configFile))
            {

                using (StreamReader streamReader = new StreamReader(configFile))
                {
                    using (XmlReader xmlReader = XmlReader.Create(streamReader))
                    {
                        if (!xmlReader.ReadToFollowing(UISection.SectionKey))
                        {
                            throw new ArgumentException("Missing configuration section.", UISection.SectionKey);
                        }

                        _config = new UISection();

                        if (!xmlReader.MoveToAttribute("connectTimeout"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "connectTimeout");
                        }

                        _config.ConnectTimeout = xmlReader.ReadContentAsInt();

                        if (!xmlReader.MoveToAttribute("receiveTimeout"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "receiveTimeout");
                        }

                        _config.ReceiveTimeout = xmlReader.ReadContentAsInt();

                        if (!xmlReader.MoveToAttribute("sendTimeout"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "sendTimeout");
                        }

                        _config.SendTimeout = xmlReader.ReadContentAsInt();

                        if (!xmlReader.MoveToAttribute("retryCount"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "retryCount");
                        }

                        _config.RetryCount = xmlReader.ReadContentAsInt();

                        if (!xmlReader.MoveToAttribute("logEnabled"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "logEnabled");
                        }

                        _config.LogEnabled = bool.Parse(xmlReader.ReadContentAsString());

                        if (!xmlReader.MoveToAttribute("terminalId"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "terminalId");
                        }

                        _config.TerminalId = xmlReader.ReadContentAsString();

                        if (!xmlReader.MoveToAttribute("nativeDriver"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "nativeDriver");
                        }

                        _config.NativeDriver = xmlReader.ReadContentAsString();

                        if (xmlReader.MoveToAttribute("lastSessionId"))
                        {
                            _config.LastSessionId = xmlReader.ReadContentAsString();
                        }

                        ReadFonts(xmlReader, _config);

                        ReadApplications(xmlReader, _config);

                        ReadWindowsDesktopSettings(xmlReader, _config);
                    }
                }
            }
            else
            {
                _config = new UISection();
            }

            return _config;
        }

        private static void ReadFonts(XmlReader xmlReader, UISection settings)
        {
            if (xmlReader.ReadToFollowing("fontConversions"))
            {
                if (xmlReader.ReadToDescendant("font"))
                {
                    do
                    {
                        FontElement element = new FontElement();

                        if (!xmlReader.MoveToAttribute("oldName"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "oldName");
                        }

                        element.OldName = xmlReader.ReadContentAsString();

                        if (!xmlReader.MoveToAttribute("newName"))
                        {
                            throw new ArgumentException("Missing configuration attribute.", "newName");
                        }

                        element.NewName = xmlReader.ReadContentAsString();

                        if (xmlReader.MoveToAttribute("sizeAdjust"))
                        {
                            element.SizeAdjust = xmlReader.ReadContentAsFloat();
                        }

                        settings.FontCollection.Add(element);

                    } while (xmlReader.ReadToNextSibling("font"));
                }
            }
        }

        private static void ReadApplications(XmlReader xmlReader, UISection settings)
        {
            if (!xmlReader.ReadToFollowing("servers"))
            {
                throw new ArgumentException("Missing configuration section.", "servers");
            }

            if (xmlReader.ReadToDescendant("server"))
            {

                do
                {
                    ServerElement element = new ServerElement();

                    if (!xmlReader.MoveToAttribute("name"))
                    {
                        throw new ArgumentException("Missing configuration attribute.", "name");
                    }

                    element.Name = xmlReader.ReadContentAsString();

                    if (!xmlReader.MoveToAttribute("hostName"))
                    {
                        throw new ArgumentException("Missing configuration attribute.", "hostName");
                    }

                    element.HostName = xmlReader.ReadContentAsString();

                    if (!xmlReader.MoveToAttribute("port"))
                    {
                        throw new ArgumentException("Missing configuration attribute.", "port");
                    }

                    element.Port = xmlReader.ReadContentAsInt();

                    if (xmlReader.MoveToAttribute("default"))
                    {
                        element.Default = bool.Parse(xmlReader.ReadContentAsString());
                    }

                    if (xmlReader.MoveToAttribute("defaultApplication"))
                    {
                        element.DefaultApplication = xmlReader.ReadContentAsString();
                    }

                    settings.ServerCollection.Add(element);
                } while (xmlReader.ReadToNextSibling("server"));
            }
        }

        private static void ReadWindowsDesktopSettings(XmlReader xmlReader, UISection settings)
        {
            if (xmlReader.ReadToFollowing("windowsDesktopSettings"))
            {
                if (xmlReader.ReadToDescendant("setting"))
                {
                    do
                    {
                        Setting element = new Setting();

                        if (xmlReader.MoveToAttribute("key"))
                        {
                            element.Key = xmlReader.ReadContentAsString();

                            if (xmlReader.MoveToAttribute("value"))
                            {
                                element.Value = xmlReader.ReadContentAsString();
                            }
                            else
                            {
                                element.Value = string.Empty;
                            }

                            settings.WindowsDesktopSettingsCollection.Add(element.Key, element);
                        }
                    } while (xmlReader.ReadToDescendant("setting"));
                }
            }
        }

        public static void SaveConfiguration(UISection configuration)
        {
            string configFile = string.Format("{0}\\client.config", new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath.Replace("%20", " "));

            if (File.Exists(configFile))
            {
                FileInfo fi = new FileInfo(configFile);
                fi.Attributes = FileAttributes.Archive;
            }

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(configFile, xmlSettings))
            {
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("configuration");

                xmlWriter.WriteStartElement(UISection.SectionKey);

                xmlWriter.WriteAttributeString("connectTimeout", configuration.ConnectTimeout.ToString());
                xmlWriter.WriteAttributeString("receiveTimeout", configuration.ReceiveTimeout.ToString());
                xmlWriter.WriteAttributeString("sendTimeout", configuration.SendTimeout.ToString());
                xmlWriter.WriteAttributeString("retryCount", configuration.RetryCount.ToString());
                xmlWriter.WriteAttributeString("logEnabled", configuration.LogEnabled.ToString());
                xmlWriter.WriteAttributeString("terminalId", configuration.TerminalId);
                xmlWriter.WriteAttributeString("nativeDriver", configuration.NativeDriver);
                xmlWriter.WriteAttributeString("lastSessionId", configuration.LastSessionId);

                xmlWriter.WriteStartElement("fontConversions");

                foreach (FontElement fe in configuration.FontCollection)
                {
                    xmlWriter.WriteStartElement("font");
                    xmlWriter.WriteAttributeString("oldName", fe.OldName);
                    xmlWriter.WriteAttributeString("newName", fe.NewName);
                    xmlWriter.WriteAttributeString("sizeAdjust", fe.SizeAdjust.ToString());
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("servers");

                foreach (ServerElement ae in configuration.ServerCollection)
                {
                    xmlWriter.WriteStartElement("server");
                    xmlWriter.WriteAttributeString("name", ae.Name);
                    xmlWriter.WriteAttributeString("hostName", ae.HostName);
                    xmlWriter.WriteAttributeString("port", ae.Port.ToString());
                    xmlWriter.WriteAttributeString("default", ae.Default.ToString());
                    xmlWriter.WriteAttributeString("defaultApplication", ae.DefaultApplication);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

                if (configuration.WindowsDesktopSettingsCollection.Count > 0)
                {
                    xmlWriter.WriteStartElement("windowsDesktopSettings");

                    foreach (KeyValuePair<string, Setting> Wsetting in configuration.WindowsDesktopSettingsCollection)
                    {
                        xmlWriter.WriteStartElement("setting");
                        xmlWriter.WriteAttributeString("key", Wsetting.Value.Key);
                        xmlWriter.WriteAttributeString("value", Wsetting.Value.Value);
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();

                xmlWriter.Close();
            }

        }
    }
}
