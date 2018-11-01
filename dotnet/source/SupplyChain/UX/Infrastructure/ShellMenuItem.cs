using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class ShellMenuItem : IAuthOperation
    {
        private bool _isAuthorized;
        private bool _isEnabled;
        private string _caption;
        private string _parameters;
        private string _eventTopic;
        private string _assemblyFile;

        public ShellMenuItem()
        {
            
        }
                
        public string Id { get; set; }

        public string EventTopic
        {
            get
            {
                return _eventTopic;
            }
            set
            {
                _eventTopic = value;
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        public string Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return _isAuthorized;
            }
            set
            {
                _isAuthorized = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public string AssemblyFile
        {
            get
            {
                return _assemblyFile;
            }
            set
            {
                _assemblyFile = value;
            }
        }

        public string Operation { get; set; }
    }
}
