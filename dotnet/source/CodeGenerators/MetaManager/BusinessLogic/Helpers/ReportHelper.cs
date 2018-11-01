using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;
using NHibernate;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ReportHelper : Cdc.MetaManager.BusinessLogic.Helpers.IReportHelper
    {
        private IModelService ModelService { get; set; }
                
        public static IList<ReportQuery> GetAllReportQueries(Report report)
        {
            var queries = new List<ReportQuery>();

            foreach (ReportQuery rq in report.ReportQueries)
            { 
                queries.AddRange(GetAllReportQueriesRecursive(rq));
            }

            return queries;
        }

        [Transaction(ReadOnly = true)]
        public Report GetInitializedReport(Guid reportId)
        {
            Report report = ModelService.GetDomainObject<Report>(reportId);

            NHibernateUtil.Initialize(report);
            NHibernateUtil.Initialize(report.Application);
            NHibernateUtil.Initialize(report.RequestMap);
            
            if (report.RequestMap != null)
            {
                NHibernateUtil.Initialize(report.RequestMap.MappedProperties);
            }

            foreach (ReportQuery query in ReportHelper.GetAllReportQueries(report))
            {
                NHibernateUtil.Initialize(query.Query);
                NHibernateUtil.Initialize(query.Query.Properties);
            }

            return report;
        }

        private static IList<ReportQuery> GetAllReportQueriesRecursive(ReportQuery reportQuery)
        {
            var queries = new List<ReportQuery>();
            queries.Add(reportQuery);

            foreach (ReportQuery child in reportQuery.Children)
            { 
                queries.AddRange(GetAllReportQueriesRecursive(child));
            }

            return queries;
        }

        public static bool CheckReportParameterCombinations(ReportSQLSources existingSources, string sqlStatement, out string errorText, out ReportQuery parentReportQuery)
        {
            // Get parameters from SQL Statement
            IList<string> parameters = OracleQueryAnalyzer.GetParametersInQuery(sqlStatement);

            IList<string> uniqueParameterNames = new List<string>();

            List<ReportSQLSource> usedSources = new List<ReportSQLSource>();

            parentReportQuery = null;
            errorText = string.Empty;

            // Go through the parameters and find all the used sources from the existing ones
            foreach (string parameter in parameters)
            {
                IList<string> splitted = parameter.Split(new char[] { '.' }).ToList();

                if (splitted.Count == 1)
                {
                    // Interface parameter
                    // Check the name of the variable if it exists
                    if (existingSources.First().Variables.Where(v => v.Name == splitted[0]).FirstOrDefault() == null)
                    {
                        errorText = string.Format("The parameter \"{0}\" doesn't exist in the Report's interface.", parameter);

                        return false;
                    }

                    if (!usedSources.Contains(existingSources.First()))
                        usedSources.Add(existingSources.First());

                    if (uniqueParameterNames.Contains(splitted[0].ToUpper()))
                    {
                        errorText = string.Format("The input parameter \"{0}\" is not unique in the query.\n" +
                                                  "One or more input parameters have the same name.", splitted[0]);

                        return false;
                    }
                    else
                    {
                        uniqueParameterNames.Add(splitted[0].ToUpper());
                    }
                }
                else
                {
                    // Try to find the sql with the name of the first part of the splitted parameter.
                    ReportSQLSource source = existingSources.Where(s => s.Name == splitted[0]).FirstOrDefault();

                    if (source != null)
                    {
                        // Check variable name
                        if (source.Variables.Where(v => v.Name == splitted[1]).FirstOrDefault() == null)
                        {
                            errorText = string.Format("The parameter \"{0}\" doesn't exist in the query \"{1}\".", parameter, source.Name);

                            return false;
                        }

                        if (!usedSources.Contains(source))
                            usedSources.Add(source);
                    }
                    else
                    {
                        errorText = string.Format("The parameter \"{0}\" references an query that doesn't exist.", parameter);

                        return false;
                    }

                    if (uniqueParameterNames.Contains(splitted[1].ToUpper()))
                    {
                        errorText = string.Format("The input parameter \"{0}\" is not unique in the query.\n" +
                                                  "One or more input parameters have the same name.", parameter);
                        return false;
                    }
                    else
                    {
                        uniqueParameterNames.Add(splitted[1].ToUpper());
                    }

                }
            }

            if (usedSources.Count > 1)
            {
                ReportSQLSource deepestSource = null;

                // Check compatibility for all used sources
                foreach (ReportSQLSource source in usedSources)
                {
                    // If deepest source isnt set then start by setting the first
                    if (deepestSource == null)
                    {
                        deepestSource = source;
                    }
                    else
                    {
                        ReportSQLSource current = source;

                        // Check if any of the current source parents finds the deepestsource
                        // before hitting the top of the tree.
                        while (current != null)
                        {
                            if (current.Parent != null &&
                                current.Parent.Equals(deepestSource))
                            {
                                deepestSource = current;
                                break;
                            }

                            current = current.Parent;
                        }

                        // If we didn't find it then try to do the other way around. That is, try
                        // to find the source from the deepestSource before hitting the top of the tree.
                        if (current == null)
                        {
                            current = deepestSource;

                            while (current != null)
                            {
                                if (current.Parent != null &&
                                    current.Parent.Equals(source))
                                {
                                    break;
                                }

                                current = current.Parent;
                            }
                        }

                        if (current == null)
                        {
                            // Error. The source can't be found in the chain!
                            errorText = "There are parameters referencing queries in different branches.\n" +
                                        "You may only use parameters from queries in the same branch.";

                            return false;
                        }
                    }
                }

                // Set the parent report query to the deepest sources reportquery
                parentReportQuery = deepestSource.ReportQuery;
            }
            else if (usedSources.Count == 1)
            {
                // Set the parent report query to the found source reportquery
                parentReportQuery = usedSources[0].ReportQuery;
            }
            else if (usedSources.Count == 0)
            {
                errorText = "There are no parameters in the query.";

                return false;
            }

            return true;
        }
    }
}
