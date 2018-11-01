using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public class OracleDataTypeInfo
    {
        public string ColumnName { get; set; }

        public OracleDbType? ProviderType { get; set; }

        public string DataType { get; set; }

        public int? ColumnSize { get; set; }

        public bool? IsByteSematic { get; set; }

        public short? Precision { get; set; }

        public short? Scale { get; set; }

        public System.Data.ParameterDirection Direction { get; set; }
    }

    public class OracleDataTypeInfoList : List<OracleDataTypeInfo> { }

    public enum RefCurStoredProcedureStatus
    {
        Valid,
        OracleError,
        OtherError,
        ParameterDataTypeNotSupported,
        NoRefCursorMetadataFound,
        NoRefCursorTestSectionDefined
    }

    public class RefCurStoredProcedure
    {
        public static RefCurStoredProcedure Create(string storedProcedureName, string packageName, string connectionString)
        {
            RefCurStoredProcedure storedProc = new RefCurStoredProcedure();

            storedProc.Name = storedProcedureName;
            storedProc.PackageName = packageName;

            // Try to call database to see if we can catch the parameters
            OracleSchemaInfo.GetRefCursorTypes(connectionString, storedProc);

            return storedProc;
        }

        private OracleDataTypeInfoList initialParameterList = null;

        public OracleDataTypeInfoList InitialParameterList
        {
            get
            {
                if (initialParameterList == null)
                {
                    initialParameterList = new OracleDataTypeInfoList();
                }

                return initialParameterList;
            }
        }

        private OracleDataTypeInfoList parameterList = null;

        public OracleDataTypeInfoList ParameterList
        {
            get
            {
                if (parameterList == null)
                {
                    parameterList = new OracleDataTypeInfoList();
                }

                return parameterList;
            }
        }

        public string Name { get; set; }

        public string PackageName { get; set; }

        public string PackageAndProcName
        {
            get
            {
                return string.Format("{0}.{1}", PackageName, Name).ToUpper();
            }
        }

        public bool IsFunction { get; set; }

        public RefCurStoredProcedureStatus Status { get; set; }

        public string ErrorText { get; set; }
    }

    public static class OracleSchemaInfo
    {
        public static OracleDataTypeInfoList GetDataTypeInfo(string connectionString, string sqlStatement)
        {
            OracleDataTypeInfoList dataTypeList = null;

            if (!String.IsNullOrEmpty(connectionString) && !String.IsNullOrEmpty(sqlStatement))
            {
                try
                {
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        conn.Open();

                        OracleCommand cmd = new OracleCommand(sqlStatement, conn);

                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
                        {
                            // Read the SchemaTable for the datareader
                            DataTable dt = reader.GetSchemaTable();

                            // Create the list
                            dataTypeList = new OracleDataTypeInfoList();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                OracleDataTypeInfo dti = new OracleDataTypeInfo();

                                int ordinal;

                                // ProviderType
                                ordinal = dt.Columns["ProviderType"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ProviderType = null;
                                else
                                    dti.ProviderType = ((OracleDbType)dt.Rows[i].ItemArray[ordinal]);

                                // Datatype
                                dti.DataType = ConvertProviderType(dti.ProviderType);

                                // ColumnName
                                ordinal = dt.Columns["ColumnName"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ColumnName = String.Empty;
                                else
                                    dti.ColumnName = (string)dt.Rows[i].ItemArray[ordinal];

                                // ColumnSize
                                ordinal = dt.Columns["ColumnSize"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ColumnSize = null;
                                else
                                    dti.ColumnSize = (int)dt.Rows[i].ItemArray[ordinal];

                                // IsByteSemantic
                                ordinal = dt.Columns["IsByteSemantic"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.IsByteSematic = null;
                                else
                                    dti.IsByteSematic = (bool)dt.Rows[i].ItemArray[ordinal];

                                // NumericPrecision
                                ordinal = dt.Columns["NumericPrecision"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.Precision = null;
                                else
                                    dti.Precision = (short)dt.Rows[i].ItemArray[ordinal];

                                // NumericScale
                                ordinal = dt.Columns["NumericScale"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.Scale = null;
                                else
                                    dti.Scale = (short)dt.Rows[i].ItemArray[ordinal];

                                // Add to list
                                dataTypeList.Add(dti);
                            }
                        }
                        conn.Close();
                    }
                }
                catch (OracleException e)
                {
                    if (e.Number == 1008)
                    {
                        throw new Exception(string.Format("OracleError when trying to find datatypes.\n" +
                                            "If SQL looks ok then check all parenthesis and see if you can\n" +
                                            "remove any to get the same result.\n\n" +
                                            "This error has occured with a UNION that was working in Toad but failed here:\n" +
                                            "\t( select ... ) union ( select ... ) order by ...\n\n" +
                                            "It worked when the parenthesises was removed:\n" +
                                            "\tselect ... union select ... order by ...\n" +
                                            "\nOriginal Error:\n" +
                                            "\t{0}", e.Message), e);
                    }
                    else
                        throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return dataTypeList;
        }

        private static string ConvertProviderType(OracleDbType? providerType)
        {
            // Interpreted datatype
            switch (providerType)
            {
                case OracleDbType.Byte: return "NUMBER";
                case OracleDbType.Int16: return "NUMBER";
                case OracleDbType.Int32: return "NUMBER";
                case OracleDbType.Int64: return "NUMBER";
                case OracleDbType.Single: return "NUMBER";
                case OracleDbType.Double: return "NUMBER";
                case OracleDbType.Decimal: return "NUMBER";
                case OracleDbType.BFile: return "BFILE";
                case OracleDbType.BinaryDouble: return "BINARY_DOUBLE";
                case OracleDbType.BinaryFloat: return "BINARY_FLOAT";
                case OracleDbType.Blob: return "BLOB";
                case OracleDbType.Char: return "CHAR";
                case OracleDbType.Clob: return "CLOB";
                case OracleDbType.Date: return "DATE";
                case OracleDbType.IntervalDS: return "INTERVAL DAY TO SECOND";
                case OracleDbType.IntervalYM: return "INTERVAL YEAR TO MONTH";
                case OracleDbType.Long: return "LONG";
                case OracleDbType.LongRaw: return "LONG RAW";
                case OracleDbType.NChar: return "NCHAR";
                case OracleDbType.NClob: return "NCLOB";
                case OracleDbType.NVarchar2: return "NVARCHAR2";
                case OracleDbType.Raw: return "RAW";
                case OracleDbType.RefCursor: return "REF CURSOR";
                case OracleDbType.TimeStamp: return "TIMESTAMP";
                case OracleDbType.TimeStampLTZ: return "TIMESTAMP WITH LOCAL TIME ZONE";
                case OracleDbType.TimeStampTZ: return "TIMESTAMP WITH TIME ZONE";
                case OracleDbType.Varchar2: return "VARCHAR2";
                case OracleDbType.XmlType: return "XMLType";
                default: return String.Empty;
            }
        }

        public static OracleDbType? GetOracleDbType(string dataType)
        {
            if (dataType.ToLower() == "clob")
            {
                return OracleDbType.Clob;
            }
            else if (dataType.ToLower() == "binary_integer")
            {
                return OracleDbType.Int32;
            }
            else if (dataType.ToLower() == "date")
            {
                return OracleDbType.Date;
            }
            else if (dataType.ToLower() == "raw")
            {
                return OracleDbType.Raw;
            }
            else if (dataType.ToLower() == "number")
            {
                return OracleDbType.Decimal;
            }
            else if (dataType.ToLower() == "ref cursor")
            {
                return OracleDbType.RefCursor;
            }
            else if (dataType.ToLower() == "rowid")
            {
                return OracleDbType.Varchar2; // Seems to work.
            }
            else if (dataType.ToLower() == "char")
            {
                return OracleDbType.Char;
            }
            else if (dataType.ToLower() == "varchar2")
            {
                return OracleDbType.Varchar2;
            }

            return null;
        }

        public static System.Data.ParameterDirection GetDirection(string directionString)
        {
            if (directionString.ToLower() == "in")
            {
                return System.Data.ParameterDirection.Input;
            }
            else if (directionString.ToLower() == "out")
            {
                return System.Data.ParameterDirection.Output;
            }
            else
            {
                return System.Data.ParameterDirection.InputOutput;
            }
        }

        private static void FetchInitialStoredProcedureParameterList(string connectionString, RefCurStoredProcedure procedure)
        {
            if (!String.IsNullOrEmpty(connectionString) && procedure != null)
            {
                // Clear list
                procedure.InitialParameterList.Clear();

                if (procedure.Status == RefCurStoredProcedureStatus.Valid)
                {
                    try
                    {
                        using (OracleConnection conn = new OracleConnection(connectionString))
                        {
                            conn.Open();

                            string sqlText = string.Format("select  SEQUENCE" +
                                                           "       ,ARGUMENT_NAME" +
                                                           "       ,DATA_TYPE" +
                                                           "       ,IN_OUT" +
                                                           "       ,DATA_LENGTH" +
                                                           "       ,DATA_PRECISION" +
                                                           "       ,DATA_SCALE" +
                                                           "  from  USER_ARGUMENTS" +
                                                           "  where OBJECT_NAME = '{0}'" +
                                                           "  and   PACKAGE_NAME = '{1}'" +
                                                           "  and   DATA_LEVEL = 0" +
                                                           "  order by SEQUENCE",
                                                           procedure.Name.ToUpper(),
                                                           procedure.PackageName.ToUpper());

                            OracleCommand cmd = new OracleCommand(sqlText, conn);

                            bool firstParam = true;

                            using (OracleDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    OracleDataTypeInfo param = new OracleDataTypeInfo();

                                    param.ColumnName = reader["ARGUMENT_NAME"].ToString();
                                    param.ProviderType = GetOracleDbType(reader["DATA_TYPE"].ToString());

                                    // Check if parameter is not supported (null)
                                    if (param.ProviderType == null)
                                    {
                                        procedure.Status = RefCurStoredProcedureStatus.ParameterDataTypeNotSupported;
                                        procedure.ErrorText = string.Format("The stored procedure contains a parameter with an unsupported data type ({0})!", reader["DATA_TYPE"].ToString());
                                        reader.Close();
                                        break;
                                    }

                                    param.Direction = GetDirection(reader["IN_OUT"].ToString());

                                    if (reader["DATA_LENGTH"] != DBNull.Value)
                                        param.ColumnSize = int.Parse(reader["DATA_LENGTH"].ToString());
                                    else
                                        param.ColumnSize = null;

                                    if (reader["DATA_PRECISION"] != DBNull.Value)
                                        param.Precision = short.Parse(reader["DATA_PRECISION"].ToString());
                                    else
                                        param.Precision = null;

                                    if (reader["DATA_SCALE"] != DBNull.Value)
                                        param.Scale = short.Parse(reader["DATA_SCALE"].ToString());
                                    else
                                        param.Scale = null;

                                    procedure.InitialParameterList.Add(param);

                                    // Check if return value for a function
                                    if (firstParam && string.IsNullOrEmpty(param.ColumnName) && param.Direction == System.Data.ParameterDirection.Output)
                                    {
                                        param.Direction = System.Data.ParameterDirection.ReturnValue;
                                        procedure.IsFunction = true;
                                    }

                                    firstParam = false;
                                }
                            }

                            conn.Close();
                        }
                    }
                    catch (OracleException e)
                    {
                        procedure.Status = RefCurStoredProcedureStatus.OracleError;
                        procedure.ErrorText = e.ToString();
                    }
                    catch (Exception e)
                    {
                        procedure.Status = RefCurStoredProcedureStatus.OtherError;
                        procedure.ErrorText = e.ToString();
                    }
                }
            }
        }

        private static void SetTestRefCursor(OracleConnection conn)
        {
            if (conn != null)
            {
                OracleCommand cmd = new OracleCommand("Global.Set_TestRefCursor", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
            }
        }

        private static bool ResetTestRefCursor(OracleConnection conn)
        {
            if (conn != null)
            {
                OracleCommand cmd = new OracleCommand("Global.Reset_TestRefCursor", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter paramRet = new OracleParameter();
                paramRet.Direction = System.Data.ParameterDirection.ReturnValue;
                paramRet.DbType = DbType.Decimal;
                cmd.Parameters.Add(paramRet);

                // Execute function
                cmd.ExecuteNonQuery();

                // Return result from function call
                return paramRet.Value.ToString() == "1";
            }

            return false;
        }

        public static void GetRefCursorTypes(string connectionString, RefCurStoredProcedure procedure)
        {
            if (!String.IsNullOrEmpty(connectionString) && procedure != null)
            {
                // Fetch the initial stored procedure list first
                FetchInitialStoredProcedureParameterList(connectionString, procedure);

                // Clear the "real" parameterlist
                procedure.ParameterList.Clear();

                try
                {
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        conn.Open();

                        // Set TestRefCursor
                        SetTestRefCursor(conn);

                        OracleCommand cmd = new OracleCommand(procedure.PackageAndProcName, conn);

                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (OracleDataTypeInfo param in procedure.InitialParameterList)
                        {
                            if (param.Direction == System.Data.ParameterDirection.ReturnValue)
                                cmd.Parameters.Add(param.ColumnName, (OracleDbType)param.ProviderType, System.Data.ParameterDirection.ReturnValue);
                            else
                                cmd.Parameters.Add(param.ColumnName, (OracleDbType)param.ProviderType, param.ColumnSize ?? 0, DBNull.Value, param.Direction);
                        }

                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
                        {
                            // Read the SchemaTable for the datareader
                            DataTable dt = reader.GetSchemaTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                OracleDataTypeInfo dti = new OracleDataTypeInfo();

                                int ordinal;

                                // ProviderType
                                ordinal = dt.Columns["ProviderType"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ProviderType = null;
                                else
                                    dti.ProviderType = ((OracleDbType)dt.Rows[i].ItemArray[ordinal]);

                                // Direction
                                dti.Direction = System.Data.ParameterDirection.Output;

                                // Datatype
                                dti.DataType = ConvertProviderType(dti.ProviderType);

                                // ColumnName
                                ordinal = dt.Columns["ColumnName"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ColumnName = String.Empty;
                                else
                                    dti.ColumnName = (string)dt.Rows[i].ItemArray[ordinal];

                                // ColumnSize
                                ordinal = dt.Columns["ColumnSize"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.ColumnSize = null;
                                else
                                    dti.ColumnSize = (int)dt.Rows[i].ItemArray[ordinal];

                                // IsByteSemantic
                                ordinal = dt.Columns["IsByteSemantic"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.IsByteSematic = null;
                                else
                                    dti.IsByteSematic = (bool)dt.Rows[i].ItemArray[ordinal];

                                // NumericPrecision
                                ordinal = dt.Columns["NumericPrecision"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.Precision = null;
                                else
                                    dti.Precision = (short)dt.Rows[i].ItemArray[ordinal];

                                // NumericScale
                                ordinal = dt.Columns["NumericScale"].Ordinal;
                                if (dt.Rows[i].ItemArray[ordinal] == DBNull.Value)
                                    dti.Scale = null;
                                else
                                    dti.Scale = (short)dt.Rows[i].ItemArray[ordinal];

                                // Add to list
                                procedure.ParameterList.Add(dti);
                            }
                        }

                        // Reset TestRefCursor
                        if (!ResetTestRefCursor(conn))
                        {
                            procedure.Status = RefCurStoredProcedureStatus.NoRefCursorTestSectionDefined;
                            procedure.ErrorText = string.Format("The stored procedure is missing a test section for returning the structure of the ref cursor.\n" +
                                                                "Check the instructions for how to define this section.");
                        }
                        // Test if any ref cursor parameters (metadata) was found
                        else if (procedure.ParameterList.Count == 0)
                        {
                            procedure.Status = RefCurStoredProcedureStatus.NoRefCursorMetadataFound;
                            procedure.ErrorText = string.Format("No Metadata could be fetched from the Stored Procedure for the Ref Cursor.\n" +
                                                                "This is probably because the test section of the stored procedure for returning\n" +
                                                                "the structure of the ref cursor doesn't return any ref cursor. Check instructions\n" +
                                                                "for how to define the test section.");
                        }

                        conn.Close();
                    }
                }
                catch (OracleException e)
                {
                    procedure.Status = RefCurStoredProcedureStatus.OracleError;
                    procedure.ErrorText = e.ToString();
                }
                catch (Exception e)
                {
                    procedure.Status = RefCurStoredProcedureStatus.OtherError;
                    procedure.ErrorText = e.ToString();
                }
            }
        }

    }
}
