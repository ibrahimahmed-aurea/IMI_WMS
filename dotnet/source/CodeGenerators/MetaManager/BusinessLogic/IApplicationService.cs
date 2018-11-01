using System;
using System.Collections;
using System.Collections.Generic;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IApplicationService
    {
        ServiceMethod GetServiceMethodWithRequestMap(Guid serviceMethodId);
        IList<ServiceMethod> GetAllServiceMethodsToQueriesByApplication(Guid applicationId);
        ServiceMethod GetServiceMethodMapsById(Guid serviceMethodId);
        Schema GetSchemaByApplicationId(Guid applicationId);
        Query GetQueryByIdWithProperties(Guid queryId);
        IList<UXSessionProperty> GetUXSessionProperties(Application application);
        void GetServiceMethodResponseMap(ServiceMethod serviceMethod, out PropertyMap responseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void GetServiceMethodRequestMap(ServiceMethod serviceMethod, out PropertyMap requestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        PropertyMap SaveAndMergePropertyMap(PropertyMap map);
        IList<DeploymentGroup> GetAllDeploymentGroups();
        IList<string> FindAllDisplayFormatsUsed(Type displayFormatDataType);
        Property GetPropertyByTableAndColumn(string tableName, string columnName, Guid applicationId);

        bool MMDbExsisting();
    }
}
