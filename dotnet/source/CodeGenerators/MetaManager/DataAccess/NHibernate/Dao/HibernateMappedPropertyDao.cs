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
    public class HibernateMappedPropertyDao : HibernateDaoSupport, IMappedPropertyDao
    {
        #region IMappedPropertyDao Members

        [Transaction(ReadOnly = true)]
        MappedProperty IMappedPropertyDao.FindById(Guid mappedPropertyId)
        {
            return HibernateTemplate.Get<MappedProperty>(mappedPropertyId);
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindByQueryPropertyId(Guid queryPropertyId)
        {
            string[] paramNames = { "queryPropertyId" };
            object[] paramValues = { queryPropertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindByQueryPropertyId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindByProcedurePropertyId(Guid procedurePropertyId)
        {
            string[] paramNames = { "procedurePropertyId" };
            object[] paramValues = { procedurePropertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindByProcedurePropertyId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindByPropertyId(Guid propertyId)
        {
            string[] paramNames = { "propertyId" };
            object[] paramValues = { propertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindByPropertyId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindByTargetPropertyId(Guid propertyId)
        {
            string[] paramNames = { "propertyId" };
            object[] paramValues = { propertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindByTargetPropertyId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindOwnerVisualTree(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { "%>" + propertyMapId + "<%"};

            IList<View> result = HibernateTemplate.FindByNamedQueryAndNamedParam<View>(
                                     "MappedProperty.FindOwnerVisualTree",
                                     paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public object FindOwner(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<object> result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                                 "MappedProperty.FindOwnerView",
                                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                                 "MappedProperty.FindOwnerViewNode",
                                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                                 "MappedProperty.FindOwnerViewAction",
                                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                             "MappedProperty.FindOwnerAction",
                             paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                 "MappedProperty.FindOwnerServiceMethod",
                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                 "MappedProperty.FindOwnerUXAction",
                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            result = HibernateTemplate.FindByNamedQueryAndNamedParam<object>(
                 "MappedProperty.FindOwnerDataSource",
                 paramNames, paramValues);

            if (result.Count > 0)
                return result.First();

            return null;
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindBySourcesById(Guid mappedPropertyId)
        {
            string[] paramNames = { "mappedPropertyId" };
            object[] paramValues = { mappedPropertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindSourcesById",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<MappedProperty> FindByTargetViewInterfacePropertyId(Guid viewInterfacePropertyId)
        {
            string[] paramNames = { "viewInterfacePropertyId" };
            object[] paramValues = { viewInterfacePropertyId };

            IList<MappedProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<MappedProperty>(
                                 "MappedProperty.FindByTargetViewInterfacePropertyId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        IList<MappedProperty> IMappedPropertyDao.FindAll()
        {
            return HibernateTemplate.LoadAll<MappedProperty>();
        }

        [Transaction(ReadOnly = false)]
        public MappedProperty Save(MappedProperty mappedProperty)
        {
            if (mappedProperty.Id == Guid.Empty) { mappedProperty.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(mappedProperty);
            return mappedProperty;
        }

        [Transaction(ReadOnly = false)]
        public MappedProperty SaveOrUpdate(MappedProperty mappedProperty)
        {
            HibernateTemplate.SaveOrUpdate(mappedProperty);
            return mappedProperty;
        }

        [Transaction(ReadOnly = false)]
        public MappedProperty SaveOrUpdateMerge(MappedProperty mappedProperty)
        {
            object mergedObj = Session.Merge(mappedProperty);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (MappedProperty)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(MappedProperty mappedProperty)
        {
            HibernateTemplate.Delete(mappedProperty);
        }

        [Transaction(ReadOnly = true)]
        public IList<string> FindAllDisplayFormatsUsed(Type displayFormatDataType)
        {
            string[] paramNames = { "displayFormatDataType" };
            object[] paramValues = { string.Format("{0}%", displayFormatDataType.ToString()) };

            IList<string> result = HibernateTemplate.FindByNamedQueryAndNamedParam<string>(
                                 "MappedProperty.FindAllDisplayFormatsUsed",
                                 paramNames, paramValues);

            return result;
        }

        #endregion
                
    }
}
