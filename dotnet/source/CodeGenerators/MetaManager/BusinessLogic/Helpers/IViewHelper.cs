using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess.Domain;
using System.Reflection;
using Cdc.MetaManager.DataAccess;
using NHibernate;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IViewHelper
    {
        void InitializeUXComponent(UXComponent component);
        void InitializeUXComponent(UXComponent component, Dictionary<Guid, IDomainObject> loadedObjects);
        void FindComponentServiceReferences(UXComponent component, IDictionary<Guid, Service> references);
        View ConnectServiceMethodToView(Guid viewId, Guid serviceMethodId);
        View CopyView(View view);
        void SetHints(View view);
    }
}
