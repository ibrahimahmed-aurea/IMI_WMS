using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;
using AuthorizationParser.Readers;
using System.Xml;

namespace AuthorizationParser.Utilities
{
    public static class ActionHasOperationFixer
    {
        public static void Fix(Dictionary<string, ApplicationXMLModel> anApplications)
        {
            foreach (ApplicationXMLModel application in anApplications.Values)
            {
                ProcessMenuItem(application.MenuItems.Children, application);
            }
        }

        private static void ProcessMenuItem(List<MenuItemModel> aMenuItems, ApplicationXMLModel anApplication)
        {
            try
            {
                foreach (MenuItemModel menuItem in aMenuItems)
                {
                    if (menuItem != null)
                    {
                        ProcessActions(menuItem, anApplication);
                        ProcessMenuItem(menuItem.Children, anApplication);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ProcessActions(MenuItemModel menuitem, ApplicationXMLModel anApplication)
        {
            try
            {
                if (menuitem.Actions == null) return;
                foreach (UXAction action in menuitem.Actions)
                {
                    string newIdentity;
                    if (action == null) continue;
                    var uxActionNode = anApplication.UXActionXMLDocuments.Documents[action.Identity];
                    if (uxActionNode == null) continue;

                    var uxActionCaptionNode = uxActionNode.SelectSingleNode("/UXAction/Caption");
                    var dialogIdentityNode = uxActionNode.SelectSingleNode("/UXAction/Dialog/Dialog/Id");
                    if (dialogIdentityNode == null) continue;

                    var dialogIdentityDoc = anApplication.DialogXmlDocuments.Documents[dialogIdentityNode.InnerText];
                    if (dialogIdentityNode == null) continue;

                    newIdentity = dialogIdentityNode.InnerText;
                    var uxCaption = uxActionCaptionNode.InnerText;
                    var dialogTitleNode = dialogIdentityDoc.SelectSingleNode("/Dialog/Title");
                    if (dialogTitleNode == null) continue;

                    var dialogTitle = dialogTitleNode.InnerText;
                    if (uxCaption.Equals(dialogTitle)) continue;

                    var uxActionNameNode = uxActionNode.SelectSingleNode("/UXAction/Name");
                    var uxName = uxActionNameNode.InnerText;
                    var dialogNameNode = dialogIdentityDoc.SelectSingleNode("/Dialog/Name");
                    var dialogName = dialogNameNode.InnerText;

                    uxName = uxName.ToUpper();
                    dialogName = dialogName.ToUpper();

                    if (uxName.Equals(dialogName)) continue;

                    if (uxName.StartsWith("SHOW") && !dialogName.StartsWith("SHOW")) continue;

                    action.Identity = newIdentity;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static XmlNode ProcessViewNode(XmlNode aViewNode, string anIdentity)
        {
            XmlNode viewActionsNode = aViewNode.SelectSingleNode("ViewActions");
            if (viewActionsNode != null)
            {
                XmlNode viewActionsSearchResult = SearchViewActions(viewActionsNode, anIdentity);
                if (viewActionsSearchResult != null)
                {
                    return viewActionsSearchResult;
                }
            }

            XmlNode childrenNode = aViewNode.SelectSingleNode("Children");
            if (childrenNode != null)
            {
                foreach (XmlNode viewNodeChildNode in childrenNode)
                {
                    XmlNode processViewNodeResult = ProcessViewNode(viewNodeChildNode, anIdentity);
                    if (processViewNodeResult != null)
                    {
                        return processViewNodeResult;
                    }
                }
            }
            return null;
        }

        private static string GetTypeFromViewActionNode(XmlNode aViewActionNode)
        {
            XmlNode typeNode = aViewActionNode.SelectSingleNode("Type");
            if (typeNode == null) return null;

            return typeNode.InnerText;
        }

        private static XmlNode SearchViewActions(XmlNode aViewActionsNode, string anIdentity)
        {
            foreach (XmlNode viewActionNode in aViewActionsNode)
            {
                var identityNode = viewActionNode.SelectSingleNode("Action/UXAction/Id");
                if (identityNode == null) continue;
                if (identityNode.InnerText == anIdentity)
                    return viewActionNode;
            }
            return null;
        }
    }
}