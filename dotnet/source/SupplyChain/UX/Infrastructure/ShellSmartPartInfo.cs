using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class ShellSmartPartInfo : ISmartPartInfo
    {
        private string _description;
        private string _title;
        private ShellHyperlink _hyperlink;
        
        public ShellSmartPartInfo(string title, string description)
            : this(title, description, null)
        {
        }

        public ShellSmartPartInfo(string title, string description, ShellHyperlink hyperlink)
        {
            _title = title;
            _description = description;
            _hyperlink = hyperlink;
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public ShellHyperlink Hyperlink
        {
            get
            {
                return _hyperlink;
            }
            set
            {
                _hyperlink = value;
            }
        }
    }
}
