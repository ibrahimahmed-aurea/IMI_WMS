using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;
using System.Xml;

namespace AuthorizationParser.Writers
{
    public static class MetaMenuWriter
    {

        public static void Write(Dictionary<string, ApplicationXMLModel> anApplications, string anOutputDirectory)
        {
            // Create the XmlDocument.
            string finalPath = "";
            if (!string.IsNullOrEmpty(anOutputDirectory))
            {
                finalPath += anOutputDirectory;
                finalPath += "\\";
            }
            finalPath += "MetaMenu.xml";

            XmlDocument doc = new XmlDocument();

            XmlElement appsElem = doc.CreateElement("Applications");

            foreach (string key in anApplications.Keys)
            {
                XmlElement applicationElem = CreateApplicationElem(anApplications[key], appsElem);
                appsElem.AppendChild(applicationElem);
            }
            doc.AppendChild(appsElem);
            // Save the document to a file. White space is preserved
            doc.PreserveWhitespace = true;
            doc.Save(finalPath);
            using (XmlTextWriter writer = new XmlTextWriter(finalPath, null))
            {
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
            }
        }
        /// <summary>
        /// Method that builds XML.
        /// An application e.g. Warehouse has a menu called Warehouse
        /// with children called Inbound, Outbound, etc
        /// whom each have children e.g Inbound Overview
        /// </summary>
        /// <param name="anApplicationXmlModel"></param>
        /// <param name="anApplicationElement"></param>
        /// <returns></returns>
        private static XmlElement CreateApplicationElem(ApplicationXMLModel anApplicationXmlModel, XmlElement anApplicationElement)
        {
            XmlDocument doc = anApplicationElement.OwnerDocument;

            XmlElement applicationElem = doc.CreateElement("Application");

            XmlElement applicationNameElement = doc.CreateElement("Name");
            applicationNameElement.InnerText = anApplicationXmlModel.Name;
            applicationElem.AppendChild(applicationNameElement);

            XmlElement menuElement = doc.CreateElement("Menu");
            applicationElem.AppendChild(menuElement);

            XmlElement topMenuItemElement = doc.CreateElement("MenuItem");
            menuElement.AppendChild(topMenuItemElement);

            XmlElement captionElement = doc.CreateElement("Caption");
            captionElement.InnerText = anApplicationXmlModel.MenuItems.Caption;
            topMenuItemElement.AppendChild(captionElement);

            XmlElement idElement = doc.CreateElement("Id");
            idElement.InnerText = anApplicationXmlModel.MenuItems.Identity;
            topMenuItemElement.AppendChild(idElement);

            XmlElement actionsElement = doc.CreateElement("Actions");
            topMenuItemElement.AppendChild(actionsElement);

            XmlElement childrenElement = doc.CreateElement("Children");
            topMenuItemElement.AppendChild(childrenElement);

            foreach (MenuItemModel item in anApplicationXmlModel.MenuItems.Children)
            {
                XmlElement childElement = LoadChildElement(childrenElement, item);
                if (childElement != null)
                {
                    childrenElement.AppendChild(childElement);
                }
            }
            return applicationElem;
        }
        /// <summary>
        /// Method that finds and returns the children of an element.
        /// Each child will be recursively called to be checked for children.
        /// </summary>
        /// <param name="aChild">element that will be checked for child elements</param>
        /// <param name="anItem"></param>
        /// <param name="aDepth">depth 0 = children, depth 1 = grandchildren, etc</param>
        /// <returns></returns>
        private static XmlElement LoadChildElement(XmlElement aChild, MenuItemModel anItem, int aDepth = 0)
        {
            if (anItem == null) return null;

            XmlDocument doc = aChild.OwnerDocument;
            XmlElement menuItemElement = doc.CreateElement("MenuItem");

            try
            {
                XmlElement captionElement = doc.CreateElement("Caption");
                captionElement.InnerText = anItem.Caption;
                menuItemElement.AppendChild(captionElement);

                XmlElement idElement = doc.CreateElement("Id");
                idElement.InnerText = anItem.Identity;
                menuItemElement.AppendChild(idElement);

                XmlElement actionsElement = doc.CreateElement("Actions");
                menuItemElement.AppendChild(actionsElement);

                if (anItem.Actions != null)
                {
                    string parent = null;
                    foreach (UXAction action in anItem.Actions)
                    {
                        if (!string.IsNullOrEmpty(action.Parent) && parent == null)
                        {
                            parent = action.Parent;
                            XmlElement actionParentElement = doc.CreateElement("Action");
                            XmlElement actionParentIdentityElement = doc.CreateElement("Id");
                            actionParentIdentityElement.InnerText = parent;

                            actionParentElement.AppendChild(actionParentIdentityElement);

                            actionsElement.AppendChild(actionParentElement);
                        }

                        XmlElement actionElement = doc.CreateElement("Action");
                        XmlElement actionIdentityElement = doc.CreateElement("Id");
                        actionIdentityElement.InnerText = action.Identity;

                        actionElement.AppendChild(actionIdentityElement);

                        actionsElement.AppendChild(actionElement);
                    }
                }
                XmlElement childrenElement = doc.CreateElement("Children");
                menuItemElement.AppendChild(childrenElement);

                if (anItem.Children != null)
                {
                    foreach (MenuItemModel itemChild in anItem.Children)
                    {
                        XmlElement child = LoadChildElement(childrenElement, itemChild, ++aDepth);
                        if (child != null)
                        {
                            childrenElement.AppendChild(child);
                        }
                    }
                }

                return menuItemElement;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("LoadChildElement method failed at depth {0}\n{1}", aDepth, ex.Message));
            }
        }
    }
}
