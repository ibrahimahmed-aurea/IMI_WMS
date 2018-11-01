using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.Parsing.PLSQLSpecParsing;

namespace  Cdc.Framework.Parsing.PLSQLPackageSpecification
{
    public static class PLSQLSupportedSpec
    {
        public static PLSQLSpec ParseString(string stringToParse)
        {
            PLSQLSpecParsed spec = PLSQLSpecParser.ParseString(stringToParse);

            return ValidateParsedSpec(spec);
        }

        public static PLSQLSpec ParseSpecFile(string pathAndFilename)
        {
            PLSQLSpecParsed spec = PLSQLSpecParser.ParseFile(pathAndFilename);

            return ValidateParsedSpec(spec);
        }

        public static PLSQLSpec ValidateParsedSpec(PLSQLSpecParsed parsedSpec)
        {
            PLSQLSpec spec = null;

            if (parsedSpec != null)
            {
                spec = new PLSQLSpec();

                // Get the package name
                spec.PackageName = parsedSpec.PackageName;

                // Get the filename (if it exists)
                spec.FileNameParsed = parsedSpec.FileNameParsed;

                // Get the length of the read package
                spec.PackageLength = parsedSpec.PackageLength;

                // Get the hash of the read package
                spec.PackageHash = parsedSpec.PackageHash;

                // Loop through the parsed procedures.
                foreach (PLSQLSpecParsedProcedure procedure in parsedSpec.Procedures)
                {
                    // Create a new procedure
                    PLSQLProcedure newProcedure = new PLSQLProcedure();

                    // Get name
                    newProcedure.Name = procedure.Name;

                    // Set status and errortext if parse error
                    if (procedure.HasParseError)
                    {
                        newProcedure.Status = ProcedureStatus.ParseError;
                        newProcedure.SetErrorText(procedure.ParseErrorText, string.Empty);
                    }

                    // Get if function
                    if (procedure.IsFunction)
                    {
                        newProcedure.IsFunction = true;

                        switch (procedure.FunctionReturnType)
                        {
                            case ParseFunctionReturnType.DataType:
                                newProcedure.FunctionReturnType = FunctionReturnType.DataType;

                                switch (procedure.FunctionReturnTypeData.DataType)
                                {
                                    case ParseFunctionReturnDBDataType.Boolean:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Boolean;
                                        break;
                                    case ParseFunctionReturnDBDataType.Date:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Date;
                                        break;
                                    case ParseFunctionReturnDBDataType.Number:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Number;
                                        break;
                                    case ParseFunctionReturnDBDataType.Varchar:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Varchar;
                                        break;
                                    case ParseFunctionReturnDBDataType.Varchar2:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Varchar2;
                                        break;
                                    default:
                                        newProcedure.FunctionReturnTypeDataType_DataType = FunctionReturnDBDataType.Invalid;
                                        break;
                                }

                                // Check if we have catched the datatype. If we have an invalid datatype this is not supported.
                                if (newProcedure.Status == ProcedureStatus.Valid &&
                                    newProcedure.FunctionReturnTypeDataType_DataType == FunctionReturnDBDataType.Invalid)
                                {
                                    newProcedure.Status = ProcedureStatus.NotSupported;
                                    newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureFunctionReturnTypeNotSupported, string.Empty);
                                }

                                break;

                            case ParseFunctionReturnType.TableColumn:
                                newProcedure.FunctionReturnType = FunctionReturnType.TableColumn;

                                newProcedure.FunctionReturnTypeTableColumn_Table = procedure.FunctionReturnTypeTableColumn_Table;
                                newProcedure.FunctionReturnTypeTableColumn_Column = procedure.FunctionReturnTypeTableColumn_Column;
                                break;
                        }
                    }
                    else
                    {
                        newProcedure.IsFunction = false;
                    }

                    // Variable for breaking the loop below.
                    bool doBreak = false;

                    // Loop through all parameters
                    foreach (PLSQLSpecParsedProcedureParameter parameter in procedure.Parameters)
                    {
                        // Create the new parameter
                        PLSQLProcedureParameter newParam = new PLSQLProcedureParameter();

                        // Fetch name
                        newParam.Name = parameter.Name;

                        // Fetch if mandatory
                        newParam.IsMandatory = parameter.IsMandatory;

                        // Check parametertype
                        switch (parameter.ParameterType)
                        {
                            case ParseParameterType.DataType:

                                // Set the type
                                newParam.ParameterType = ParameterType.DataType;

                                // What datatypes do we support from the parsed ones.
                                switch (parameter.ParameterTypeData.DataType)
                                {
                                    case ParseParameterDBDataType.Boolean:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Boolean;
                                        break;

                                    case ParseParameterDBDataType.Number:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Number;
                                        break;

                                    case ParseParameterDBDataType.Varchar2:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Varchar2;
                                        break;

                                    case ParseParameterDBDataType.Varchar:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Varchar;
                                        break;

                                    case ParseParameterDBDataType.Date:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Date;
                                        break;

                                    case ParseParameterDBDataType.Ref_Cur:
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Ref_Cur;
                                        newProcedure.IsContainingRefCursor = true;

                                        // Check that this is an out parameter and that there are no more out
                                        // parameters exept ALMID_O and that this parameter is the last one
                                        if (parameter.Direction != ParseParameterDirection.Out ||
                                            procedure.Parameters.Where(param => param.Name != newParam.Name && param.Name != "ALMID_O" && ((param.Direction == ParseParameterDirection.InOut) || (param.Direction == ParseParameterDirection.Out))).Count() > 0)
                                        {
                                            newProcedure.Status = ProcedureStatus.NotSupported;
                                            newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureRefCursorVariantNotSupported, parameter.Name);

                                            // Don't handle more parameters for this procedure.
                                            doBreak = true;
                                            break;
                                        }
                                        else if (procedure.IsFunction)
                                        {
                                            newProcedure.Status = ProcedureStatus.NotSupported;
                                            newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureRefCursorFunctionNotSupported, parameter.Name);

                                            // Don't handle more parameters for this procedure.
                                            doBreak = true;
                                            break;
                                        }
                                        else if (procedure.Parameters.Last().Name != newParam.Name && procedure.Parameters.Last().Name != "ALMID_O" || (procedure.Parameters.Last().Name == "ALMID_O" && procedure.Parameters[procedure.Parameters.Count-2].Name != newParam.Name))
                                        {
                                            newProcedure.Status = ProcedureStatus.NotSupported;
                                            newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureRefCursorParameterOrderNotSupported, parameter.Name);

                                            // Don't handle more parameters for this procedure.
                                            doBreak = true;
                                            break;
                                        }

                                        break;

                                    default:
                                        // If the original datatype is Invalid then there will be
                                        // a parse error anyway so just copy the Invalid type.
                                        newParam.ParameterTypeDataType_DataType = ParameterDBDataType.Invalid;
                                        break;
                                }

                                // Check if we have catched the datatype. If we have an invalid datatype this is not supported.
                                if (newProcedure.Status == ProcedureStatus.Valid &&
                                    newParam.ParameterTypeDataType_DataType == ParameterDBDataType.Invalid)
                                {
                                    newProcedure.Status = ProcedureStatus.NotSupported;
                                    newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureParameterTypeNotSupported, parameter.Name);
                                }
                                break;

                            case ParseParameterType.TableColumn:
                                // Set the parameter type
                                newParam.ParameterType = ParameterType.TableColumn;

                                newParam.ParameterTypeTableColumn_Table = parameter.ParseParameterTypeTableColumn_Table;
                                newParam.ParameterTypeTableColumn_Column = parameter.ParseParameterTypeTableColumn_Column;
                                break;

                            case ParseParameterType.TableRow:
                                // Parameters with types like TABLENAME%rowtype are not supported.
                                newProcedure.Status = ProcedureStatus.NotSupported;
                                newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureParameterTypeTablerowNotSupported, parameter.Name);

                                // Set the parameter type
                                newParam.ParameterType = ParameterType.NotSupported;

                                // Don't handle more parameters for this procedure.
                                doBreak = true;
                                break;

                            case ParseParameterType.PackageDataType:
                                // Parameters with types like PACKAGE.PACKAGEDATATYPE are not supported.
                                newProcedure.Status = ProcedureStatus.NotSupported;
                                newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureParameterTypePackageDataTypeNotSupported, parameter.Name);

                                // Set the parameter type
                                newParam.ParameterType = ParameterType.NotSupported;

                                // Don't handle more parameters for this procedure.
                                doBreak = true;
                                break;
                            default:
                                // Unknown parameter type
                                newProcedure.Status = ProcedureStatus.NotSupported;
                                newProcedure.SetErrorText(PLSQLSupportedSpecTexts.ProcedureParameterTypeUnknown, parameter.Name);

                                // Set the parameter type
                                newParam.ParameterType = ParameterType.NotSupported;

                                // Don't handle more parameters for this procedure.
                                doBreak = true;
                                break;
                        }

                        // Translate directions
                        switch (parameter.Direction)
                        {
                            case ParseParameterDirection.In:
                                newParam.Direction = ParameterDirection.In;
                                break;
                            case ParseParameterDirection.InOut:
                                newParam.Direction = ParameterDirection.InOut;
                                break;
                            case ParseParameterDirection.Out:
                                newParam.Direction = ParameterDirection.Out;
                                break;
                        }

                        // Add the parameter to the parameterlist
                        newProcedure.Parameters.Add(newParam);

                        // Don't handle more parameters if this variable is set to true.
                        if (doBreak)
                        {
                            break;
                        }
                    }

                    // Add the procedure to the procedure list
                    spec.Procedures.Add(newProcedure);
                }
            }

            return spec;
        }
    }
}
