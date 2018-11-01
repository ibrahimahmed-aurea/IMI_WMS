using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public class OracleQueryAnalyzer
    {
        // Dictionary for keeping track of unique select names.
        // The key is the name and the value is the sequence number.
        private static Dictionary<string, int> selectNameUnique = new Dictionary<string, int>();

        // Dictionary for aliases for tables found in from rows.
        // The key is the alias and the table is set as the value.
        // If a table doesn't have an alias set then it isn't included in this
        // dictionary. Only the "fromTables" dictionary.
        private static Dictionary<string, string> fromAliases = new Dictionary<string, string>();

        public static Dictionary<string, string> FromAliases 
        {
            get
            {
                return fromAliases;
            }
        }

        // Dictionary for all valid tables found in the from rows.
        // Key is Table. If an alias exist it's set as value.
        // If an alias exist then the this will also be inserted in the "fromAliases" dictionary.
        private static Dictionary<string, string> fromTables = new Dictionary<string, string>();

        public static Dictionary<string, string> FromTables
        {
            get
            {
                return fromTables;
            }
        }

        public static OracleQuery Analyze(string sql, string oracleConnectionString)
        {
            return Analyze(sql, oracleConnectionString, false);
        }

        public static OracleQuery Analyze(string sql, string oracleConnectionString, bool isSpecialWhereParameters)
        {
            OracleQuery query = new OracleQuery();
            query.SQL = sql;

            OracleQueryAnalyzer analyzer = new OracleQueryAnalyzer();

            analyzer.DoAnalyze(query, oracleConnectionString, isSpecialWhereParameters);

            return query;
        }

        private static string TrimComments(string sql)
        {
            MatchCollection sqlMatches = RegexHelper.Parse(RegexConstants.SQL_COMMENT_FIND, sql);

            return Regex.Replace(sql, RegexConstants.SQL_COMMENT_FIND, (m) => 
            {
                if (!string.IsNullOrEmpty(m.Groups[RegexConstants.SQL_COMMENT].Value))
                    return "";
                else
                    return m.Value;
            
            });
        }

        private bool DoAnalyze(OracleQuery query, string oracleConnectionString, bool isSpecialWhereParameters)
        {
            bool ok = true;

            if (query != null)
            {
                string sql = TrimComments(query.SQL);

                // Reset the query before analyze
                query.Parameters.Clear();

                selectNameUnique.Clear();
                fromAliases.Clear();
                fromTables.Clear();

                // Get matches for the SQL
                MatchCollection sqlMatches = RegexHelper.Parse(RegexConstants.SQL_MAIN, sql);

                if (sqlMatches != null)
                {
                    // Fetch all parameters in SQL
                    SetQueryParameters(sql, query, isSpecialWhereParameters);

                    foreach (Match sqlMatch in sqlMatches)
                    {
                        // If there is something in SQLSPLITTER (contains union, intersect or minus if they exist)
                        // then clear the unique names since it's a completely different sql
                        if (!string.IsNullOrEmpty(sqlMatch.Groups[RegexConstants.SQL_MAIN_SQLSPLITTER].Value))
                            selectNameUnique.Clear();

                        // Check if distinct is found
                        if (!string.IsNullOrEmpty(sqlMatch.Groups[RegexConstants.SQL_MAIN_DISTINCT].Value))
                        {
                            query.ParseWarnings.Add("The query contains \"select distinct\" which should be avoided if possible." + Environment.NewLine +
                                                    "Please consider to rewrite the query!");
                        }

                        // Get from rows from the SQL
                        try
                        {
                            GetFromRows(sqlMatch.Groups[RegexConstants.SQL_MAIN_FROMROWS].Captures);

                            GetSelectRows(sqlMatch.Groups[RegexConstants.SQL_MAIN_SELECTROWS].Captures, query);

                            GetWhereRows(sqlMatch.Groups[RegexConstants.SQL_MAIN_WHEREROWS].Captures, query, isSpecialWhereParameters);
                        }
                        catch (Exception ex)
                        {
                            query.ParseErrors.Add(ex.Message);

                            ok = false;
                        }
                    }
                }
                else
                {
                    query.ParseErrors.Add("Couldn't parse the SQL (no matches found). Please analyze!");

                    ok = false;
                }
            }

            if (ok)
            {
                try
                {
                    FindDataTypes(query, oracleConnectionString, isSpecialWhereParameters);
                }
                catch (Exception ex)
                {
                    query.ParseErrors.Add(ex.Message);
                    ok = false;
                }
            }

            return ok;
        }

        private void FindDataTypes(OracleQuery query, string oracleConnectionString, bool isSpecialWhereParameters)
        {
            if (!String.IsNullOrEmpty(oracleConnectionString))
            {
                // If special where parameters then there could be parameters on the format :XXXX.YYYY
                // which Oracle doesn't like. So we fix these before doing the call to get
                // the datatypeinfo.
                string sql = TrimComments(query.SQL);

                if (isSpecialWhereParameters)
                {
                    sql = Regex.Replace(sql, @":(([A-Za-z]\w*)\.?){1,2}(?!\.)", ":param");
                }

                OracleDataTypeInfoList odti = OracleSchemaInfo.GetDataTypeInfo(oracleConnectionString, sql);

                if (odti != null)
                {
                    for (int i = 0; i < odti.Count; i++)
                    {
                        OracleQueryParameter qp = query.GetPropertyByTypeAndSequence(OracleQueryParameterDirection.Out, i + 1);

                        if (qp != null)
                        {
                            qp.DbDatatype = odti[i].DataType;

                            if (qp.DbDatatype == "NUMBER")
                            {
                                qp.Precision = odti[i].Precision;
                                qp.Scale = odti[i].Scale;

                                if (qp.Scale.HasValue)
                                {
                                    if (qp.Precision.HasValue)
                                    {
                                        // Scale cannot have a higher value than the precision.
                                        if (qp.Scale > qp.Precision)
                                            qp.Scale = qp.Precision;

                                        // And scale (decimals) should maximum contain 9 numbers as highest
                                        if (qp.Scale > 9)
                                            qp.Scale = 9;
                                    }
                                    else
                                        qp.Scale = null;
                                }
                            }
                            else
                            {
                                qp.Precision = null;
                                qp.Scale = null;
                            }

                            if (qp.DbDatatype == "VARCHAR2" ||
                                qp.DbDatatype == "CHAR")
                            {
                                qp.Length = odti[i].ColumnSize;
                            }
                            else
                            {
                                qp.Length = null;
                            }
                        }
                    }
                }
            }
        }

        private void GetFromRows(CaptureCollection captures)
        {
            // Loop through the from rows to handle them
            foreach (Capture capture in captures)
            {
                // Parse the fromrow
                Match fromRowMatch = RegexHelper.ParseGetFirstMatch(RegexConstants.SQL_FROMROW, capture.Value);

                if (fromRowMatch != null)
                {
                    // First check if a tablename is found
                    if (!InsertFromTable(fromRowMatch.Groups[RegexConstants.SQL_FROMROW_TABLENAME].Value, fromRowMatch.Groups[RegexConstants.SQL_FROMROW_ALIAS].Value))
                    {
                        // Error, couldn't insert the alias since it probably already
                        // exist in the from statement either as an alias or an existing
                        // table.
                        throw new Exception("There is an error in a FROM row where a table alias is set to a different tablename than before!");
                    }
                }
                else
                {
                    throw new Exception("Couldn't parse this FROM row: \"" + fromRowMatch.Value + "\". Please analyze!");
                }
            }
        }

        private void GetSelectRows(CaptureCollection captures, OracleQuery query)
        {
            int selectRowCount = 1;
            bool existing = false;

            // Loop through the select rows to handle them
            foreach (Capture capture in captures)
            {
                OracleQueryParameter qp = query.GetPropertyByTypeAndSequence(OracleQueryParameterDirection.Out, selectRowCount);

                existing = qp != null;

                if (!existing)
                {
                    qp = new OracleQueryParameter();
                }

                if (!existing)
                {
                    qp.Sequence = selectRowCount;
                    qp.Direction = OracleQueryParameterDirection.Out;
                }

                // Set the text to the full selectrow
                qp.Text = capture.Value;

                // Parse the selectrow
                Match selRowMatch = RegexHelper.ParseGetFirstMatch(RegexConstants.SQL_SELECTROW, capture.Value);

                if (selRowMatch != null)
                {
                    // First check if TABLE.COLUMN was found
                    if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_TABLE].Value != string.Empty &&
                        selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value != string.Empty)
                    {
                        // Check if an alias is given, in that case use that. If not then
                        // use the column.
                        if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value != string.Empty)
                        {
                            qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value, selectRowCount);
                        }
                        else
                        {
                            qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value, selectRowCount);
                        }

                        // Check if we can find out the real name of the table. Either it is a table or an alias
                        // in the from-statement. If the real table can't be found then we can't set the table and column.
                        string foundTableName = GetTableName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_TABLE].Value);

                        if (!string.IsNullOrEmpty(foundTableName))
                        {
                            qp.OutColumnName = selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value;
                            qp.OutTableName = foundTableName;
                        }
                    }
                    // Check if selectrow starts with some kind of function
                    else if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_FUNCTION].Value != string.Empty &&
                             selRowMatch.Groups[RegexConstants.SQL_SELECTROW_FUNCTIONPARAMETERS].Value != string.Empty)
                    {
                        // Check if an alias is given, in that case use that. If not then
                        // we throw an error since we need unique names.
                        if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value != string.Empty)
                        {
                            qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value, selectRowCount);
                        }
                        else
                        {
                            throw new Exception(string.Format("An alias is missing for the selectrow ({0}) containing:\n" +
                                                              "\t{1}\n" +
                                                              "\nAdd an unique alias and try again."
                                                             , selectRowCount.ToString()
                                                             , selRowMatch.Value));
                        }
                    }
                    // Unknown select row that we cannot parse more than getting the alias
                    else
                    {
                        // Check if we found a single column but no table and if we only have
                        // one table as a from table. In that case we know what table and column it is.
                        if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value != string.Empty &&
                            selRowMatch.Groups[RegexConstants.SQL_SELECTROW_TABLE].Value == string.Empty &&
                            fromTables.Count == 1)
                        {
                            // Check if an alias is given, in that case use that. If not then
                            // it has no name so set a NONAME then.
                            if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value != string.Empty)
                            {
                                qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value, selectRowCount);
                            }
                            else if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value != string.Empty)
                            {
                                qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value, selectRowCount);
                            }
                            else
                            {
                                throw new Exception(string.Format("An alias is missing for the selectrow ({0}) containing:\n" +
                                                                  "\t{1}\n" +
                                                                  "\nAdd an unique alias and try again."
                                                                 , selectRowCount.ToString()
                                                                 , selRowMatch.Value));
                            }

                            qp.OutColumnName = selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value;
                            qp.OutTableName = fromTables.Keys.ToList()[0];
                        }
                        // Check if an alias is given, in that case use that. If not then
                        // it has no name so set a NONAME then.
                        else if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value != string.Empty)
                        {
                            qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_ALIAS].Value, selectRowCount);
                        }
                        else if (selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value != string.Empty)
                        {
                            qp.Name = CheckSelectUniqueName(selRowMatch.Groups[RegexConstants.SQL_SELECTROW_COLUMN].Value, selectRowCount);
                        }
                        else
                        {
                            throw new Exception(string.Format("An alias is missing for the selectrow ({0}) containing:\n" +
                                                              "\t{1}\n" +
                                                              "\nAdd an unique alias and try again."
                                                             , selectRowCount.ToString()
                                                             , capture.Value));
                        }
                    }
                }
                else
                {
                    throw new Exception("Couldn't parse this SELECT row: \"" + selRowMatch.Value + "\". Please analyze!");
                }

                if (!existing)
                {
                    // Add property to query
                    query.Parameters.Add(qp);
                }

                // Increase count
                selectRowCount++;
            }
        }

        private void GetWhereRows(CaptureCollection captures, OracleQuery query, bool isSpecialWhereParameters)
        {
            // Loop through the where rows to handle them
            foreach (Capture capture in captures)
            {
                // Check if whererow contains any parameters
                IList<string> parameters = GetParametersInText(capture.Value, isSpecialWhereParameters);

                if (parameters.Count > 0)
                {
                    // If there is only one parameter on a whererow then we try to find out
                    // what type it is.
                    if (parameters.Count == 1)
                    {
                        string regexString = isSpecialWhereParameters ? RegexConstants.SQL_WHEREROWSPECIAL : RegexConstants.SQL_WHEREROW;

                        Match parameterMatch = Regex.Match(capture.Value, regexString);

                        if (parameterMatch.Success &&
                            !string.IsNullOrEmpty(parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value))
                        {
                            // Get the added parameter
                            OracleQueryParameter qp = query.GetPropertyByTypeAndName(OracleQueryParameterDirection.In, parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value);

                            if (qp == null)
                            {
                                throw new Exception("Property with the name \"" + parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value + "\" doesn't exist.");
                            }

                            if (parameterMatch.Groups[RegexConstants.SQL_WHEREROW_TABLE].Value != string.Empty &&
                                parameterMatch.Groups[RegexConstants.SQL_WHEREROW_COLUMN].Value != string.Empty &&
                                parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value != string.Empty &&
                                string.IsNullOrEmpty(qp.OutTableName) &&
                                string.IsNullOrEmpty(qp.OutColumnName))
                            {
                                // Set the text to the full whererow
                                qp.Text = capture.Value;

                                // Check if we can find out the real name of the table. Either it is a table or an alias
                                // in the from-statement. If the real table can't be found then we can't set the table and column.
                                string foundTableName = GetTableName(parameterMatch.Groups[RegexConstants.SQL_WHEREROW_TABLE].Value);

                                if (!string.IsNullOrEmpty(foundTableName))
                                {
                                    qp.OutColumnName = parameterMatch.Groups[RegexConstants.SQL_WHEREROW_COLUMN].Value;
                                    qp.OutTableName = foundTableName;
                                }
                            }
                            // Special case if there is only a column without a given table but there is only one table
                            // in the from statement then we know what table and column it is anyway.
                            else if (parameterMatch.Groups[RegexConstants.SQL_WHEREROW_TABLE].Value == string.Empty &&
                                parameterMatch.Groups[RegexConstants.SQL_WHEREROW_COLUMN].Value != string.Empty &&
                                parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value != string.Empty &&
                                fromTables.Count == 1 &&
                                string.IsNullOrEmpty(qp.OutTableName) &&
                                string.IsNullOrEmpty(qp.OutColumnName))
                            {
                                // Set the text to the full whererow
                                qp.Text = capture.Value;

                                qp.OutColumnName = parameterMatch.Groups[RegexConstants.SQL_WHEREROW_COLUMN].Value;
                                qp.OutTableName = fromTables.Keys.ToList()[0];
                            }
                            else if (parameterMatch.Groups[RegexConstants.SQL_WHEREROW_FUNCTION].Value != string.Empty &&
                                     parameterMatch.Groups[RegexConstants.SQL_WHEREROW_FUNCTIONPARAMETERS].Value != string.Empty &&
                                     parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value != string.Empty)
                            {
                                // Set the text to the full whererow
                                qp.Text = capture.Value;
                            }
                            else
                            {
                                // Set the text to the full whererow
                                qp.Text = capture.Value;
                            }
                        }
                        // Can atleast set the text for the single parameter
                        else
                        {
                            // Get the added parameter
                            OracleQueryParameter qp = query.GetPropertyByTypeAndName(OracleQueryParameterDirection.In, parameters[0]);

                            if (qp == null)
                            {
                                throw new Exception("Property with the name \"" + parameters[0] + "\" doesn't exist.");
                            }

                            // Check if this is a row where a table.column is compared to a function where the parameter
                            // is somewhere within the functions parameters.
                            // Example: ITESTOTMP.WSID = nvl(:WSID_I,ITESTOTMP.WSID)
                            //if (parameterMatch.Groups[RegexConstants.SQL_WHEREROW_FUNCTION].Value != string.Empty &&
                            //    parameterMatch.Groups[RegexConstants.SQL_WHEREROW_FUNCTIONPARAMETERS].Value != string.Empty &&
                            //    parameterMatch.Groups[RegexConstants.SQL_WHEREROW_PARAMETER].Value != string.Empty)


                            // Set the text to the full whererow
                            qp.Text = capture.Value;
                        }
                    }
                    // More than one parameter on the same row.
                    // Check if information is found for every parameter already. In that case
                    // skip it. Otherwise atleast set the text for the parameter.
                    else if (parameters.Count > 1)
                    {
                        foreach (string parameter in parameters)
                        {
                            // Get the parameter
                            OracleQueryParameter qp = query.GetPropertyByTypeAndName(OracleQueryParameterDirection.In, parameter);

                            if (qp == null)
                            {
                                throw new Exception("Property with the name \"" + parameter + "\" doesn't exist.");
                            }

                            // Now check if information is already set.
                            // If not then set the text.
                            if (String.IsNullOrEmpty(qp.OutColumnName) &&
                                String.IsNullOrEmpty(qp.OutTableName) &&
                                String.IsNullOrEmpty(qp.Text))
                            {
                                qp.Text = capture.Value;
                            }
                        }
                    }
                }
            }
        }

        private void SetQueryParameters(string sql, OracleQuery query, bool isSpecialWhereParameters)
        {
            // Check if sql contains any parameters
            IList<string> parameters = GetParametersInText(sql, isSpecialWhereParameters);

            if (parameters.Count > 0)
            {
                int sequence = 1;

                foreach (string parameter in parameters)
                {
                    // Insert the matched parameter
                    OracleQueryParameter qp = new OracleQueryParameter();

                    qp.Sequence = sequence;
                    qp.Direction = OracleQueryParameterDirection.In;

                    // Set the parameter name
                    qp.Name = parameter;

                    // Add property to query
                    query.Parameters.Add(qp);

                    sequence++;
                }
            }
        }

        private bool InsertFromTable(string tableName, string alias)
        {
            bool result = true;

            // Check if there is a tablename defined.
            // If a tablename is not defined, there could be an alias defined anyway, since it could
            // be a subselect or something. But we ignore this here since we can't handle the
            // subselects anyway.
            if (!string.IsNullOrEmpty(tableName))
            {
                // Add the table to the tablelist if it doesn't already exist.
                if (!fromTables.ContainsKey(tableName))
                {
                    fromTables.Add(tableName, alias);
                }

                if (!string.IsNullOrEmpty(alias))
                {
                    // Check if the alias exist as an alias already.
                    if (!fromAliases.ContainsKey(alias))
                    {
                        fromAliases.Add(alias, tableName);
                    }
                    else
                    {
                        // If the alias exist already it has to match the same tablename as it has before.
                        if (fromAliases[alias].ToUpper() != tableName.ToUpper())
                            result = false;
                    }
                }
            }

            return result;
        }

        // Function for retrieving a tablename from either a alias for a table or the tablename itself.
        private string GetTableName(string name)
        {
            // Check if the name is an alias.
            if (fromAliases.ContainsKey(name))
            {
                string tableName;
                fromAliases.TryGetValue(name, out tableName);
                return tableName;
            }
            // Check if the name is a real table.
            else if (fromTables.ContainsKey(name))
            {
                return name;
            }
            else
            {
                // Since the name can't be found either as an alias or a real tablename then
                // this is not a valid table. This can be because the table consist of a subselect.
                // Return an empty string.
                return string.Empty;
            }
        }

        private string CheckSelectUniqueName(string name, int sequence)
        {
            if (selectNameUnique.ContainsKey(name))
            {
                throw new Exception(string.Format("The name of each column in the select must have a unique tablename and column or alias!\n" +
                                                  "Check the selectrow with the name {0} on select row number {1}.\n" +
                                                  "\nIf you are using a union/intersect/minus then please also check that you don't have\n" +
                                                  "parenthesises around each seperate select statement, because then they can't be treated correctly.", name, sequence.ToString()));
            }
            else
            {
                selectNameUnique.Add(name, sequence);
            }

            return name;
        }

        public static IList<string> GetParametersInQuery(string sql)
        {
            return GetParametersInText(sql, true);
        }

        private static IList<string> GetParametersInText(string text, bool isSpecialWhereParameters)
        {
            List<string> parameters = new List<string>();
            List<string> uniqueParameters = new List<string>();

            string regexString = isSpecialWhereParameters ? RegexConstants.SQL_PARAMETERSPECIAL_FIND : RegexConstants.SQL_PARAMETER_FIND;

            Match m = RegexHelper.ParseGetFirstMatch(regexString, text);

            if (m.Success)
            {
                foreach (Capture capture in m.Groups[RegexConstants.SQL_PARAMETER_PARAMETER].Captures)
                {
                    parameters.Add(capture.Value);
                }

                uniqueParameters.AddRange(parameters.Distinct());
            }

            return uniqueParameters;
        }

    }
}
