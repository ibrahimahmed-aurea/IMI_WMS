using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ReportQueryHelper : Cdc.MetaManager.BusinessLogic.Helpers.IReportQueryHelper
    {
        private IModelService ModelService { get; set; }
        private IConfigurationManagementService ConfigurationManagementService { get; set; }

        private List<QueryProperty> queryPropertyToDelete = new List<QueryProperty>();

        public static Report GetReport(ReportQuery reportQuery)
        {
            while (reportQuery != null)
            {
                if (reportQuery.Report != null)
                {
                    return reportQuery.Report;
                }

                reportQuery = reportQuery.Parent;
            }

            return null;
        }

        [Transaction(ReadOnly = false)]
        public void SaveAndSynchronize(ReportQuery reportQuery)
        {
            Guid schemaId = Guid.Empty;
            Application schemaApp = null;

            if (reportQuery.Query.Id != Guid.Empty)
            {
                Query dbQuery = ModelService.GetInitializedDomainObject<Query>(reportQuery.Query.Id);

                bool propertiesAdded;

                MetaManagerServices.Helpers.QueryHelper.SyncProperties(reportQuery.Query, dbQuery, HandleDeletedQueryProperties, out propertiesAdded);

                foreach (QueryProperty qp in queryPropertyToDelete)
                {
                    dbQuery.Properties.Remove(qp);
                }

                reportQuery.Query = dbQuery;
            }

            ModelService.MergeSaveDomainObject(reportQuery);

        }

        private bool HandleDeletedQueryProperties(IList<QueryProperty> propertyList)
        {
            queryPropertyToDelete.Clear();

            queryPropertyToDelete.AddRange(propertyList.ToArray());

            return true;
        }
    }
}
