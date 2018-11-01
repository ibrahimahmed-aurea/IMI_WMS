using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace AuthorizationParser.Readers
{
    public static class MetaMenuReader
    {
        public static Dictionary<string, ApplicationXMLModel> Read(string aPath)
        {
            Dictionary<string, ApplicationXMLModel> applications = new Dictionary<string, ApplicationXMLModel>();
            LoadMenus(aPath, applications);

            return applications;
        }
        /// <summary>
        /// Method used to read the metamenu file generated from this application.
        /// </summary>
        /// <param name="aPath">The path to were the file is that we want to read from</param>
        /// <param name="anApplications">A Dictionary that will be populated with XML data</param>
        private static void LoadMenus(string aPath, Dictionary<string, ApplicationXMLModel> anApplications)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(aPath);
                //Read the applications. e.g. Warehouse, Transportation
                XmlNode applicationsNode = doc.SelectSingleNode("/Applications");

                foreach (XmlNode applicationNode in applicationsNode)
                {
                    ApplicationXMLModel application = new ApplicationXMLModel();
                    //Give the RootNode a name e.g. Warehouse
                    XmlNode applicationNameNode = applicationNode.SelectSingleNode("Name");
                    ThrowIfNull(applicationNameNode,
                                "applicationNameNode was null in LoadMenus method");
                    application.Name = applicationNameNode.InnerText;

                    XmlNode topMenuItemNode = applicationNode.SelectSingleNode("Menu/MenuItem");
                    ThrowIfNull(applicationNameNode,
                                "topMenuItemNode was null in LoadMenus method");

                    MenuItemModel topMenuItemModel = new MenuItemModel();

                    XmlNode captionTopMenuItemNode = topMenuItemNode.SelectSingleNode("Caption");
                    ThrowIfNull(captionTopMenuItemNode,
                                "captionTopMenuItemNode was null in LoadMenus method");
                    topMenuItemModel.Caption = captionTopMenuItemNode.InnerText;

                    XmlNode identityTopMenuItemNode = topMenuItemNode.SelectSingleNode("Id");
                    ThrowIfNull(identityTopMenuItemNode,
                                "identityTopMenuItemNode was null in LoadMenus method");
                    topMenuItemModel.Identity = identityTopMenuItemNode.InnerText;

                    topMenuItemModel.Actions = null;

                    XmlNode topMenuChildrenNode = topMenuItemNode.SelectSingleNode("Children");
                    ThrowIfNull(topMenuChildrenNode,
                                "topMenuChildrenNode was null in LoadMenus method");

                    foreach (XmlNode menuItemChildren in topMenuChildrenNode)
                    {
                        MenuItemModel menuItemModel = LoadMenu(menuItemChildren);
                        ThrowIfNull(menuItemModel, "A child node in topMenu was null in LoadMenus method");

                        topMenuItemModel.Children.Add(menuItemModel);

                    }
                    application.MenuItems = topMenuItemModel;
                    anApplications.Add(application.Name, application);
                }

            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException) 
                {
                    MessageBox.Show("MetaMenu.XML not found!", "", MessageBoxButtons.OK);
                }
            }
        }

        private static void ThrowIfNull(object aCheckThisObject, string aMessage)
        {
            if (aCheckThisObject == null)
            {
                throw new Exception(aMessage);
            }
        }

        private static MenuItemModel LoadMenu(XmlNode aNode, int depth = 0)
        {
            try
            {
                MenuItemModel menu = new MenuItemModel();
                menu.Identity = aNode.SelectSingleNode("Id").InnerText;
                menu.Caption = aNode.SelectSingleNode("Caption").InnerText;
                if (menu.Actions == null)
                {
                    menu.Actions = new List<UXAction>();
                }
                XmlNode actionsNode = aNode.SelectSingleNode("Actions");
                if (actionsNode.ChildNodes != null)
                {
                    foreach (XmlNode actionNode in actionsNode.ChildNodes)
                    {
                        XmlNode identityNode = actionNode.SelectSingleNode("Id");
                        if (identityNode == null) continue;
                        UXAction newAction = new UXAction();
                        newAction.Identity = identityNode.InnerText;
                        menu.Actions.Add(newAction);
                    }
                }

                XmlNode children = aNode.SelectSingleNode("Children");
                if (children != null)
                {
                    foreach (XmlNode child in children)
                    {
                        //Recursive call if child has additional children
                        MenuItemModel childMenuItem = LoadMenu(child, ++depth);
                        if (childMenuItem != null && menu.Children == null)
                        {
                            menu.Children = new List<MenuItemModel>();
                        }
                        menu.Children.Add(childMenuItem);
                    }
                }
                return menu;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
