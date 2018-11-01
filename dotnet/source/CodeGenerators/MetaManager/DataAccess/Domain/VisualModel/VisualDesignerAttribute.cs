using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class VisualDesignerAttribute : Attribute
    {
        public string ComponentName { get; set; }

        // Only used on newly created UXComponents.
        // Use the same names on the variables here as they are in UXComponents since they
        // are matched via reflection.
        public int Width { get; set; }
        public int Height { get; set; }

        public VisualDesignerAttribute()
        {
            // Set initial values for new components.
            Width = 100;
            Height = 21;
        }

        public VisualDesignerAttribute(string componentName)
            : this()
        {
            ComponentName = componentName;
        }
       
    }
}
