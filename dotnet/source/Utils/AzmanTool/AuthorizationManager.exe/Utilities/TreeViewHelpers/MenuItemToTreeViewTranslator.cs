using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AuthorizationParser.Models;
using System.Runtime.InteropServices;

namespace AuthorizationManager.Utilities.TreeViewHelpers
{
    public static class MenuItemToTreeViewTranslator
    {
        public static TreeNode Translate(MenuItemModel aMenuItemModel)
        {
            TreeNode node = RecurseMenuItem(aMenuItemModel);
            return node;
        }

        private static TreeNode RecurseMenuItem(MenuItemModel aMenuItemModel)
        {
            try
            {
                TreeNode node = new TreeNode();
                if (!string.IsNullOrEmpty(aMenuItemModel.Caption))
                {
                    node.Text = aMenuItemModel.Caption;
                }
                if (aMenuItemModel.Actions != null)
                {
                    //If the node/menu has an action
                    if (aMenuItemModel.Actions.FirstOrDefault() != null)
                    {
                        List<string> operations = new List<string>();
                        string tooltip = "";
                        foreach (UXAction action in aMenuItemModel.Actions)
                        {
                            operations.Add(action.Identity);
                            if (string.IsNullOrEmpty(action.Parent))
                            {
                                tooltip += action.Identity + " : ";
                                tooltip += action.Parent + "\n";
                            }
                            else
                            {
                                tooltip += action.Identity + " : ";
                                tooltip += action.Parent + "\n";
                            }
                        }
                        node.Tag = aMenuItemModel.Actions;
                        node.ToolTipText = tooltip;
                    }
                }

                if (aMenuItemModel.Children != null)
                {
                    foreach (MenuItemModel child in aMenuItemModel.Children)
                    {
                        TreeNode childResult = RecurseMenuItem(child);
                        if (childResult != null)
                        {
                            node.Nodes.Add(childResult);
                        }
                    }
                }
                return node;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }

}
