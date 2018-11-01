using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.DataGrid.Settings;

namespace Imi.Framework.Wpf.Controls
{
    public class DataGridSettingsRepository : SettingsRepository
    {
        protected override ColumnSettings CreateColumnSettings()
        {
            return new DataGridColumnSettings();
        }
    }

    public class DataGridColumnSettings : ColumnSettings
    {
        public string GroupStringFormat { get { return base.GroupStringFormat; } set { base.GroupStringFormat = value; } }
        public bool? IsOrphan { get { return base.IsOrphan; } set { base.IsOrphan = value; } }
        public string StringFormat { get { return base.StringFormat; } set { base.StringFormat = value; } }
        public XmlColumnWidth? Width { get { return base.Width; } set { base.Width = value; } }
        public bool? Visible { get { return base.Visible; } set { base.Visible = value; } }
        public int? VisiblePosition { get { return base.VisiblePosition; } set { base.VisiblePosition = value; } }

        public object Title { get { return null; } set { base.Title = null; } }

        public DataGridColumnSettings() { }
    }
}
