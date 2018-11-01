using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class QueryHelper
    {
        public void UpdateQuery(Query updateQuery, OracleQuery oracleQuery)
        {
            if (updateQuery != null &&
                oracleQuery != null)
            {
                updateQuery.SqlStatement = oracleQuery.SQL;

                updateQuery.Properties.Clear();

                foreach (OracleQueryParameter parameter in oracleQuery.Parameters)
                {
                    QueryProperty qp = new QueryProperty();

                    qp.DbDatatype = parameter.DbDatatype;

                    if (parameter.Direction == OracleQueryParameterDirection.In)
                        qp.PropertyType = DbPropertyType.In;
                    else if (parameter.Direction == OracleQueryParameterDirection.Out)
                        qp.PropertyType = DbPropertyType.Out;

                    qp.Length = parameter.Length;
                    qp.Name = parameter.Name;
                    qp.OriginalColumn = parameter.OutColumnName;
                    qp.OriginalTable = parameter.OutTableName;
                    qp.Precision = parameter.Precision;
                    qp.Scale = parameter.Scale;
                    qp.Sequence = parameter.Sequence;
                    qp.Text = parameter.Text;
                    qp.Query = updateQuery;

                    updateQuery.Properties.Add(qp);
                }
            }
        }

        //// Find database query
        //Query oldQuery = applicationService.GetQueryByIdWithProperties(newQuery.Id);

        // Procedure to handle deletion of queryproperties that should be deleted from the dbquery
        public delegate bool DelegateHandleDeletedQueryPropertyList(IList<QueryProperty> propertyList);

        /// <summary>
        ///     Sync Queryproperties from a new edited query to the one in existing in the database.
        ///     When procedure is done then the dbQuery is ready to be saved back to database.
        /// </summary>
        /// <param name="newQuery">The new query where the queryproperties has been changed already.</param>
        /// <param name="dbQuery">The newly read query with it's parameters.</param>
        /// <param name="delegateHandleDeletedList">A delegate to a procedure handling the list of deleted QueryParameters in the dbQuery.</param>
        /// <param name="propertiesAdded">Returns true if parameters has been added. False if not.</param>
        /// <returns>Returns true if sync is done correctly or false if anything is wrong.</returns>
        public bool SyncProperties(Query newQuery, 
                                          Query dbQuery, 
                                          DelegateHandleDeletedQueryPropertyList delegateHandleDeletedList, 
                                          out bool propertiesAdded)
        {
            propertiesAdded = false;

            List<QueryProperty> addList = new List<QueryProperty>();
            List<QueryProperty> changeList = new List<QueryProperty>();
            List<QueryProperty> nameChangeList = new List<QueryProperty>();
            List<QueryProperty> deleteList = new List<QueryProperty>(dbQuery.Properties);

                // Compare new query properties to old ones
            // determine cause of action, i.e. Add new Properties, Delete removed Properties
            // or update data type
            foreach (QueryProperty newQp in newQuery.Properties)
            {
                QueryProperty original = FindPropertyByName(newQp.Name, newQp.PropertyType, deleteList);

                // Check if we found a property by name
                if (original != null)
                {
                    // Property found, ok check if rest is unchanged too
                    if (!AreQueryPropertiesTheSame(newQp, original))
                    {
                        // Datatype changed
                        changeList.Add(newQp);
                    }

                    // Mark as treated
                    deleteList.Remove(original);
                }
                else
                {
                    // See if we can find a property that has the same text but maybe has changed name.
                    original = FindPropertyByText(newQp, newQp.PropertyType, deleteList);

                    if (original != null)
                    {
                        // Name changed
                        nameChangeList.Add(newQp);

                        // Mark as treated
                        deleteList.Remove(original);
                    }
                    else
                    {
                        // A new property
                        addList.Add(newQp);
                    }
                }
            }

            // What is left in deleteList should be removed
            if (deleteList.Count > 0 && delegateHandleDeletedList != null)
            {
                if (!delegateHandleDeletedList(deleteList))
                    return false;
            }

            // Add new properties
            if (addList.Count > 0)
            {
                propertiesAdded = true;

                foreach (QueryProperty addProp in addList)
                {
                    addProp.Query = dbQuery;
                    dbQuery.Properties.Add(addProp);
                }
            }

            if (changeList.Count > 0 || nameChangeList.Count > 0)
            {
                foreach (QueryProperty changeProp in changeList)
                {
                    QueryProperty dbQp = FindPropertyByName(changeProp.Name, changeProp.PropertyType, dbQuery.Properties);

                    if (dbQp != null)
                    {
                        dbQp.DbDatatype = changeProp.DbDatatype;
                        dbQp.Length = changeProp.Length;
                        dbQp.Precision = changeProp.Precision;
                        dbQp.Scale = changeProp.Scale;
                        dbQp.OriginalColumn = changeProp.OriginalColumn;
                        dbQp.OriginalTable = changeProp.OriginalTable;
                        dbQp.Text = changeProp.Text;
                        dbQp.Sequence = changeProp.Sequence;
                    }
                }

                foreach (QueryProperty nameChange in nameChangeList)
                {
                    QueryProperty dbQp = FindPropertyByText(nameChange, nameChange.PropertyType, dbQuery.Properties);

                    if (dbQp != null)
                    {
                        dbQp.Name = nameChange.Name;
                        dbQp.Text = nameChange.Text;
                        dbQp.DbDatatype = nameChange.DbDatatype;
                        dbQp.Length = nameChange.Length;
                        dbQp.Precision = nameChange.Precision;
                        dbQp.Scale = nameChange.Scale;
                        dbQp.OriginalColumn = nameChange.OriginalColumn;
                        dbQp.OriginalTable = nameChange.OriginalTable;
                        dbQp.Sequence = nameChange.Sequence;
                    }
                }
            }

            // Update the database query with the new sqlstatement
            dbQuery.SqlStatement = newQuery.SqlStatement;

            return true;
        }

        public QueryProperty FindPropertyByName(string propertyName, DbPropertyType direction, IList<QueryProperty> propertyList)
        {
            IEnumerable<QueryProperty> foundProperties =
                from QueryProperty property in propertyList
                where property.Name == propertyName &&
                      property.PropertyType == direction
                select property;

            if (foundProperties.Count() > 0)
                return foundProperties.First();
            else
                return null;
        }

        public QueryProperty FindPropertyByText(QueryProperty testProperty, DbPropertyType direction, IList<QueryProperty> propertyList)
        {
            // Remove the alias if it exists at the end since the name of the
            // alias might have changed.

            if (string.IsNullOrEmpty(testProperty.Text)) //thro
                return null;

            string testText = Regex.Replace(testProperty.Text, string.Format(@"\s+{0}$", testProperty.Name), string.Empty);

            IEnumerable<QueryProperty> foundProperties =
                from QueryProperty property in propertyList
                where !string.IsNullOrEmpty(property.Text) && Regex.Replace(property.Text, string.Format(@"\s+{0}$", property.Name), string.Empty) == testText &&
                      property.PropertyType == direction
                select property;

            if (foundProperties.Count() > 0)
                return foundProperties.First();
            else
                return null;
        }

        public bool AreQueryPropertiesTheSame(QueryProperty firstProp, QueryProperty secondProp)
        {
            return
                (
                    (firstProp.DbDatatype == secondProp.DbDatatype) &&
                    (firstProp.Length == secondProp.Length) &&
                    (firstProp.Precision == secondProp.Precision) &&
                    (firstProp.Scale == secondProp.Scale) &&
                    (firstProp.OriginalColumn == secondProp.OriginalColumn) &&
                    (firstProp.OriginalTable == secondProp.OriginalTable) &&
                    (firstProp.Sequence == secondProp.Sequence)
                );
        }

    }
}
