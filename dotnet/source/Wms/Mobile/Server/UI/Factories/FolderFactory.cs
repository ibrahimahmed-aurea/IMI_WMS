using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.Server.UI.Configuration;

namespace Imi.Wms.Mobile.Server.UI
{
    class FolderFactory
    {
        private static object[] GetChildren(TreeNode parent)
        {
            List<object> l = new List<object>();

            if (parent.Nodes != null)
            {
                foreach (TreeNode n in parent.Nodes)
                {
                    if (n.Tag is ManagedConnection)
                    {
                        ManagedConnection mc = n.Tag as ManagedConnection;
                        ServerType s = mc.Config;
                        s.DisplayName = n.Text;
                        l.Add(s);
                    }
                    else
                    {
                        FolderType f = new FolderType();
                        f.Name = n.Text;
                        f.Items = GetChildren(n);
                        f.Name = n.Text;
                        l.Add(f);
                    }
                }
            }

            return (l.ToArray());
        }

        public static FolderType CreateFolder(TreeView t)
        {
            // Create root folder then get all children
            FolderType f = new FolderType();
            f.Name = "/";
            f.Items = GetChildren(t.Nodes[0]);

            return (f);
        }
    }
}
