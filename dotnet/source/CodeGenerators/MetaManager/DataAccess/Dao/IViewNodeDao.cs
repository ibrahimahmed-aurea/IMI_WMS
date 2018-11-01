using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IViewNodeDao
    {
        ViewNode FindById(Guid viewNodeId);
        IList<ViewNode> FindAll(Guid applicationId);
        IList<ViewNode> FindAllByViewId(Guid viewId);
        IList<ViewNode> FindAllByDialogId(Guid dialogId);
        long CountByViewId(Guid viewId);
        ViewNode Save(ViewNode viewNode);
        ViewNode SaveOrUpdate(ViewNode viewNode);
        ViewNode SaveOrUpdateMerge(ViewNode viewNode);
        void Delete(ViewNode viewNode);
        ViewNode FindByPropertyMapId(Guid propertyMapId);
    }
}

