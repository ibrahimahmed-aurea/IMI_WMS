using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace Imi.Wms.Mobile.UI.Configuration
{
    public class UISection
    {
        public const string SectionKey = "imi.wms.mobile.ui";
        private ServerCollection _serverCollection;
        private FontCollection _fontCollection;
        private WindowsDesktopSettingsCollection _windowsDesktopSettingsCollection;

        public UISection()
        {
            _serverCollection = new ServerCollection();
            _fontCollection = new FontCollection();
            _windowsDesktopSettingsCollection = new WindowsDesktopSettingsCollection();
            ConnectTimeout = 5;
            ReceiveTimeout = 10;
            SendTimeout = 5;
            RetryCount = 2;
        }

        public int ConnectTimeout
        {
            get;
            set;
        }

        public int ReceiveTimeout
        {
            get;
            set;
        }

        public int SendTimeout
        {
            get;
            set;
        }

        public int RetryCount
        {
            get;
            set;
        }

        public bool LogEnabled
        {
            get;
            set;
        }

        public string TerminalId
        {
            get;
            set;
        }

        public string LastSessionId
        {
            get;
            set;
        }

        public string NativeDriver
        {
            get;
            set;
        }
        
        public ServerCollection ServerCollection
        {
            get
            {
                return _serverCollection;
            }
            set
            {
                _serverCollection = value;
            }
        }

        public FontCollection FontCollection
        {
            get
            {
                return _fontCollection;
            }
            set
            {
                _fontCollection = value;
            }
        }

        public WindowsDesktopSettingsCollection WindowsDesktopSettingsCollection
        {
            get
            {
                return _windowsDesktopSettingsCollection;
            }
            set
            {
                _windowsDesktopSettingsCollection = value;
            }
        }
    }
               
    
    public class ServerCollection : List<ServerElement>
    {
        
    }

    public class ServerElement
    {
        public string Name
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool Default
        {
            get;
            set;
        }

        public string DefaultApplication
        {
            get;
            set;
        }
    }

    public class FontCollection : List<FontElement>
    {

    }

    public class FontElement
    {
        public string OldName
        {
            get;
            set;
        }

        public string NewName
        {
            get;
            set;
        }

        public float SizeAdjust
        {
            get;
            set;
        }
    }

    public class WindowsDesktopSettingsCollection : Dictionary<string,Setting>
    {

    }

    public class Setting
    {
        public string Key {get; set;}
        public string Value {get; set;}
    }
}
