using System;
using System.Collections.Generic;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using System.Workflow.Activities.Rules;
using System.Runtime.Serialization;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Reflection;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [Serializable]
    [XmlInclude(typeof(UXCheckBox))]
    [XmlInclude(typeof(UXComboBox))]
    [XmlInclude(typeof(UXDataGrid))]
    [XmlInclude(typeof(UXDockPanel))]
    [XmlInclude(typeof(UXGroupBox))]
    [XmlInclude(typeof(UXExpander))]
    [XmlInclude(typeof(UXLabel))]
    [XmlInclude(typeof(UXLayoutGrid))]
    [XmlInclude(typeof(UXListBox))]
    [XmlInclude(typeof(UXRadioGroup))]
    [XmlInclude(typeof(UXStackPanel))]
    [XmlInclude(typeof(UXTextBox))]
    [XmlInclude(typeof(UXWrapPanel))]
    [XmlInclude(typeof(UXDataGridColumn))]
    [XmlInclude(typeof(UXSearchPanel))]
    [XmlInclude(typeof(UXSearchPanelItem))]
    [XmlInclude(typeof(UXMonthCalendar))]
    [XmlInclude(typeof(UXDatePicker))]
    [XmlInclude(typeof(UXTwoWayListBox))]
    [XmlInclude(typeof(UXComboDialog))]
    [XmlInclude(typeof(UXUpDown))]
    [XmlInclude(typeof(UXHyperDialog))]
    [XmlInclude(typeof(UXFileSelector))]
    public abstract class UXComponent
    {
        public static string DefaultName { get { return "<NoName>"; } }
        public static string Duplicate_Suffix { get { return "__DUPE"; } }

        private Rectangle area;

        protected UXComponent() : this(null)
        {
        }

        protected UXComponent(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = DefaultName;
            
            this.Name = name;

            area = new Rectangle(0, 0, Width, Height);
            this.Visibility = UXVisibility.Visible;
            RuleSetWrapper = new RuleSetWrapper();
        }
        
        public UXVisibility Visibility { get; set; }
        
        public string Name { get; set; }

        public Point Location
        {
            get
            {
                return area.Location;
            }
            set
            {
                area.Location = value;
            }
        }

        public int Top
        {
            get
            {
                return area.Top;
            }
        }

        public int Bottom
        {
            get
            {
                return area.Bottom;
            }
        }

        public int Left
        {
            get
            {
                return area.Left;
            }

        }

        public int Right
        {
            get
            {
                return area.Right;
            }

        }

        public int Width
        {
            get
            {
                return area.Width;
            }
            set
            {
                area.Width = value;
            }

        }

        public int Height
        {
            get
            {
                return area.Height;
            }
            set
            {
                area.Height = value;
            }

        }

        public Rectangle Bounds
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
            }
        }

        public bool IsReadOnly { get; set; }

        [XmlIgnore]
        public UXContainer Parent { get; set; }
                
        public RuleSetWrapper RuleSetWrapper { get; set; }


        public static string GetComponentNamePath(UXComponent component)
        {
            string namePath = string.Empty;

            UXComponent current = component;

            do
            {
                // Try to get the caption property by reflection
                PropertyInfo caption = current.GetType().GetProperty("Caption");

                string captionOrName = string.Empty;

                if (caption != null)
                {
                    captionOrName = (string)caption.GetValue(current, null);
                }

                if (string.IsNullOrEmpty(captionOrName))
                {
                    captionOrName = current.Name;
                }

                namePath = (string.IsNullOrEmpty(captionOrName) ?
                    string.Format("[{0}]", current.GetType().ToString()) :
                    captionOrName) + namePath;

                current = current.Parent;

                if (current != null)
                    namePath = "." + namePath;

            }
            while (current != null);

            return namePath;
        }
        
        private Hint hint;

        [XmlIgnore]
        [DomainReference]
        public virtual Hint Hint
        {
            get
            {
                return hint;
            }
            set
            {
                hint = value;

                if (hint == null)
                    hintId = Guid.Empty;
            }
        }

        private Guid hintId;

        public Guid HintId
        {
            get
            {
                if (Hint != null)
                    return Hint.Id;
                else
                    return hintId;
            }
            set
            {
                hintId = value;
            }
        }
       
        public override string ToString()
        {
            return Name;
        }

    }
}
