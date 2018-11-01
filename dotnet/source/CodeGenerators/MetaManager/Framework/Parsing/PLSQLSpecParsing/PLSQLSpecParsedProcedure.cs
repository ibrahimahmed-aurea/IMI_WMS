using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    public enum ParseFunctionReturnType { DataType, TableColumn };

    public enum ParseFunctionReturnDBDataType { Invalid, Varchar2, Varchar, Date, Number, Boolean };

    public class PLSQLSpecParsedProcedure
    {
        public struct ParseFunctionReturnTypeDataStruct
        {
            public string Param1;
            public string Param2;
            public ParseFunctionReturnDBDataType DataType;
        }

        /// <summary>
        ///     Returns the value of the table for the TableColumn return datatype.
        /// </summary>
        public string FunctionReturnTypeTableColumn_Table
        {
            get { return FunctionReturnTypeData.Param1; }
            set { FunctionReturnTypeData.Param1 = value; }
        }

        /// <summary>
        ///     Returns the value of the column for the TableColumn return datatype.
        /// </summary>
        public string FunctionReturnTypeTableColumn_Column
        {
            get { return FunctionReturnTypeData.Param2; }
            set { FunctionReturnTypeData.Param2 = value; }
        }

        /// <summary>
        ///     Returns the value of the datatype for the DataType return datatype.
        /// </summary>
        public ParseFunctionReturnDBDataType FunctionReturnTypeDataType_DataType
        {
            get { return FunctionReturnTypeData.DataType; }
        }

        public PLSQLSpecParsedProcedure()
        {
            parameters = new PLSQLSpecParsedProcedureParameterCollection();

            Name = string.Empty;
            IsFunction = false;
            ParameterString = string.Empty;
            FunctionReturnTypeData = new ParseFunctionReturnTypeDataStruct();
        }

        /// <summary>
        ///     Parameters in the procedure/function
        /// </summary>
        private PLSQLSpecParsedProcedureParameterCollection parameters;

        /// <summary>
        ///     The name of the procedure/function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     If procedure has any parseerrors.
        /// </summary>
        public bool HasParseError { get; set; }

        /// <summary>
        ///     Text that explains what went wrong when parsing the procedure.
        /// </summary>
        public string ParseErrorText { get; private set; }

        /// <summary>
        ///     Procedure for setting the errortext.
        /// </summary>
        /// <param name="errorText">The errortext that can include {0} to represent the errorvalue.</param>
        /// <param name="errorFieldValue">The field value that is the cause of the error.</param>
        public void SetParseErrorText(string errorText, string errorFieldValue)
        {
            ParseErrorText = string.Format(errorText, errorFieldValue);
        }

        /// <summary>
        ///     If this is a function or not.
        /// </summary>
        public bool IsFunction { get; set; }

        /// <summary>
        ///     The return datatype of the function.
        /// </summary>
        public ParseFunctionReturnType FunctionReturnType { get; set; }

        /// <summary>
        ///     The return data of the function
        /// </summary>
        public ParseFunctionReturnTypeDataStruct FunctionReturnTypeData;

        /// <summary>
        ///     The complete string of the parameters parsed from the file.
        /// </summary>
        public string ParameterString { get; set; }

        /// <summary>
        ///     Parameter property. Only possible to get this, not set.
        /// </summary>
        public PLSQLSpecParsedProcedureParameterCollection Parameters
        {
            get { return parameters; }
        }

        public static string RecreateProcedure(PLSQLSpecParsedProcedure procedure)
        {
            string procString = string.Empty;

            procString += procedure.IsFunction ? "function " : "procedure ";

            procString += procedure.Name;

            if (procedure.Parameters.Count > 0)
            {
                // The current length of the procString is how many spaces
                // that should be added for each new parameter.
                int tabLength = procString.Length;

                procString += "(";

                // First check length in total
                int paraNameLen = 0;
                for (int i = 0; i < procedure.Parameters.Count; i++)
                {
                    int len;

                    PLSQLSpecParsedProcedureParameter param = procedure.Parameters[i];

                    len = param.Name.Length + 1;

                    if (param.IsMandatory)
                    {
                        len += 15 + 1; // "/* Mandatory */ "
                    }

                    if (param.Direction == ParseParameterDirection.InOut)
                    {
                        len += 6 + 1; // "in out "
                    }
                    else if (param.Direction == ParseParameterDirection.Out)
                    {
                        len += 3 + 1; // "out "
                    }

                    paraNameLen = Math.Max(paraNameLen, len);
                }

                for (int i = 0; i < procedure.Parameters.Count; i++)
                {
                    PLSQLSpecParsedProcedureParameter param = procedure.Parameters[i];

                    // Pad to the right the amount of spaces as the tabLength and add a comma.
                    if (i != 0)
                    {
                        procString += "".PadLeft(tabLength, ' ') + ",";
                    }

                    string tmpString = string.Empty;

                    tmpString += param.IsMandatory ? "/* Mandatory */ " : string.Empty;

                    switch (param.Direction)
                    {
                        case ParseParameterDirection.InOut:
                            tmpString += "in out ";
                            break;
                        case ParseParameterDirection.Out:
                            tmpString += "out ";
                            break;
                    }

                    tmpString = param.Name + "".PadLeft(paraNameLen - tmpString.Length - param.Name.Length, ' ') + tmpString;

                    procString += tmpString;

                    switch (param.ParameterType)
                    {
                        case ParseParameterType.DataType:
                            procString += PLSQLSpecParser.GetParameterDataTypeAsString(param.ParameterTypeData.DataType);
                            break;
                        case ParseParameterType.TableColumn:
                            procString += param.ParameterTypeData.Param1 + "." + param.ParameterTypeData.Param2 + "%type";
                            break;
                        case ParseParameterType.TableRow:
                            procString += param.ParameterTypeData.Param1 + "%rowtype";
                            break;
                        case ParseParameterType.PackageDataType:
                            procString += param.ParameterTypeData.Param1 + "." + param.ParameterTypeData.Param2;
                            break;
                    }

                    if (param.DefaultValue != string.Empty)
                    {
                        procString += " := " + param.DefaultValue;
                    }

                    if (i != (procedure.Parameters.Count - 1))
                    {
                        procString += Environment.NewLine;
                    }
                }

                procString += ")";
            }

            if (procedure.IsFunction)
            {
                procString += Environment.NewLine + "    return ";

                switch (procedure.FunctionReturnType)
                {
                    case ParseFunctionReturnType.DataType:
                        procString += PLSQLSpecParser.GetFunctionReturnDataTypeAsString(procedure.FunctionReturnTypeData.DataType);
                        break;
                    case ParseFunctionReturnType.TableColumn:
                        procString += procedure.FunctionReturnTypeData.Param1 + "." + procedure.FunctionReturnTypeData.Param2 + "%type";
                        break;
                    default:
                        procString += "<Unknown Datatype>";
                        break;
                }

                procString += ";";
            }
            else
            {
                procString += ";";
            }

            return procString;
        }

        public override string ToString()
        {
            string procString = string.Empty;

            // Check if the procedure has errors.
            // If it has errors then write as much as possible to be able to know how much was parsed.
            // Always try to recreate the procedure/function.

            procString += "Procedure: " + Name + Environment.NewLine;
            procString += "Type:      ";
            procString += IsFunction ? "Function" : "Procedure";
            procString += Environment.NewLine;

            if (HasParseError)
            {
                procString += "ERROR:     " + ParseErrorText + Environment.NewLine;
            }

            procString += "Recreation:" + Environment.NewLine;
            procString += RecreateProcedure(this);

            procString += Environment.NewLine;

            return procString;
        }

    }
}
