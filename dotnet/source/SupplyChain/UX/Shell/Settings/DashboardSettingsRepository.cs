using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    [Serializable]
    public class DashboardSettingsRepository : ICloneable
    {
        private List<ShellHyperlink> _activationLinks;
        
        public DashboardSettingsRepository()
        {
            _activationLinks = new List<ShellHyperlink>();
        }

        public string Layout { get; set; }

        public List<ShellHyperlink> ActivationLinks
        {
            get
            {
                return _activationLinks;
            }
            set
            {
                _activationLinks = value;
            }
        }

        public double RefreshInterval { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
