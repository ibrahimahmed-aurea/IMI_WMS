using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateDialogDao : HibernateDaoSupport, IDialogDao
    {
        #region IDialogDao Members

        [Transaction(ReadOnly = true)]
        public Dialog FindById(Guid dialogId)
        {
            return HibernateTemplate.Get<Dialog>(dialogId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindDialogs(Guid applicationId, string moduleName, string dialogName, string originalDialogName, string title)
        {
            if (string.IsNullOrEmpty(moduleName))
                moduleName = "%";

            if (string.IsNullOrEmpty(dialogName))
                dialogName = "%";

            if (string.IsNullOrEmpty(originalDialogName))
                originalDialogName = "%";

            if (string.IsNullOrEmpty(title))
                title = "%";

            string[] paramNames = { "applicationId", "moduleName", "dialogName", "originalDialogName", "title" };
            object[] paramValues = {applicationId, moduleName, dialogName, originalDialogName, title };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindDialogs", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindDialogsByNameAndModule(Guid applicationId, string moduleName, string dialogName)
        {
            
            string[] paramNames = { "applicationId", "moduleName", "dialogName" };
            object[] paramValues = { applicationId, moduleName, dialogName };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindDialogsByNameAndModule", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindAllDialogsWithInterfaceView(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindAllDialogsWithInterfaceView", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindAllDialogsWithInterfaceViewByDialogType(Guid applicationId, DialogType dialogType)
        {
            string[] paramNames = { "applicationId", "dialogType" };
            object[] paramValues = { applicationId, dialogType };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindAllDialogsWithInterfaceViewByDialogType", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindAllDrilldownDialogs(Guid applicationId)
        {
            string[] paramNames = { "applicationId", "dialogType" };
            object[] paramValues = { applicationId, (int)DialogType.Drilldown };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindAllDrilldownDialogs", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindAllByModule(Guid moduleId)
        {
            string[] paramNames = { "moduleId"};
            object[] paramValues = { moduleId};

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Dialog>("Dialog.FindAllByModule", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public Dialog Save(Dialog dialog)
        {
            if (dialog.Id == Guid.Empty) { dialog.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(dialog);
            return dialog;
        }

        [Transaction(ReadOnly = false)]
        public Dialog SaveOrUpdate(Dialog dialog)
        {
            HibernateTemplate.SaveOrUpdate(dialog);
            return dialog;
        }

        [Transaction(ReadOnly = false)]
        public Dialog SaveOrUpdateMerge(Dialog dialog)
        {
            object mergedObj = Session.Merge(dialog);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Dialog)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Dialog dialog)
        {
            HibernateTemplate.Delete(dialog);
        }


        #endregion
    }
}
