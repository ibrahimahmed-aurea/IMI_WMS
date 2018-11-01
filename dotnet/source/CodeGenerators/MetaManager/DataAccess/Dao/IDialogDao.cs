using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IDialogDao
    {
        Dialog FindById(Guid dialogId);
        IList<Dialog> FindAllDialogsWithInterfaceView(Guid applicationId);
        IList<Dialog> FindAllDialogsWithInterfaceViewByDialogType(Guid applicationId, DialogType dialogType);
        IList<Dialog> FindAllDrilldownDialogs(Guid applicationId);
        IList<Dialog> FindDialogs(Guid applicationId, string moduleName, string dialogName, string originalDialogName, string title);
        IList<Dialog> FindDialogsByNameAndModule(Guid applicationId, string moduleName, string dialogName);
        IList<Dialog> FindAllByModule(Guid moduleId);
        IList<Dialog> FindAll(Guid applicationId);
        Dialog Save(Dialog dialog);
        Dialog SaveOrUpdate(Dialog dialog);
        Dialog SaveOrUpdateMerge(Dialog dialog);
        void Delete(Dialog dialog);
    }
}
