using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.Server.UI.Configuration;

namespace Imi.Wms.Mobile.Server.UI
{
    class TreeNodeFactory
    {
        private const int DirectoryImageIndex = 2;
        private const int ServerImageIndex = 1;

        private static List<TreeNode> GetChildren(FolderType parent)
        {
            List<TreeNode> tc = new List<TreeNode>();

            if (parent.Items != null)
            {
                foreach (object o in parent.Items)
                {
                    if (o is ServerType)
                    {
                        ServerType s = o as ServerType;
                        tc.Add(CreateServerNode(s));
                    }
                    else if (o is FolderType)
                    {
                        FolderType f = o as FolderType;
                        tc.Add(CreateFolderNode(f));
                    }
                }
            }

            return (tc);
        }

        public static TreeNode CreateFolderNode(string folderName)
        {
            TreeNode n = new TreeNode(folderName, DirectoryImageIndex, DirectoryImageIndex);
            return (n);
        }

        //public static TreeNode CreateServerNode(string serverName)
        //{
        //    ManagedConnection connection = ManagedConnection.CreateDefaultConnection();
        //    connection.Config.DisplayName = serverName;
        //    return (CreateServerNode(connection));
        //}

        public static TreeNode CreateServerNode(ServerType server)
        {
            ManagedConnection connection = new ManagedConnection(server);
            return (CreateServerNode(connection));
        }

        public static TreeNode CreateServerNode(ManagedConnection connection)
        {
            TreeNode n = new TreeNode(connection.Config.DisplayName, ServerImageIndex, ServerImageIndex);
            n.Tag = connection;
            return (n);
        }

        private static TreeNode CreateFolderNode(FolderType f)
        {
            TreeNode n = CreateFolderNode(f.Name);
            n.Nodes.AddRange(GetChildren(f).ToArray());
            return (n);
        }

        public static List <TreeNode> CreateTreeNodeChildList(FolderType rootFolder)
        {
            return(GetChildren(rootFolder));
        }

    }
}
