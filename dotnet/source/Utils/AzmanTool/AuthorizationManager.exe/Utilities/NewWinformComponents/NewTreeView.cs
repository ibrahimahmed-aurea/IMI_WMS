using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AuthorizationManager.Utilities.New_Winform_Components
{
    class NewTreeView : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203) // identified double click
            {
                System.Drawing.Point localPos = PointToClient(Cursor.Position);
                TreeViewHitTestInfo hitTestInfo = HitTest(localPos);
                if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                    m.Result = IntPtr.Zero;
                else
                    base.WndProc(ref m);
            }
            else base.WndProc(ref m);
        }
    }
}
