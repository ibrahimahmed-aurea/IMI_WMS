using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser.Models;
using System.Xml;

namespace AuthorizationParser.Utilities
{
    public static class ActionParser
    {


        public static void Parse(Dictionary<string, ApplicationXMLModel> anApplications)
        {

            foreach (ApplicationXMLModel applicationModel in anApplications.Values)
            {
                ParseApplicationXMLModel(applicationModel);
            }
        }

        private static void ParseApplicationXMLModel(ApplicationXMLModel anApplicationModel)
        {
            foreach (MenuItemModel menuItem in anApplicationModel.MenuItems.Children)
            {
                MenuItemIterator(menuItem, anApplicationModel);
            }
        }

        private static void MenuItemIterator(MenuItemModel aMenuItem, ApplicationXMLModel anApplicationModel)
        {
            if (MenuItemHasChildren(aMenuItem))
            {
                foreach (MenuItemModel menuItemChild in aMenuItem.Children)
                {
                    MenuItemIterator(menuItemChild, anApplicationModel);
                }
            }
            else
            {
                ParseUXAction(aMenuItem, anApplicationModel);
            }

        }

        private static void ParseUXAction(MenuItemModel aMenuItem, ApplicationXMLModel anApplicationModel)
        {
            string actionIdentity = aMenuItem.Actions.FirstOrDefault().Identity;
            XmlDocument uXActionDoc = anApplicationModel.UXActionXMLDocuments.Documents[actionIdentity];
            string dialogId;
            XmlNode dialogIdNode = uXActionDoc.SelectSingleNode("/UXAction/Dialog/Dialog/Id");
            if (dialogIdNode != null)
            {
                dialogId = dialogIdNode.InnerText;
                XmlDocument dialogDoc = anApplicationModel.DialogXmlDocuments.Documents[dialogId];
                ParseDialog(aMenuItem, anApplicationModel, dialogDoc);
            }
        }

        private static void ParseDialog(MenuItemModel aMenuItem, ApplicationXMLModel anApplicationModel, XmlDocument dialogDoc)
        {
            string errorMessageString = "{0} was null in function ActionParser.ParseDialog";
            //läs in alla actions från dialogDoc och lägg dem i aMenuItem
            try
            {
                XmlNode viewActionsNode = dialogDoc.SelectSingleNode("/Dialog/RootViewNode/ViewNode/ViewActions");
                ThrowIfNull(viewActionsNode, string.Format(errorMessageString, "viewActionsNode"));
                List<MenuItemModel> mainActions = new List<MenuItemModel>();

                foreach (XmlNode viewActionNode in viewActionsNode.ChildNodes)
                {
                    MenuItemModel menuItem = GetUXActionsFromNode(viewActionNode, anApplicationModel);
                    if (menuItem != null)
                    {

                        XmlNode sequenceNode = viewActionNode.SelectSingleNode("Sequence");
                        menuItem.Sequence = int.Parse(sequenceNode.InnerText);
                        mainActions.Add(menuItem);
                    }
                }

                mainActions.Sort();
                aMenuItem.Children.AddRange(mainActions);

                XmlNode viewNodeChildrenNode = dialogDoc.SelectSingleNode("/Dialog/RootViewNode/ViewNode/Children");
                ThrowIfNull(viewNodeChildrenNode, string.Format("viewNodeChildrenNode"));

                foreach (XmlNode childViewNode in viewNodeChildrenNode)
                {
                    AddActionsToMenuItem(aMenuItem, childViewNode, anApplicationModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddActionsToMenuItem(MenuItemModel aMenuItem, XmlNode aViewNode, ApplicationXMLModel anApplicationModel)
        {
            //Add current viewNode actions to aMenuItem.Children
            XmlNode viewActionsNode = aViewNode.SelectSingleNode("ViewActions");
            List<MenuItemModel> childList = new List<MenuItemModel>();

            if (viewActionsNode != null)
            {
                foreach (XmlNode viewActionNode in viewActionsNode.ChildNodes)
                {
                    MenuItemModel menuItem = GetUXActionsFromNode(viewActionNode, anApplicationModel);

                    XmlNode sequenceNode = viewActionNode.SelectSingleNode("Sequence");
                    if (sequenceNode != null && menuItem != null)
                    {
                        menuItem.Sequence = int.Parse(sequenceNode.InnerText);
                        childList.Add(menuItem);
                    }
                }
                childList.Sort();
                aMenuItem.Children.AddRange(childList);

            }
            //check if aViewNode has any children, call self recursively
            XmlNode children = aViewNode.SelectSingleNode("Children");
            if (children != null)
            {
                foreach (XmlNode childViewNode in children)
                {
                    AddActionsToMenuItem(aMenuItem, childViewNode, anApplicationModel);
                }
            }
        }

        private static MenuItemModel GetUXActionsFromNode(XmlNode aViewActionNode, ApplicationXMLModel anApplicationModel)
        {
            string errorMessageString = "{0} was null in function ActionParser.GetUXActionsFromNode";
            MenuItemModel menuItem = new MenuItemModel();

            UXAction uXAction = new UXAction();

            XmlNode actionIdentityNode = aViewActionNode.SelectSingleNode("Action/UXAction/Id");
            ThrowIfNull(actionIdentityNode, string.Format(errorMessageString, "actionIdentityNode"));
            uXAction.Identity = actionIdentityNode.InnerText;

            XmlNode uXActionNode = anApplicationModel.UXActionXMLDocuments.Documents[uXAction.Identity];
            ThrowIfNull(uXActionNode, string.Format(errorMessageString, "uXActionNode"));

            XmlNode uXActionCaptionNode = uXActionNode.SelectSingleNode("/UXAction/Caption");
            ThrowIfNull(uXActionCaptionNode, string.Format(errorMessageString, "uXActionCaptionNode"));

            menuItem.Caption = uXActionCaptionNode.InnerText;

            XmlNode viewActionTypeNode = aViewActionNode.SelectSingleNode("Type");
            if (viewActionTypeNode != null)
            {
                if (viewActionTypeNode.InnerText == "JumpTo")
                {
                    AddShowActionToCorrectStartAction(aViewActionNode, anApplicationModel);
                    return null;
                }
                else if (viewActionTypeNode.InnerText == "Drilldown")
                {
                    menuItem.Caption += " (Drilldown)";
                    AddDrilldownsActionChildren(uXActionNode, menuItem, anApplicationModel);
                }
            }

            XmlNode uxActionDialogNode = aViewActionNode.SelectSingleNode("/UXAction/Dialog/Dialog/Id");

            if (uxActionDialogNode != null)
            {
                menuItem.Identity = uxActionDialogNode.InnerText;
            }

            menuItem.Actions = new List<UXAction>();
            menuItem.Actions.Add(uXAction);

            return menuItem;
        }

        private static void AddDrilldownsActionChildren(XmlNode uXActionNode, MenuItemModel aMenuItem, ApplicationXMLModel anApplicationModel)
        {
            XmlNode dialogIdentityNode = uXActionNode.SelectSingleNode("/UXAction/Dialog/Dialog/Id");
            if (dialogIdentityNode == null) return;
            ParseDialog(aMenuItem, anApplicationModel, anApplicationModel.DialogXmlDocuments.Documents[dialogIdentityNode.InnerText]);
        }

        private static void AddShowActionToCorrectStartAction(XmlNode aViewNode, ApplicationXMLModel anApplicationModel)
        {
            try
            {
                //find dialog in applicatinXMLModel using viewnodes UXAction/ID
                string startActionIdentity = GetStartActionFromUXAction(aViewNode, anApplicationModel);
                //dialog id is also start action id, iterate applicationmodels menuItems and find
                //menuitem with the Start action id, add show action id to menuitems actions
                MoveShowActionToStartAction(startActionIdentity, aViewNode, anApplicationModel);









            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void MoveShowActionToStartAction(string aStartActionIdentity, XmlNode aViewNode, ApplicationXMLModel anApplicationModel)
        {
            XmlNode uXActionIdentityNode = aViewNode.SelectSingleNode("Action/UXAction/Id");
            if (uXActionIdentityNode == null) return;

            string uXActionIdentity = uXActionIdentityNode.InnerText;

            MenuItemModel menuItemResult = FindStartItem(aStartActionIdentity, anApplicationModel.MenuItems);

            if (menuItemResult == null) return;

            if (ActionsContainsIdentity(menuItemResult.Actions, uXActionIdentity)) return;

            menuItemResult.Actions.Add(new UXAction { Identity = uXActionIdentity, Parent = aStartActionIdentity });
        }

        private static bool ActionsContainsIdentity(List<UXAction> anActions, string anIdentity)
        {
            foreach (UXAction action in anActions)
            {
                if (action.Identity == anIdentity)
                {
                    return true;
                }
            }
            return false;
        }

        private static MenuItemModel FindStartItem(string aStartActionIdentity, MenuItemModel aMenuItemModel)
        {
            try
            {
                if (aMenuItemModel == null) return null;
                if (aMenuItemModel.Actions != null)
                {
                    foreach (UXAction action in aMenuItemModel.Actions)
                    {
                        if (action.Parent == aStartActionIdentity) return aMenuItemModel;
                        if (action.Identity == aStartActionIdentity) return aMenuItemModel;
                    }
                }
                foreach (MenuItemModel child in aMenuItemModel.Children)
                {
                    MenuItemModel result = FindStartItem(aStartActionIdentity, child);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        private static string GetStartActionFromUXAction(XmlNode aViewNode, ApplicationXMLModel anApplicationModel)
        {
            XmlNode uXActionIdentityNode = aViewNode.SelectSingleNode("Action/UXAction/Id");
            if (uXActionIdentityNode == null)
            {
                return null;
            }
            string uXActionIdentity = uXActionIdentityNode.InnerText;

            XmlDocument uXActionDoc = anApplicationModel.UXActionXMLDocuments.Documents[uXActionIdentity];

            XmlNode dialogIdentityNode = uXActionDoc.SelectSingleNode("/UXAction/Dialog/Dialog/Id");
            if (dialogIdentityNode == null)
            {
                return null;
            }
            return dialogIdentityNode.InnerText;
        }

        public static bool MenuItemHasChildren(MenuItemModel aMenuItem)
        {
            if (aMenuItem.Children == null)
            {
                return false;
            }
            else if (aMenuItem.Children.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ThrowIfNull(object aCheckThisObject, string aMessage)
        {
            if (aCheckThisObject == null)
            {
                throw new Exception(aMessage);
            }
        }
    }
}
