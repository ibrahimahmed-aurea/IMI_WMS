using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.UX
{
    public class MenuEventArgs : EventArgs
    {
        private string _caption;
        private string _parameters;
        private bool _openInNewWindow;
        private string _menuItemId;

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public MenuEventArgs()
            : this(null, null, false, string.Empty)
        {
        }

        public MenuEventArgs(string caption, string parameters)
            : this(caption, parameters, false, string.Empty)
        {
        }

        public MenuEventArgs(string caption, string parameters, bool openInNewWindow, string menuItemId)
        {
            _caption = caption;
            _parameters = parameters;
            _openInNewWindow = openInNewWindow;
            _menuItemId = menuItemId;
        }
                
        public bool OpenInNewWindow
        {
            get { return _openInNewWindow; }
            set { _openInNewWindow = value; }
        }

        public string MenuItemId
        {
            get { return _menuItemId; }
            set { _menuItemId = value; }
        }
    }

}
