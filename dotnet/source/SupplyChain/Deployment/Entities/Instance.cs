using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Imi.SupplyChain.Deployment.Entities
{
    [Serializable]
    public class Instance
    {
        public string Name;
        public string Caption;
        public string Description;
        public string DeployManifestFile;
        public string ProductVersion;
        public string ClickOnceVersion;
        public string VersionPath;
        public string ApplicationManifestFile;

        [OptionalField]
        public bool Visible;

        // Parameters for the product
        [OptionalField]
        public InstanceParameters Parameters;

        public Instance()
        {
            Visible = true;
            Parameters = new InstanceParameters();
        }

        public bool DeleteManifestFile(string installPath)
        {
            // Deploy Manifestfile
            string deployManifestFile = Path.Combine(installPath, DeployManifestFile);

            // Delete the file
            try
            {
                File.Delete(deployManifestFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateManifestFileSecurity(string installPath, string addAccountName)
        {
            UpdateManifestFileSecurity(installPath, addAccountName, string.Empty);
        }

        public void UpdateManifestFileSecurity(string installPath, string addAccountName, string removeAccountName)
        {
            // Remove the accountname
            if (!string.IsNullOrEmpty(removeAccountName))
                RemoveManifestFileSecurity(installPath, removeAccountName);

            // If instance is visible then add the account
            if (Visible)
            {
                AddManifestFileSecurity(installPath, addAccountName);
            }
            else
            {
                RemoveManifestFileSecurity(installPath, addAccountName);
            }
        }

        private void RemoveManifestFileSecurity(string installPath, string removeAccountName)
        {
            string filename = Path.Combine(installPath, DeployManifestFile);

            if (File.Exists(filename))
            {
                try
                {
                    FileHandler.RemoveFileSecurity(filename, removeAccountName);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Couldn't remove the account \"{0}\" from the ACL of the file {1}.", removeAccountName, DeployManifestFile), ex);
                }
            }
        }

        private void AddManifestFileSecurity(string installPath, string addAccountName)
        {
            string filename = Path.Combine(installPath, DeployManifestFile);

            if (File.Exists(filename))
            {
                try
                {
                    FileHandler.AddFileSecurity(filename,
                                                addAccountName,
                                                System.Security.AccessControl.FileSystemRights.ReadAndExecute,
                                                System.Security.AccessControl.AccessControlType.Allow);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Couldn't add the account \"{0}\" to the ACL of the file {1}.", addAccountName, DeployManifestFile), ex);
                }
            }
        }

    }
}
