using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IMappedPropertyDao
    {
        MappedProperty FindById(Guid mappedPropertyId);
        IList<MappedProperty> FindByTargetPropertyId(Guid propertyId);
        IList<MappedProperty> FindBySourcesById(Guid mappedPropertyId);
        IList<MappedProperty> FindByTargetViewInterfacePropertyId(Guid viewInterfacePropertyId);
        IList<MappedProperty> FindAll();
        MappedProperty Save(MappedProperty mappedProperty);
        MappedProperty SaveOrUpdate(MappedProperty mappedProperty);
        MappedProperty SaveOrUpdateMerge(MappedProperty mappedProperty);
        void Delete(MappedProperty mappedProperty);
        object FindOwner(Guid propertyMapId);
        IList<View> FindOwnerVisualTree(Guid propertyMapId);
        IList<MappedProperty> FindByQueryPropertyId(Guid queryPropertyId);
        IList<MappedProperty> FindByProcedurePropertyId(Guid procedurePropertyId);
        IList<MappedProperty> FindByPropertyId(Guid propertyId);
        IList<string> FindAllDisplayFormatsUsed(Type displayFormatDataType);
    }
}
