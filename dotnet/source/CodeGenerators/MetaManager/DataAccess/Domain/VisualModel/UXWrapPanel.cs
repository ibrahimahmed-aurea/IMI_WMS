using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXWrapPanel : UXContainer
    {
        public UXWrapPanel()
            : base()
        {
            Orientation = UXPanelOrientation.Vertical;
        }

        public UXWrapPanel(string name)
            : base(name)
        {
            Orientation = UXPanelOrientation.Vertical;
        }

        public UXPanelOrientation Orientation { get; set; }
    }
}
