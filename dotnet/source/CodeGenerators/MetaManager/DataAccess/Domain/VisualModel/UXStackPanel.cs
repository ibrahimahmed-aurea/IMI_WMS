using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXStackPanel : UXContainer
    {
        public UXStackPanel()
            : base()
        {
            Orientation = UXPanelOrientation.Horizontal;
        }

        public UXStackPanel(string name)
            : base(name)
        {
            Orientation = UXPanelOrientation.Horizontal;
        }

        public UXPanelOrientation Orientation  { get; set; }
    }
}
