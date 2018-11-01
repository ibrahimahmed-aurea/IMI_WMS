using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.Framework.UX.Wpf.Workspaces
{
    public class GridSmartPartInfo : SmartPartInfo
    {
        private int column;
        private int row;

        public int Row
        {
            get { return row; }
            set { row = value; }
        }
	
        public int Column
        {
            get { return column; }
            set { column = value; }
        }
	
    }
}
