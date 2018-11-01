using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic.Helpers;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ReportSQLSource
    {
        public ReportSQLSource(string name, string prefix, ReportSQLSource parent, ReportQuery reportQuery)
        {
            Name = name;
            Prefix = prefix;
            Parent = parent;
            ReportQuery = reportQuery;
        }

        public ReportSQLSource(string name) : this(name, string.Empty, null, null) { }

        public ReportSQLSource Parent { get; private set; }
        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public ReportQuery ReportQuery { get; set; }

        private ReportSQLVariables variables = null;

        public ReportSQLVariables Variables
        {
            get
            {
                if (variables == null)
                    variables = new ReportSQLVariables();

                return variables;
            }
        }

        public void AddVariable(string name, string type, string tableColumn)
        {
            Variables.Add(new ReportSQLVariable(this, name, type, tableColumn));
        }

        public void AddVariable(string name, string type, Property property)
        {
            if (property.StorageInfo != null)
                Variables.Add(new ReportSQLVariable(this, name, type, string.Format("{0}.{1}", property.StorageInfo.TableName, property.StorageInfo.ColumnName)));
            else
                Variables.Add(new ReportSQLVariable(this, name, type, string.Empty));
        }

    }

    public class ReportSQLVariable
    {
        public ReportSQLVariable(ReportSQLSource parent, string name, string type, string tableColumn)
        {
            Source = parent;
            Name = name;
            Type = type;
            TableColumn = tableColumn;
        }

        public ReportSQLSource Source { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string TableColumn { get; private set; }

        public string GetVariableName(bool includeColon)
        {
            if (string.IsNullOrEmpty(Source.Prefix))
            {
                if (includeColon)
                    return string.Format(":{0}", Name);
                else
                    return Name;
            }
            else
            {
                if (includeColon)
                    return string.Format(":{0}.{1}", Source.Prefix, Name);
                else
                    return string.Format("{0}.{1}", Source.Prefix, Name);
            }
        }

    }

    public class ReportSQLVariables : List<ReportSQLVariable> { }

    public class ReportSQLSources : List<ReportSQLSource>
    {
        private IApplicationService ApplicationService { get; set; }

        public ReportSQLSources(IApplicationService applicationService, Report report)
        {
            ApplicationService = applicationService;

            Load(report);
        }

        public IList<ReportSQLSource> Sorted()
        {
            // Always first one.
            ReportSQLSource iface = this[0];

            // Sort list alphabetically
            IList<ReportSQLSource> orderedList = this.OrderBy(s => s.Name).ToList();

            // Remove interface and put it first
            orderedList.Remove(iface);
            orderedList.Insert(0, iface);

            return orderedList;
        }

        private void Load(Report report)
        {
            // Add the interface with variables
            ReportSQLSource source = new ReportSQLSource("Report Interface");

            // Add all variables
            foreach (MappedProperty prop in report.RequestMap.MappedProperties)
            {
                source.AddVariable(prop.Name, prop.Target.Type.ToString(), prop.TargetProperty);
            }

            Add(source);

            foreach (ReportQuery reportQuery in report.ReportQueries)
            {
                AddQueryIntoSources(source, reportQuery);
            }
        }

        private void AddQueryIntoSources(ReportSQLSource parent, ReportQuery reportQuery)
        {
            // Read the query with properties
            Query query = ApplicationService.GetQueryByIdWithProperties(reportQuery.Query.Id);

            // Add the interface with variables
            ReportSQLSource source = new ReportSQLSource(query.Name, query.Name, parent, reportQuery);

            // Add the source and parameters
            foreach (QueryProperty prop in query.Properties)
            {
                if (prop.PropertyType == DbPropertyType.Out)
                {
                    if (!string.IsNullOrEmpty(prop.OriginalColumn) &&
                        !string.IsNullOrEmpty(prop.OriginalTable))
                    {
                        // Find the property for the column and table
                        Property foundProperty = ApplicationService.GetPropertyByTableAndColumn(prop.OriginalTable, prop.OriginalColumn, ReportQueryHelper.GetReport(reportQuery).Application.Id);

                        // Only get the name for the property if the query property doesn't have an alias.
                        if (foundProperty != null &&
                            prop.OriginalColumn == prop.Name)
                        {
                            source.AddVariable(foundProperty.Name, foundProperty.Type.ToString(), foundProperty);
                        }
                        else
                        {
                            source.AddVariable(prop.Name, prop.Type == null ? string.Empty : prop.Type.ToString(), string.Empty);
                        }
                    }
                    else
                    {
                        source.AddVariable(prop.Name, prop.Type == null ? string.Empty : prop.Type.ToString(), string.Empty);
                    }
                }
            }

            // Add the source to the existing source
            Add(source);

            // Loop through the reportqueries childrens
            if (reportQuery.Children.Count > 0)
            {
                foreach (ReportQuery childReportQuery in reportQuery.Children)
                {
                    AddQueryIntoSources(source, childReportQuery);
                }
            }
        }

    }
}
