using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXChildCollection : Collection<UXComponent>
    {
        private UXContainer container;

        public UXChildCollection(UXContainer container)
        {
            this.container = container;
        }

        protected override void InsertItem(int index, UXComponent item)
        {
            item.Parent = container;

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, UXComponent item)
        {
            item.Parent = container;

            base.SetItem(index, item);
        }
    }

    public abstract class UXContainer : UXComponent
    {
        private UXChildCollection children;
        
        protected UXContainer()
            : this(null)
        {
            
        }

        protected UXContainer(string name)
            : base(name)
        {
            children = new UXChildCollection(this);
        }
                
        public virtual UXChildCollection Children
        {
            get
            {
                return children;
            }
        }
    }
}
