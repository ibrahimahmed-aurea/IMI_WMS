using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections.ObjectModel;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Shell
{
    public static class XmlToShellDrillDownMenuItemTransformer
    {
        public static ShellDrillDownMenuItem Transform(XmlDocument document)
        {
            XmlNode xmlDocumentRoot = document.SelectSingleNode("/folder");
            ShellDrillDownMenuItem entry = AddMenuItem(xmlDocumentRoot, null);
            return entry;
        }

        public static XmlDocument Transform(ShellDrillDownMenuItem topMenuItem)
        {
            XmlDocument document = new XmlDocument();
            document.AppendChild(AddChild(document, topMenuItem));
            return document;
        }

        private static XmlElement AddChild(XmlDocument document, DrillDownMenuItem menuItem)
        {
            string name = menuItem.IsFolder ? "folder" : "item";

            XmlElement node = document.CreateElement(name);

            XmlAttribute a = document.CreateAttribute("caption");
            a.Value = menuItem.Caption;
            node.Attributes.Append(a);

            if (!string.IsNullOrEmpty(menuItem.Id))
            {
                a = document.CreateAttribute("id");
                a.Value = menuItem.Id;
                node.Attributes.Append(a);
            }
            
            if ((!menuItem.IsFolder) && (menuItem is ShellDrillDownMenuItem))
            {
                ShellDrillDownMenuItem shellMenuItem = (menuItem as ShellDrillDownMenuItem);
                
                a = document.CreateAttribute("operation");
                a.Value = shellMenuItem.Operation;
                node.Attributes.Append(a);

                a = document.CreateAttribute("assemblyFile");
                a.Value = shellMenuItem.AssemblyFile;
                node.Attributes.Append(a);

                a = document.CreateAttribute("topicIdentity");
                a.Value = shellMenuItem.EventTopic;
                node.Attributes.Append(a);

                a = document.CreateAttribute("parameters");
                a.Value = shellMenuItem.Parameters;
                node.Attributes.Append(a);
            }

            if (menuItem.IsFolder)
            {
                foreach (DrillDownMenuItem child in menuItem.Children)
                {
                    if (!child.IsBackItem)
                    {
                        node.AppendChild(AddChild(document, child));
                    }
                }
            }

            return node;

        }

        private static ShellDrillDownMenuItem AddMenuItem(XmlNode currentXmlNode, ShellDrillDownMenuItem parentMenuItem)
        {
            ShellDrillDownMenuItem menuItem = new ShellDrillDownMenuItem();
            
            menuItem.Parent = parentMenuItem;
            
            menuItem.IsEnabled = true;
            menuItem.IsAuthorized = true;
            menuItem.IsFolder = (currentXmlNode.Name == "folder");

            foreach (XmlAttribute attribute in currentXmlNode.Attributes)
            {
                if (attribute.Name == "caption")
                    menuItem.Caption = attribute.Value;

                if (attribute.Name == "assemblyFile")
                    menuItem.AssemblyFile = attribute.Value;

                if (attribute.Name == "id")
                {
                    string id = attribute.Value;

                    if (!string.IsNullOrEmpty(id))
                    {
                        menuItem.Id = id;
                    }
                }

                if (attribute.Name == "operation")
                {
                    menuItem.Operation = attribute.Value;
                }

                if (attribute.Name == "topicIdentity")
                    menuItem.EventTopic = attribute.Value;

                if (attribute.Name == "parameters")
                    menuItem.Parameters = attribute.Value;
            }
                                                
            if (parentMenuItem != null)
            {
                parentMenuItem.Children.Add(menuItem);

                if (menuItem.IsFolder)
                {
                    AddBackMenuItem(menuItem);
                }
            }

            if (menuItem.IsFolder)
            {
                foreach (XmlNode childXmlNode in currentXmlNode.ChildNodes)
                {
                    AddMenuItem(childXmlNode, menuItem);
                }
            }

            return menuItem;
        }

        private static void AddBackMenuItem(ShellDrillDownMenuItem parentMenuItem)
        {
            ShellDrillDownMenuItem menuItem = new ShellDrillDownMenuItem();

            menuItem.Parent = parentMenuItem;

            menuItem.IsEnabled = true;
            menuItem.IsAuthorized = true;
            menuItem.IsFolder = false;
            menuItem.IsBackItem = true;

            menuItem.Caption = parentMenuItem.Parent.Caption;

            parentMenuItem.Children.Add(menuItem);
        }

    }
}
