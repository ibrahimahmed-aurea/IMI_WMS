using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Collections;
using System.IO;

namespace Imi.SupplyChain.Deployment.IISHandling
{
    public class VirtualDirectory
    {
        public string VirtualDirectoryName = "";
        public string PhysicalPath = "";
        public string FriendlyName = "";
        public string IISPath = "IIS://LOCALHOST/W3SVC/1/ROOT";
        public bool AuthAnonymous = true;
        public bool AuthBasic = false;
        public bool AuthNTLM = false;
        public string DefaultDocuments = "default.htm,default.aspx,default.asp";

        /// 
        /// Contains Virtual directory entry (as a DirectoryEntry object) after
        /// the virtual was created.
        /// 

        public DirectoryEntry VDir = null;

        /// 
        /// Creates a Virtual Directory on the Web Server and sets a few
        /// common properties based on teh property settings of this object.
        /// 
        /// 
        public bool CreateVirtual(string virtualDirName, string realPath)
        {
            this.VirtualDirectoryName = virtualDirName;
            this.PhysicalPath = realPath;

            return CreateVirtual();
        }

        public DirectoryEntry Get(string virtualDirectory)
        {
            DirectoryEntry root = new DirectoryEntry(this.IISPath);

            if (root == null)
            {
                return null;
            }

            try
            {
                return root.Children.Find(virtualDirectory, "IISWebVirtualDir");
            }
            catch
            {
                return null;
            }
        }

        public bool Exist(string virtualDirectory)
        {
            return Get(virtualDirectory) != null;
        }

        public bool CreateVirtual()
        {
            DirectoryEntry root = new DirectoryEntry(this.IISPath);

            if (root == null)
            {
                return false;
            }

            try
            {
                this.VDir = root.Children.Add(VirtualDirectoryName, "IISWebVirtualDir");
            }
            catch
            {
                try { this.VDir = new DirectoryEntry(this.IISPath + "/" + VirtualDirectoryName); }
                catch { ;}
            }

            if (this.VDir == null)
            {
                return false;
            }

            root.CommitChanges();
            VDir.CommitChanges();

            return this.SaveVirtualDirectory();
        }

        public bool DeleteVirtualDirectory()
        {
            // Delete the current virtual directory
            try
            {
                VDir.DeleteTree();
                VDir.CommitChanges();
                VDir.Dispose();
                VDir = null;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool SaveVirtualDirectory()
        {
            PropertyCollection Properties = VDir.Properties;

            try
            {
                Properties["Path"].Value = PhysicalPath;
            }

            catch (Exception)
            {
                return false;
            }

            this.VDir.Invoke("AppCreate", true);

            if (this.FriendlyName == string.Empty)
                VDir.Properties["AppFriendlyName"].Value = VirtualDirectoryName;
            else
                VDir.Properties["AppFriendlyName"].Value = this.FriendlyName;

            if (this.DefaultDocuments != string.Empty)
                VDir.Properties["DefaultDoc"].Value = this.DefaultDocuments;

            int Flags = 0;

            if (this.AuthAnonymous)
                Flags = 1;

            if (this.AuthBasic)
                Flags = Flags + 2;

            if (this.AuthNTLM)
                Flags = Flags + 4;

            Properties["AuthFlags"].Value = Flags;   // NTLM AuthBasic Anonymous

            VDir.CommitChanges();

            return true;
        }

        public bool UpdatePhysicalPath(string path)
        {
            PropertyCollection Properties = VDir.Properties;

            try
            {
                Properties["Path"].Value = path;
            }
            catch (Exception)
            {
                return false;
            }

            VDir.CommitChanges();

            return true;

        }


    }
}
