using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AuthorizationParser.Models;

namespace AuthorizationManager.Utilities.TreeViewHelpers
{
    public static class OperationLoader
    {
        public static void LoadIntoTreeNode(TreeNode aTreeNode, List<string> anOperationList)
        {
            ProcessNode(aTreeNode, anOperationList);
        }

        private static void ProcessNode(TreeNode aTreeNode, List<string> anOperationList)
        {
            List<UXAction> actions = (List<UXAction>)aTreeNode.Tag;
            if (actions != null)
            {
                string parent;
                if (string.IsNullOrEmpty(actions[0].Parent))
                {
                    parent = actions[0].Identity;
                }
                else
                {
                    parent = actions[0].Parent;
                }
                if (anOperationList.Contains(parent))
                {
                    aTreeNode.Checked = true;
                }
            }
            foreach (TreeNode childNode in aTreeNode.Nodes)
            {
                ProcessNode(childNode, anOperationList);
            }
        }
    }
}
