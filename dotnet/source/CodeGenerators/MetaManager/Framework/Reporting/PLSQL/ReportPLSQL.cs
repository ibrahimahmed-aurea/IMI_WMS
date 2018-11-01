using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using Cdc.Framework.Parsing.OracleSQLParsing;
using System.Xml;
using System.Text.RegularExpressions;

namespace Cdc.Framework.Reporting.PLSQL
{
    public class ReportPLSQL
    {
        // Regex for finding c comments
        private const string RE_STRIP_C_COMMENTS = @"(" +
                                                     @"/\*" +
                                                     @"(" +
                                                       @"[^*]" +
                                                       @"|" +
                                                       @"[\r\n]" +
                                                       @"|" +
                                                       @"(" +
                                                         @"\*+" +
                                                         @"(" +
                                                           @"[^*/]" +
                                                           @"|" +
                                                           @"[\r\n]" +
                                                         @")" +
                                                       @")" +
                                                     @")*" +
                                                     @"\*+/" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                     @"//.*" +
                                                   @")";

        private string ConnectionString { get; set; }

        public ReportPLSQL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string GetXmlHashSHA1(string packageName)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand(string.Format("{0}.GetXmlHashSHA1", packageName).ToUpper(), conn);

                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter paramRet = new OracleParameter();
                paramRet.Direction = System.Data.ParameterDirection.ReturnValue;
                paramRet.DbType = DbType.String;
                paramRet.Size = 30;
                cmd.Parameters.Add(paramRet);

                string returnVal = string.Empty;

                bool doInvalidRun = false;
                int numberOfRuns = 0;

                do
                {
                    try
                    {
                        // Execute function
                        cmd.ExecuteNonQuery();

                        // Return result from function call
                        returnVal = paramRet.Value.ToString();
                    }
                    catch (OracleException ex)
                    {
                        doInvalidRun = false;

                        if (Regex.Match(ex.Message, @"PLS-00905:\sobject\s\w+\.\w+\sis\sinvalid").Success)
                            doInvalidRun = true;
                    }
                    catch 
                    {
                        doInvalidRun = false;
                    }

                    numberOfRuns++;
                }
                while (doInvalidRun && numberOfRuns <= 1);

                conn.Close();

                return returnVal;
            }
        }

        private string RemoveComments(string codeString)
        {
            return Regex.Replace(codeString, RE_STRIP_C_COMMENTS, string.Empty);
        }

        public bool Execute(string commandString)
        {
            bool retVal = true;

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();

                // Remove all c comments
                string fixedStr = RemoveComments(commandString);

                // Change all CRLF to only LF
                fixedStr = commandString.Replace("\r\n", "\n");

                // Remove trailing CRLF and also slash (/)
                fixedStr = fixedStr.TrimEnd(new char[] { '/', '\n', '\r' });

                OracleCommand cmd = new OracleCommand(fixedStr, conn);
                cmd.CommandType = CommandType.Text;

                try
                {
                    // Execute function
                    cmd.ExecuteNonQuery();
                }
                catch 
                {
                    retVal = false;
                }

                conn.Close();
            }
            
            return retVal;
        }

        public bool PackageExists(string packageName)
        {
            bool exists = false;

            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlText = "select  count(*) " +
                                     "  from  user_objects" +
                                     "  where object_name = :packageName" +
                                     "  and   object_type = 'PACKAGE'";

                    OracleCommand cmd = new OracleCommand(sqlText, conn);

                    // Add the package name as parameter
                    OracleParameter param = new OracleParameter("packageName", OracleDbType.Varchar2, 100, packageName.ToUpper(), ParameterDirection.Input);
                    cmd.Parameters.Add(param);

                    object value = cmd.ExecuteScalar();

                    if (value != DBNull.Value && 
                        Convert.ToInt32(value) > 0)
                    {
                        exists = true;
                    }

                    conn.Close();
                }
            }
            catch { }

            return exists;
        }

        public IList<StoredProcedureParameter> GetXmlInParameterList(string packageName)
        {
            List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();

            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlText = string.Format("select  ARGUMENT_NAME" +
                                                   "       ,DATA_TYPE" +
                                                   "       ,IN_OUT" +
                                                   "       ,DATA_LENGTH" +
                                                   "       ,DATA_PRECISION" +
                                                   "       ,DATA_SCALE" +
                                                   "  from  USER_ARGUMENTS" +
                                                   "  where OBJECT_NAME = 'GETXML'" +
                                                   "  and   PACKAGE_NAME = '{0}'" +
                                                   "  and   DATA_LEVEL = 0" +
                                                   "  and   IN_OUT = 'IN'" +
                                                   "  order by SEQUENCE",
                                                   packageName.ToUpper());

                    OracleCommand cmd = new OracleCommand(sqlText, conn);

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoredProcedureParameter param = new StoredProcedureParameter();

                            param.Name = reader["ARGUMENT_NAME"].ToString();
                            param.DataType = GetStoredProcedureDataType(reader["DATA_TYPE"].ToString());

                            param.OracleDbType = OracleSchemaInfo.GetOracleDbType(reader["DATA_TYPE"].ToString());

                            if (reader["DATA_LENGTH"] != DBNull.Value)
                                param.Length = int.Parse(reader["DATA_LENGTH"].ToString());
                            else
                                param.Length = 0;

                            parameters.Add(param);
                        }
                    }

                    conn.Close();
                }
            }
            catch (OracleException)
            {
                parameters.Clear();
            }
            catch (Exception)
            {
                parameters.Clear();
            }

            return parameters;
        }

        public XmlDocument GetXml(string packageName, IList<StoredProcedureParameter> parameters)
        {
            XmlDocument xmlDocument = null;

            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand(string.Format("{0}.GetXml", packageName), conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Set return value as xml
                    OracleParameter paramRet = new OracleParameter("RETVAL", OracleDbType.XmlType);
                    paramRet.Direction = System.Data.ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(paramRet);

                    // Set all inparameters
                    foreach (StoredProcedureParameter param in parameters)
                    {
                        cmd.Parameters.Add(param.Name, param.OracleDbType.Value, param.Length == 0 ? 4000 : param.Length, param.IsNull ? DBNull.Value : param.Value, ParameterDirection.Input);
                    }

                    try
                    {
                        cmd.ExecuteNonQuery();

                        if (paramRet.Value != DBNull.Value)
                        {
                            xmlDocument = ((Oracle.DataAccess.Types.OracleXmlType)paramRet.Value).GetXmlDocument();
                        }
                    }
                    catch 
                    {
                        xmlDocument = null;
                    }

                    conn.Close();
                }
            }
            catch (OracleException) { }
            catch (Exception) { }

            return xmlDocument;
        }


        private StoredProcedureDataType GetStoredProcedureDataType(string dataType)
        {
            if (dataType.ToLower() == "date")
            {
                return StoredProcedureDataType.DateTime;
            }
            else if (dataType.ToLower() == "number")
            {
                return StoredProcedureDataType.Numeric;
            }
            else if (dataType.ToLower() == "char")
            {
                return StoredProcedureDataType.String;
            }
            else if (dataType.ToLower() == "varchar2")
            {
                return StoredProcedureDataType.String;
            }

            return StoredProcedureDataType.NotSupported;
        }

    }
}
