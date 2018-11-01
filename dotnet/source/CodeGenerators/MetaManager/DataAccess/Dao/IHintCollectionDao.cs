using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IHintCollectionDao
    {
        HintCollection FindById(Guid hintCollectionId);
        IList<HintCollection> FindAll(Guid applicationId);
        HintCollection Save(HintCollection hintCollection);
        HintCollection SaveOrUpdate(HintCollection hintCollection);
        HintCollection SaveOrUpdateMerge(HintCollection hintCollection);
        void Delete(HintCollection hintCollection);
    }
}
