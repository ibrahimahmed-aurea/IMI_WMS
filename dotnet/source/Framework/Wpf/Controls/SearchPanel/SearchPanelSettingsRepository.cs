using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Imi.Framework.Wpf.Controls
{
    [Serializable]
    public class SearchPanelItemSetting
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
    }

    [Serializable]
    public class SearchPanelSettingsRepository
    {
        [XmlArray]
        [XmlArrayItem(typeof(SearchPanelItemSetting))]
        public SearchPanelItemSetting[] Items
        { 
            get;
            set; 
        }
    }
}
