using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Imi.Framework.UX.Wpf.Services
{
    public class TextInputBarcodeScannerSection : ConfigurationSection
    {
        public const string SectionKey = "imi.framework.UX.Wpf.TextInputBarcodeScanner";

        [ConfigurationProperty("EOF_char", IsRequired = true)]
        public char EOF_char
        {
            get { return (char)this["EOF_char"]; }
            set { this["EOF_char"] = value; }
        }
    }
}
