using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Cdc.Framework.Parsing.PLSQLPackageSpecification
{
    public enum FunctionReturnType { DataType, TableColumn };

    public enum FunctionReturnDBDataType { Invalid, Varchar2, Varchar, Date, Number, Boolean };

    public enum ProcedureStatus { Valid, ParseError, NotSupported };

    public class PLSQLProcedure
    {
        public struct FunctionReturnTypeDataStruct
        {
            public string Param1;
            public string Param2;
            public FunctionReturnDBDataType DataType;
        }
        
        public PLSQLProcedure()
        {
            parameters = new PLSQLProcedureParameterCollection();

            Name = string.Empty;
            IsFunction = false;
            IsContainingRefCursor = false;
            Status = ProcedureStatus.Valid;
            ErrorText = string.Empty;
            FunctionReturnTypeData = new FunctionReturnTypeDataStruct();
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
        public FunctionReturnDBDataType FunctionReturnTypeDataType_DataType
        {
            get { return FunctionReturnTypeData.DataType; }
            set { FunctionReturnTypeData.DataType = value; }
        }

        /// <summary>
        ///     Parameters in the procedure/function
        /// </summary>
        private PLSQLProcedureParameterCollection parameters;

        /// <summary>
        ///     The name of the procedure/function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Status of the procedure.
        /// </summary>
        public ProcedureStatus Status { get; set; }

        /// <summary>
        ///     Text that explains what went wrong when parsing the procedure.
        /// </summary>
        public string ErrorText { get; private set; }

        /// <summary>
        ///     Procedure for setting the errortext.
        /// </summary>
        /// <param name="errorText">The errortext that can include {0} to represent the errorvalue.</param>
        /// <param name="errorFieldValue">The field value that is the cause of the error.</param>
        public void SetErrorText(string errorText, string errorFieldValue)
        {
            ErrorText = string.Format(errorText, errorFieldValue);
        }

        /// <summary>
        ///     If this is a function or not.
        /// </summary>
        public bool IsFunction { get; set; }

        /// <summary>
        ///     If there are any parameters that contains a  ref cursor.
        /// </summary>
        public bool IsContainingRefCursor { get; set; }

        /// <summary>
        ///     The return datatype of the function.
        /// </summary>
        public FunctionReturnType FunctionReturnType { get; set; }

        /// <summary>
        ///     The return data of the function
        /// </summary>
        public FunctionReturnTypeDataStruct FunctionReturnTypeData;

        /// <summary>
        ///     Parameter property. Only possible to get this, not set.
        /// </summary>
        public PLSQLProcedureParameterCollection Parameters
        {
            get { return parameters; }
        }

        public static string RecreateProcedure(PLSQLProcedure procedure)
        {
            string procString = string.Empty;

            if (procedure.Status == ProcedureStatus.Valid)
            {
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

                        PLSQLProcedureParameter param = procedure.Parameters[i];

                        len = param.Name.Length + 1;

                        if (param.IsMandatory)
                        {
                            len += 15 + 1; // "/* Mandatory */ "
                        }

                        if (param.Direction == ParameterDirection.InOut)
                        {
                            len += 6 + 1; // "in out "
                        }
                        else if (param.Direction == ParameterDirection.Out)
                        {
                            len += 3 + 1; // "out "
                        }

                        paraNameLen = Math.Max(paraNameLen, len);
                    }

                    for (int i = 0; i < procedure.Parameters.Count; i++)
                    {
                        PLSQLProcedureParameter param = procedure.Parameters[i];

                        // Pad to the right the amount of spaces as the tabLength and add a comma.
                        if (i != 0)
                        {
                            procString += "".PadLeft(tabLength, ' ') + ",";
                        }

                        string tmpString = string.Empty;

                        tmpString += param.IsMandatory ? "/* Mandatory */ " : string.Empty;

                        switch (param.Direction)
                        {
                            case ParameterDirection.InOut:
                                tmpString += "in out ";
                                break;
                            case ParameterDirection.Out:
                                tmpString += "out ";
                                break;
                        }

                        tmpString = param.Name + "".PadLeft(paraNameLen - tmpString.Length - param.Name.Length, ' ') + tmpString;

                        procString += tmpString;

                        switch (param.ParameterType)
                        {
                            case ParameterType.DataType:
                                switch (param.ParameterTypeDataType_DataType)
                                {
                                    case ParameterDBDataType.Boolean:
                                        procString += "boolean";
                                        break;
                                    case ParameterDBDataType.Date:
                                        procString += "date";
                                        break;
                                    case ParameterDBDataType.Number:
                                        procString += "number";
                                        break;
                                    case ParameterDBDataType.Varchar:
                                        procString += "varchar";
                                        break;
                                    case ParameterDBDataType.Varchar2:
                                        procString += "varchar2";
                                        break;
                                    case ParameterDBDataType.Ref_Cur:
                                        procString += "ref_cur";
                                        break;
                                    default:
                                        procString += "<NotDefined>";
                                        break;
                                }
                                break;

                            case ParameterType.TableColumn:
                                procString += param.ParameterTypeTableColumn_Table + "." + param.ParameterTypeTableColumn_Column + "%type";
                                break;
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
                        case FunctionReturnType.DataType:
                            switch (procedure.FunctionReturnTypeDataType_DataType)
                            {
                                case FunctionReturnDBDataType.Boolean:
                                    procString += "boolean";
                                    break;
                                case FunctionReturnDBDataType.Number:
                                    procString += "number";
                                    break;
                                case FunctionReturnDBDataType.Varchar2:
                                    procString += "varchar2";
                                    break;
                                case FunctionReturnDBDataType.Varchar:
                                    procString += "varchar";
                                    break;
                                case FunctionReturnDBDataType.Date:
                                    procString += "date";
                                    break;
                                default:
                                    procString += "<NotDefined>";
                                    break;
                            }
                            break;

                        case FunctionReturnType.TableColumn:
                            procString += procedure.FunctionReturnTypeTableColumn_Table + "." + procedure.FunctionReturnTypeTableColumn_Column + "%type";
                            break;
                    }

                    procString += ";";
                }
                else
                {
                    procString += ";";
                }
            }

            return procString;
        }

        public override string ToString()
        {
            string procString = string.Empty;

            // Check if the procedure has errors.
            // If it has errors then write as much as possible to be able to know how much was parsed.
            // If there was no error then try to recreate the procedure.

            procString += "Procedure: " + Name + Environment.NewLine;
            procString += "Type:      ";
            procString += IsFunction ? "Function" : "Procedure";
            procString += Environment.NewLine;

            if (Status != ProcedureStatus.Valid)
            {
                if (Status == ProcedureStatus.ParseError)
                {
                    procString += "ERRORTYPE: Parsing Error" + Environment.NewLine;
                }
                else if (Status == ProcedureStatus.NotSupported)
                {
                    procString += "ERRORTYPE: Not Supported" + Environment.NewLine;
                }

                procString += "ERROR:     " + ErrorText + Environment.NewLine;
            }
            else
            {
                procString += "Recreation:" + Environment.NewLine;
                procString += RecreateProcedure(this);
            }
            procString += Environment.NewLine;

            return procString;
        }

        /// <summary>
        ///     Function for retreiving the different datatypes for function return datatypes as a string.
        /// </summary>
        /// <param name="dataType">The databasetype to get as string.</param>
        /// <returns>Returns the databasetype given as a string.</returns>
        public static string GetFunctionReturnDBTypeAsString(FunctionReturnDBDataType dataType)
        {
            switch (dataType)
            {
                case FunctionReturnDBDataType.Boolean: return "BOOLEAN";
                case FunctionReturnDBDataType.Date: return "DATE";
                case FunctionReturnDBDataType.Number: return "NUMBER";
                case FunctionReturnDBDataType.Varchar: return "VARCHAR";
                case FunctionReturnDBDataType.Varchar2: return "VARCHAR2";
                default:
                case FunctionReturnDBDataType.Invalid: return "<INVALID DATATYPE>";
            }
        }

        

    }
}
