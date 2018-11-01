using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Imi.SupplyChain.UX.Workflow
{
    [ActivityDesignerTheme(typeof(ResultDesignerTheme))]
    public class ResultActivityDesigner : ActivityDesigner
    {
    }

    public class ResultDesignerTheme : ActivityDesignerTheme
    {
        public ResultDesignerTheme(WorkflowTheme theme)
            : base(theme)
        {
            BackColorStart = Color.Gainsboro;
            BackColorEnd = Color.Silver;
            BackgroundStyle = LinearGradientMode.Horizontal;
            BorderColor = Color.Gray;
        }
    }

}
