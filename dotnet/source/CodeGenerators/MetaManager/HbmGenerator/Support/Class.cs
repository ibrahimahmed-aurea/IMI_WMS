using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Imi.HbmGenerator.Attributes;

namespace Cdc.HbmGenerator.Support
{
    public class Class
    {
        public Class()
        {
            StorageHint = new ClassStorageHintAttribute();
        }

        // public virtual int Id { get; set; } not needed right now
        public virtual Type Type { get; set; }

        //public virtual string Table { get; set; }
        //public virtual bool GenerateDao { get; set; }

        public ClassStorageHintAttribute StorageHint;

        private IList<Property> properties;

        public virtual IList<Property> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<Property>();
                }
                return properties;
            }
            set { properties = value; }

        }

        public string AssemblyShortName
        {
            get
            {
                return Type.Assembly.FullName.Split(new char[] { ',' })[0];
            }
        }
    }
}
