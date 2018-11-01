using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;
using System.Xml;
using System.IO;
using AuthorizationParser.Utilities;
using AuthorizationParser.Readers;
using AuthorizationParser.Writers;

namespace AuthorizationParser
{
    public static class MetaDataParser
    {
        private static Dictionary<string, ApplicationXMLModel> _applications = new Dictionary<string, ApplicationXMLModel>();
        private static List<string> allOperations = new List<string>();
        private static List<string> duplicatesList = new List<string>();
        /// <summary>
        /// This method returns the Smart Client Menu Replica in a structured way.
        /// Right now it first looks for the metadata-catalogue and builds a new MetaMenu.xml-file,
        /// and if the metadata-catalogue is missing it tries to open the MetaMenu.xml-file.
        /// The idea is to later build a console-application that lets the SmartClientServer 
        /// build the MetaMenu.xml.
        /// 
        /// This method, and class, needs to be cleaned up and reorganized before release.
        /// </summary>
        /// <returns></returns>

        public static bool CreateMetaMenu(string anOutputDirectory, string aMetadataDirectoryPath)
        {
            try
            {
                if (Directory.Exists(aMetadataDirectoryPath))
                {
                    LoadMetaData(aMetadataDirectoryPath);
                    MenuSequenceSorter.Sort(_applications);
                    ActionParser.Parse(_applications);
                    ActionHasOperationFixer.Fix(_applications);

                    CreateMenuXML(anOutputDirectory);
                }
                else
                {
                    throw new Exception("no metadata folder or menu.xml file found");
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static Dictionary<string, ApplicationXMLModel> LoadMetaMenu(string aPath)
        {
            Dictionary<string,ApplicationXMLModel> applications = MetaMenuReader.Read(aPath);

            _applications = applications;

            return applications;
        }

        /// <summary>
        /// Dictionary containing all applications and their properties
        /// </summary>
        public static Dictionary<string, ApplicationXMLModel> Applications
        {
            get { return _applications; }
            set { _applications = value; }
        }

        private static void CreateMenuXML(string aPath)
        {
            MetaMenuWriter.Write(_applications, aPath);
        }

        private static MenuItemModel LoadMetaData(string aPath)
        {
            //LoadDialogAndUXActionMetaData returns null?
            ApplicationXMLModel application = LoadDialogAndUXActionMetaData(aPath);
            LoadMenus(aPath, _applications);

            return null;
        }

        /// <summary>
        /// Method that loads the main MenuItem then all child MenuItems
        /// </summary>
        /// <param name="aPath"></param>
        /// <param name="anApplications"></param>
        private static void LoadMenus(string aPath, Dictionary<string, ApplicationXMLModel> anApplications)
        {
            try
            {
                string[] moduleDirectoryEntries = Directory.GetDirectories(aPath);

                foreach (string subDirectoryName in moduleDirectoryEntries)
                {
                    ApplicationXMLModel app;
                    //Backend folders are not used
                    string appName;
                    bool isFrontend = subDirectoryName.Contains("_Frontend");

                    if (isFrontend)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(subDirectoryName);

                        appName = dirInfo.Name.Split('_').First();

                        if (anApplications.ContainsKey(appName))
                        {
                            app = anApplications[appName];
                        }
                        else
                        {
                            app = new ApplicationXMLModel();
                            app.Name = appName;
                            anApplications.Add(appName, app);
                        }

                        string[] menuFileNames = Directory.GetFiles(subDirectoryName + "\\Menu");
                        foreach (string menuPath in menuFileNames)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(menuPath);
                            XmlNode node = doc.SelectSingleNode("/Menu/TopMenuItem/MenuItem");
                            //For each child MenuItem add a MenuItem under the tag Children for this Menuitem
                            MenuItemModel menuItemModel = LoadMenu(node, _applications[appName]);
                            if (menuItemModel != null)
                            {
                                app.MenuItems = menuItemModel;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method that searches for children of a MenuItem
        /// and connects that child to the parent MenuItem
        /// </summary>
        /// <param name="aNode">A menuItem</param>
        /// <param name="depth">0 = children, 1 = grandchildren, etc</param>
        /// <returns>A child MenuItem</returns>
        private static MenuItemModel LoadMenu(XmlNode aNode, ApplicationXMLModel anApplication, int depth = 0)
        {
            try
            {
                MenuItemModel menu = new MenuItemModel();
                menu.Identity = aNode.SelectSingleNode("Id").InnerText;
                menu.Caption = aNode.SelectSingleNode("Caption").InnerText;
                if (IdentityExceptionHelper.Contains(menu.Caption)) return null;

                XmlNode sequenceNode = aNode.SelectSingleNode("Sequence");
                if (sequenceNode != null) menu.Sequence = int.Parse(sequenceNode.InnerText);

                UXAction action = new UXAction();
                XmlNode uXActionIdentityNode = aNode.SelectSingleNode("Action/UXAction/Id");
                if (uXActionIdentityNode != null)
                {
                    action.Identity = uXActionIdentityNode.InnerText;
                    //load ux-file
                    XmlDocument uXDoc = anApplication.UXActionXMLDocuments.Documents[action.Identity];
                    XmlNode uXDialogNode = uXDoc.SelectSingleNode("/UXAction/Dialog/Dialog/Id");
                    //insert check if has dialog and if dialog is a Start-operation
                    if (uXDialogNode != null)
                    {
                        XmlDocument dialogDoc = anApplication.DialogXmlDocuments.Documents[uXDialogNode.InnerText];
                        XmlNode dialogIdNode = dialogDoc.SelectSingleNode("/Dialog/Id");
                        if (dialogIdNode != null)
                        {
                                //if it is a start-action, swap action.Identity for the dialog identity
                                action.Parent = dialogIdNode.InnerText;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(action.Identity))
                {
                    if (menu.Actions == null)
                    {
                        menu.Actions = new List<UXAction>();
                    }
                    menu.Actions.Add(action);
                }

                XmlNode children = aNode.SelectSingleNode("Children");
                if (children != null)
                {
                    foreach (XmlNode child in children)
                    {
                        //Recursive call to this method in case there are grandchildren and beyond.
                        MenuItemModel childMenuItem = LoadMenu(child, anApplication, ++depth);
                        if (childMenuItem != null && menu.Children == null)
                        {
                            menu.Children = new List<MenuItemModel>();
                        }
                        if (childMenuItem != null)
                        {
                            menu.Children.Add(childMenuItem);
                        }
                    }
                }
                return menu;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Init the variables and call the Load methods for Dialogs and UXActions
        /// </summary>
        /// <param name="aPath"></param>
        /// <returns></returns>
        private static ApplicationXMLModel LoadDialogAndUXActionMetaData(string aPath)
        {
            ApplicationXMLModel metaDataModel = new ApplicationXMLModel();
            metaDataModel.DialogXmlDocuments = new DialogXMLModel();
            metaDataModel.UXActionXMLDocuments = new UXActionXMLModel();

            LoadDialogs(aPath, _applications);
            LoadUXActions(aPath, _applications);

            return null;
        }
        /// <summary>
        /// Load all UXActions from the UXAction folder
        /// </summary>
        /// <param name="aPath"></param>
        /// <param name="anApplications"></param>
        private static void LoadUXActions(string aPath, Dictionary<string, ApplicationXMLModel> anApplications)
        {
            try
            {
                string[] moduleDirectoryEntries = Directory.GetDirectories(aPath);

                foreach (string subDirectoryName in moduleDirectoryEntries)
                {
                    ApplicationXMLModel app;
                    string appName;
                    bool isFrontend = subDirectoryName.Contains("_Frontend");

                    if (isFrontend)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(subDirectoryName);

                        appName = dirInfo.Name.Split('_').First();

                        if (anApplications.ContainsKey(appName))
                        {
                            app = anApplications[appName];
                        }
                        else
                        {
                            app = new ApplicationXMLModel();
                            app.Name = appName;
                            anApplications.Add(appName, app);
                        }

                        string[] uXActionFileNames = Directory.GetFiles(subDirectoryName + "\\UXAction");
                        foreach (string uXActionPath in uXActionFileNames)
                        {

                            try
                            {
                                XmlDocument document = LoadXMLDocument(uXActionPath);
                                if (document != null)
                                {
                                    string key = document.SelectSingleNode("/UXAction/Id").InnerText;
                                    app.UXActionXMLDocuments.Documents.Add(key, document);
                                }
                            }
                            catch (Exception ex)
                            {
                                //If file size is 0 then ignore the error and continue
                                if (new FileInfo(uXActionPath).Length > 0)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Load all dialogs from the Dialog folder
        /// </summary>
        /// <param name="aPath"></param>
        /// <param name="anApplications"></param>
        static void LoadDialogs(string aPath, Dictionary<string, ApplicationXMLModel> anApplications)
        {
            try
            {
                string[] moduleDirectoryEntries = Directory.GetDirectories(aPath);

                foreach (string subDirectoryName in moduleDirectoryEntries)
                {
                    ApplicationXMLModel app;
                    string appName;

                    bool isFrontend = subDirectoryName.Contains("_Frontend");
                    if (isFrontend)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(subDirectoryName);

                        appName = dirInfo.Name.Split('_').First();

                        if (anApplications.ContainsKey(appName))
                        {
                            app = anApplications[appName];
                        }
                        else
                        {
                            app = new ApplicationXMLModel();
                            app.Name = appName;
                            anApplications.Add(appName, app);
                        }

                        string[] dialogFileNames = Directory.GetFiles(subDirectoryName + "\\Dialog");
                        foreach (string dialogPath in dialogFileNames)
                        {
                            try
                            {
                                XmlDocument document = LoadXMLDocument(dialogPath);
                                if (document != null)
                                {
                                    string key = document.SelectSingleNode("/Dialog/Id").InnerText;
                                    app.DialogXmlDocuments.Documents.Add(key, document);
                                }
                            }
                            catch (Exception ex)
                            {
                                //If file size is 0 then ignore the error and continue
                                if (new FileInfo(dialogPath).Length > 0)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load XMLDocument from specified path
        /// </summary>
        /// <param name="aPath">Location of the document to load</param>
        /// <returns>The document</returns>
        private static XmlDocument LoadXMLDocument(string aPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(aPath);
            return doc;
        }
    }
}
