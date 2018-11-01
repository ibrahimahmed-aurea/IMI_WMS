using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Imi.SupplyChain.Deployment.Entities
{
    [Serializable]
    public class Instances : List<Instance> 
    {
        public Instances() { }

        public void UpdateRightsOnManifestFiles(string installPath, string addAccountName)
        {
            UpdateRightsOnManifestFiles(installPath, addAccountName, string.Empty);
        }

        public void UpdateRightsOnManifestFiles(string installPath, string addAccountName, string removeAccountName)
        {
            foreach (Instance instance in this)
            {
                instance.UpdateManifestFileSecurity(installPath, addAccountName, removeAccountName);
            }
        }

    }
}
