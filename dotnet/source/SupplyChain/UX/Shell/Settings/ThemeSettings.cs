using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    [Serializable]
    public class ThemeSettings : ICloneable
    {
        public bool IsGlassEnabled { get; set; }
        public string ThemeName { get; set; }
        public Color? TintColor { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
