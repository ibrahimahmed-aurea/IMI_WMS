using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX
{
    public class UserSettingsChangedEventArgs : EventArgs
    {
        private IList<string> _openDialogs;
        private bool _forceClose;

        public UserSettingsChangedEventArgs()
            : this(false)
        {
        }

        public UserSettingsChangedEventArgs(bool forceClose)
        {
            _openDialogs = new List<string>();
            _forceClose = forceClose;
        }

        public IList<string> OpenDialogs
        {
            get
            {
                return _openDialogs;
            }
        }

        public bool ForceClose
        {
            get
            {
                return _forceClose;
            }
        }
    }
}
