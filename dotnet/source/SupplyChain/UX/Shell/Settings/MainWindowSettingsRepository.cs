using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Media;
using System.Windows;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    [Serializable]
    public class MainWindowSettingsRepository : ICloneable
    {
        public MainWindowSettingsRepository()
        {
        }

        public ThemeSettings ThemeSettings { get; set; }
        [XmlIgnore]
        public bool UseDefaultWindowSettings { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsCommonWorkspaceEnabled { get; set; }
        public double Top;
        public double Left;
        public double Width;
        public double Height;
        public double ZoomLevel;

        #region ICloneable Members

        public object Clone()
        {
            MainWindowSettingsRepository clone = (MainWindowSettingsRepository)MemberwiseClone();
            clone.ThemeSettings = (ThemeSettings)ThemeSettings.Clone();
            return clone;
        }

        #endregion
    }
}
