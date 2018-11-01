using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AuthorizationParser.Models;

namespace AuthorizationManager.Gui.exe.Utilities.TreeViewHelpers
{
    public static class TreeViewToOperationsListTranslator
    {
        public static List<string> Translate(TreeNode aTreeNode)
        {
            List<string> operations = new List<string>();

            ProcessNode(aTreeNode, operations, false);

            List<string> operationsList = new List<string>();
            foreach (string operation in operations)
            {
                if (operationsList.Contains(operation)) continue;
                operationsList.Add(operation);
            }
            return operationsList;
        }

        private static void ProcessNode(TreeNode aNode, List<string> anOperations, bool getAll)
        {
            List<UXAction> operations;
            if (aNode.Checked == true || getAll)
            {
                operations = (List<UXAction>)aNode.Tag;
                if (operations != null)
                {
                    foreach (UXAction action in operations)
                    {
                        if (!string.IsNullOrEmpty(action.Parent))
                        {
                            if (!anOperations.Contains(action.Parent))
                            {
                                anOperations.Add(action.Parent);
                            }
                        }
                        anOperations.Add(action.Identity);
                    }
                }
            }

            foreach (TreeNode node in aNode.Nodes)
            {
                ProcessNode(node, anOperations, getAll);
            }
        }

        public static List<string> GetAllNodeOperations(TreeNode aTreeNode)
        {
            List<string> operations = new List<string>();

            ProcessNode(aTreeNode, operations, true);

            List<string> operationsList = new List<string>();
            foreach (string operation in operations)
            {
                operationsList.Add(operation);
            }
            return operationsList;
        }
    }
}
