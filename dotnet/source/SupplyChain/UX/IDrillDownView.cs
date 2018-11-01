using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX
{
    public enum DrillDownType
    {
        Drilldown, 
        JumpTo,
        OpenFile
    }

    public class DrillDownArgs
    {
        public DrillDownArgs()
        {
            Type = DrillDownType.Drilldown;
        }

        public string FieldName { get; set; }
        public string ActionId { get; set; }
        public string Caption { get; set; }
        public DrillDownType Type { get; set; }
        public string FileContentFieldName { get; set; }
    }

    public interface IDrillDownView
    {
        void EnableDrillDown(DrillDownArgs args);
        bool IsDrillDownEnabled { get; }
    }
}
