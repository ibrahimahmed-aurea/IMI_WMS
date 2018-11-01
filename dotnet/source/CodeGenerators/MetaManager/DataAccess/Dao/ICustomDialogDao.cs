using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface ICustomDialogDao
    {
        CustomDialog FindById(Guid customDialogId);
        IList<CustomDialog> FindAll(Guid applicationId);
        CustomDialog SaveOrUpdate(CustomDialog customDialog);
        CustomDialog SaveOrUpdateMerge(CustomDialog customDialog);
        void Delete(CustomDialog customDialog);
    }
}
