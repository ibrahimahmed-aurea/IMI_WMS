using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.Framework.ExtensionMethods;

namespace Cdc.MetaManager.DataAccess
{
    public class DataModelChanges : Dictionary<object, DataModelChange>
    {
        public new DataModelChange this[object key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];
                else
                    return null;
            }
            set
            {
                base[key] = value;
            }
        }

        public bool HintsOnly { get; set; }
        

        public void Add(object key, DataModelChangeType changeType)
        {
            Add(key, changeType, string.Empty);
        }

        public void Add(object key, DataModelChangeType changeType, IList<string> changes)
        {
            foreach (string change in changes)
            {
                Add(key, changeType, change);
            }
        }

        public void Add(object key, DataModelChangeType changeType, string change)
        {
            if (ContainsKey(key))
            {
                
                if (!string.IsNullOrEmpty(change))
                    this[key].AddChange(changeType, change);
                  
                 
            }
            else
            {
                this[key] = new DataModelChange(changeType, change);
            }
        }

        
    }
}
