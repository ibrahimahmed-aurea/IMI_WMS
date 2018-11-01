using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Imi.Framework.HashLib;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    public static class PLSQLSpecParser
    {
        public static PLSQLSpecParsed ParseFile(string pathAndFileName)
        {
            string fullSpec = ReadSpecFile(pathAndFileName);

            if (fullSpec != string.Empty)
            {
                return ParseString(fullSpec, pathAndFileName);
            }

            return null;
        }

        public static PLSQLSpecParsed ParseString(string stringToParse)
        {
            return ParseString(stringToParse, string.Empty);
        }

        public static PLSQLSpecParsed ParseString(string stringToParse, string pathAndFileName)
        {
            PLSQLSpecParsed specFile = null;

            if (!string.IsNullOrEmpty(stringToParse))
            {
                // Create the regular expression objects needed
                Regex reSpec = new Regex(PLSQLSpecParserRegEx.SPEC_PACKAGE_FETCH);
                Regex reProcCount = new Regex(PLSQLSpecParserRegEx.PROCEDURE_COUNT);
                Regex reProcFetch = new Regex(PLSQLSpecParserRegEx.PROCEDURE_FETCH);
                Regex reParamCount = new Regex(PLSQLSpecParserRegEx.PARAMETER_COUNT);
                Regex reParamFetch = new Regex(PLSQLSpecParserRegEx.PARAMETER_FETCH);

                // Fetch the spec file
                MatchCollection specFetchMatches = reSpec.Matches(stringToParse);

                // Check that we got one match
                if (specFetchMatches.Count == 1)
                {
                    // Create spec object
                    specFile = new PLSQLSpecParsed();

                    // Get the length of the read string.
                    specFile.PackageLength = stringToParse.Length;

                    // Get the hash of the read string.
                    specFile.PackageHash = Hashing.Get(Hash.HashTypes.SHA1, stringToParse);

                    // Get the filename that was read to parse
                    specFile.FileNameParsed = pathAndFileName;

                    // Get the only match
                    Match specMatch = specFetchMatches[0];

                    // Fetch the packagename
                    if (!string.IsNullOrEmpty(specMatch.Groups[PLSQLSpecParserRegEx.SPEC_PACKAGE_PACKAGENAME].Value))
                    {
                        specFile.PackageName = specMatch.Groups[PLSQLSpecParserRegEx.SPEC_PACKAGE_PACKAGENAME].Value;

                        string procedures = specMatch.Groups[PLSQLSpecParserRegEx.SPEC_PACKAGE_PROCEDURES].Value;

                        if (!string.IsNullOrEmpty(procedures))
                        {
                            // Do the fetch of the procedures and functions.
                            MatchCollection procFetchMatches = reProcFetch.Matches(procedures);

                            // Loop through the fetched procedures.
                            foreach (Match procMatch in procFetchMatches)
                            {
                                PLSQLSpecParsedProcedure procedure = new PLSQLSpecParsedProcedure();

                                // Fetch the name of the procedure/function
                                if (procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_NAME].Value != string.Empty)
                                {
                                    procedure.Name = procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_NAME].Value;
                                }
                                else
                                {
                                    // Name is empty!
                                    specFile.Procedures.HasParseError = true;
                                    specFile.Procedures.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureNameError, procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_NAME].Value);

                                    continue;
                                }

                                // Fetch if this is a function or procedure
                                if (!procedure.HasParseError)
                                {
                                    bool? isFunction = GetIsFunction(procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_TYPE].Value);

                                    if (isFunction == true)
                                    {
                                        procedure.IsFunction = true;
                                    }
                                    else if (isFunction == false)
                                    {
                                        procedure.IsFunction = false;
                                    }
                                    else
                                    {
                                        // Invalid type!
                                        procedure.HasParseError = true;
                                        procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureTypeError, procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_TYPE].Value);
                                    }
                                }

                                // Fetch the function return type if it's a function.
                                if (!procedure.HasParseError)
                                {
                                    if (procedure.IsFunction)
                                    {
                                        // What type of return data type is it.
                                        if (procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_TABLE].Value != string.Empty &&
                                            procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_COLUMN].Value != string.Empty)
                                        {
                                            procedure.FunctionReturnType = ParseFunctionReturnType.TableColumn;

                                            procedure.FunctionReturnTypeData.Param1 = procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_TABLE].Value;
                                            procedure.FunctionReturnTypeData.Param2 = procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_COLUMN].Value;
                                        }
                                        else if (procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_DBTYPE].Value != string.Empty)
                                        {
                                            procedure.FunctionReturnType = ParseFunctionReturnType.DataType;

                                            procedure.FunctionReturnTypeData.DataType = GetFunctionReturnDataType(procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_DBTYPE].Value);

                                            if (procedure.FunctionReturnTypeData.DataType == ParseFunctionReturnDBDataType.Invalid)
                                            {
                                                procedure.HasParseError = true;
                                                procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureFunctionReturnDataTypeError, procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_RETURNTYPE_DBTYPE].Value);
                                            }
                                        }
                                        else
                                        {
                                            // Parse error. Function return type unknown.
                                            procedure.HasParseError = true;
                                            procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureFunctionReturnTypeUnknown, string.Empty);
                                        }
                                    }
                                }

                                // Fetch all parameters into the string.
                                // It IS possible for the parameters to be empty since it could be a parameterless
                                // procedure or function.
                                if (!procedure.HasParseError)
                                {
                                    procedure.ParameterString = procMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_PARAMETERS].Value.Trim();

                                    if (procedure.ParameterString != string.Empty)
                                    {
                                        // First check how many parameters that exists.
                                        MatchCollection paramCountMatches = reParamCount.Matches(procedure.ParameterString);

                                        // Do the real fetch of the procedures and functions.
                                        MatchCollection paramFetchMatches = reParamFetch.Matches(procedure.ParameterString);

                                        // Check that the two different regular expressions generates the same count
                                        // of parameters.

                                        if (paramCountMatches.Count == paramFetchMatches.Count)
                                        {
                                            // Loop through all the parameters found
                                            foreach (Match paramMatch in paramFetchMatches)
                                            {
                                                PLSQLSpecParsedProcedureParameter param = new PLSQLSpecParsedProcedureParameter();

                                                // First check what type of parameter this is so we know what kind of
                                                // object we should instantiate.
                                                if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_TABLE].Value != string.Empty &&
                                                    paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_COLUMN].Value != string.Empty)
                                                {
                                                    // This is a parameter of type TABLE.COLUMN%type parameter.
                                                    param.ParameterType = ParseParameterType.TableColumn;

                                                    param.ParseParameterTypeTableColumn_Table = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_TABLE].Value;
                                                    param.ParseParameterTypeTableColumn_Column = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_COLUMN].Value;
                                                }
                                                else if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_TABLEROW].Value != string.Empty)
                                                {
                                                    // This is a parameter of type TABLE%rowtype parameter.
                                                    param.ParameterType = ParseParameterType.TableRow;

                                                    param.ParseParameterTypeTableRow_Table = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_TABLEROW].Value;
                                                }
                                                else if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_PACKAGE].Value != string.Empty &&
                                                         paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_PACKAGEDATATYPE].Value != string.Empty)
                                                {
                                                    // This is a parameter of type PACKAGE.PACKAGEDATATYPE parameter.
                                                    param.ParameterType = ParseParameterType.PackageDataType;

                                                    param.ParseParameterTypePackageDataType_Package = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_PACKAGE].Value;
                                                    param.ParseParameterTypePackageDataType_CustomDataType = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_PACKAGEDATATYPE].Value;
                                                }
                                                else if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_DBTYPE].Value != string.Empty)
                                                {
                                                    // This is a parameter of type varchar2, boolean etc. parameter.
                                                    param.ParameterType = ParseParameterType.DataType;

                                                    param.ParseParameterTypeDataType_DataType = GetParameterDataType(paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_DBTYPE].Value);

                                                    // Check the datatype if it's supported or not
                                                    if (param.ParseParameterTypeDataType_DataType == ParseParameterDBDataType.Invalid)
                                                    {
                                                        procedure.HasParseError = true;
                                                        procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterDataTypeNotParsable, paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_NAME].Value);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    // Unknown parametertype!
                                                    procedure.HasParseError = true;
                                                    procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterUnknownType, paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_NAME].Value);
                                                    break;
                                                }

                                                // Fetch the name of the parameter
                                                if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_NAME].Value != string.Empty)
                                                {
                                                    param.Name = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_NAME].Value;
                                                }
                                                else
                                                {
                                                    // Name is empty. Some error with parsing.
                                                    procedure.HasParseError = true;
                                                    procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterNameEmpty, string.Empty);
                                                    break;
                                                }

                                                // Check if mandatory
                                                // Pattern is created so that if there is text in the group
                                                // then it's mandatory. If empty it's not.
                                                if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_ISMANDATORY].Value != string.Empty)
                                                {
                                                    param.IsMandatory = true;
                                                }
                                                else
                                                {
                                                    param.IsMandatory = false;
                                                }

                                                // Check direction of parameter
                                                if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_ISINANDOUT].Value != string.Empty)
                                                {
                                                    param.Direction = ParseParameterDirection.InOut;
                                                }
                                                else if (paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_ISOUT].Value != string.Empty)
                                                {
                                                    param.Direction = ParseParameterDirection.Out;
                                                }
                                                else
                                                {
                                                    param.Direction = ParseParameterDirection.In;
                                                }

                                                // Fetch the defaultvalue (can be empty)
                                                param.DefaultValue = paramMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_DEFAULTVALUE].Value;

                                                // Add the parameter to the procedures parameters.
                                                procedure.Parameters.Add(param);
                                            }
                                        }
                                        else /* if (paramCountMatches.Count == paramFetchMatches.Count) */
                                        {
                                            if (paramCountMatches.Count > paramFetchMatches.Count)
                                            {
                                                // Check what parameter is missing.
                                                string checkName = string.Empty;

                                                foreach (Match countMatch in paramCountMatches)
                                                {
                                                    checkName = countMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_COUNT_NAME].Value;

                                                    foreach (Match fetchMatch in paramFetchMatches)
                                                    {
                                                        if (checkName == fetchMatch.Groups[PLSQLSpecParserRegEx.PARAMETER_FETCH_NAME].Value)
                                                        {
                                                            checkName = string.Empty;
                                                            break;
                                                        }
                                                    }

                                                    if (checkName != string.Empty)
                                                    {
                                                        // Name of first parameter found that is missing.
                                                        procedure.HasParseError = true;
                                                        procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterTooFewFetchedError, checkName);
                                                        break;
                                                    }
                                                }

                                                // Check if it's not set to error yet. There should be an error
                                                // even if we can't find any specific parameters that we miss.
                                                if (!procedure.HasParseError)
                                                {
                                                    procedure.HasParseError = true;
                                                    procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterTooFewFetchedGenericError, string.Empty);
                                                }
                                            }
                                            else
                                            {
                                                // Too many fetched parameters compared to counted ones. Very strange!
                                                procedure.HasParseError = true;
                                                procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureParameterTooManyFetchedGenericError, string.Empty);
                                            }
                                        }
                                    }
                                }

                                // Add the procedure to the collection.
                                if (procedure != null)
                                {
                                    specFile.Procedures.Add(procedure);
                                }
                            }

                            // Check how many procedures and functions that exists in file.
                            MatchCollection procCountMatches = reProcCount.Matches(procedures);

                            // Check if we got the same count of procedures.
                            if (procCountMatches.Count > procFetchMatches.Count)
                            {
                                // There are more procedures that has been counted than really got fetched.
                                // Find out what procedures and functions that we miss so that we can add
                                // "ghost" versions of them to set as errors.
                                string checkName = string.Empty;
                                int namesFound = 0;

                                foreach (Match countMatch in procCountMatches)
                                {
                                    checkName = countMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_COUNT_NAME].Value;

                                    foreach (Match fetchMatch in procFetchMatches)
                                    {
                                        if (checkName == fetchMatch.Groups[PLSQLSpecParserRegEx.PROCEDURE_FETCH_NAME].Value)
                                        {
                                            checkName = string.Empty;
                                            break;
                                        }
                                    }

                                    if (checkName != string.Empty)
                                    {
                                        namesFound++;

                                        PLSQLSpecParsedProcedure procedure = new PLSQLSpecParsedProcedure();

                                        procedure.Name = checkName;

                                        procedure.HasParseError = true;
                                        procedure.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureNotParsedError, string.Empty);

                                        // Add the "ghost" procedure so that it's possible to see the error.
                                        specFile.Procedures.Add(procedure);
                                    }
                                }

                                // Check if all names was found.
                                if (procCountMatches.Count + namesFound < procFetchMatches.Count)
                                {
                                    specFile.Procedures.HasParseError = true;
                                    specFile.Procedures.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureCollectionMissingProcedures, string.Empty);
                                }
                            }
                            else if (procCountMatches.Count < procFetchMatches.Count)
                            {
                                specFile.Procedures.HasParseError = true;
                                specFile.Procedures.SetParseErrorText(PLSQLSpecParserErrorTexts.ProcedureCollectionTooManyProcedures, string.Empty);
                            }
                        }
                    }
                }
            }

            return specFile;
        }

        private static string ReadSpecFile(string pathAndFileName)
        {
            StreamReader stream = null;

            try
            {
                stream = File.OpenText(pathAndFileName);

                string specFile = stream.ReadToEnd();

                stream.Close();
                stream.Dispose();

                return specFile;
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }

                // TODO: Fix some logging
                // Exception caught
            }
            return string.Empty;
        }

        /// <summary>
        ///     Procedure for fetching if the parsestring is function/procedure if invalid.
        /// </summary>
        /// <param name="typeString">String that should hold "function" or "procedure".</param>
        /// <returns>Returns true if "function", false if "procedure" and null in all other cases.</returns>
        public static bool? GetIsFunction(string typeString)
        {
            if (typeString == "function")
            {
                return true;
            }
            else if (typeString == "procedure")
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Function for fetching what return datatype a function returns.
        /// </summary>
        /// <param name="returnDataType">The datatype string that holds the datatype to be parsed.</param>
        /// <returns>Returns the implemented datatypes. Will return as invalid if datatype isn't implemented.</returns>
        public static ParseFunctionReturnDBDataType GetFunctionReturnDataType(string returnDataType)
        {
            if (returnDataType.ToLower() == "varchar2")
            {
                return ParseFunctionReturnDBDataType.Varchar2;
            }
            else if (returnDataType.ToLower() == "varchar")
            {
                return ParseFunctionReturnDBDataType.Varchar;
            }
            else if (returnDataType.ToLower() == "date")
            {
                return ParseFunctionReturnDBDataType.Date;
            }
            else if (returnDataType.ToLower() == "boolean")
            {
                return ParseFunctionReturnDBDataType.Boolean;
            }
            else if (returnDataType.ToLower() == "number")
            {
                return ParseFunctionReturnDBDataType.Number;
            }
            else if (returnDataType.ToLower() == "integer")
            {
                return ParseFunctionReturnDBDataType.Number;
            }
            else
            {
                return ParseFunctionReturnDBDataType.Invalid;
            }
        }

        /// <summary>
        ///     Function for converting a function return datatype to a string.
        /// </summary>
        /// <param name="returnDataType">The return datatype.</param>
        /// <returns>Returns the converted datatype as a string.</returns>
        public static string GetFunctionReturnDataTypeAsString(ParseFunctionReturnDBDataType returnDataType)
        {
            switch (returnDataType)
            {
                case ParseFunctionReturnDBDataType.Boolean: return "boolean";
                case ParseFunctionReturnDBDataType.Date: return "date";
                case ParseFunctionReturnDBDataType.Number: return "number";                
                case ParseFunctionReturnDBDataType.Varchar: return "varchar";
                case ParseFunctionReturnDBDataType.Varchar2: return "varchar2";
                default:
                    return "<Invalid>";
            }
        }

        /// <summary>
        ///     Function for fetching what parameter database datatype it is.
        /// </summary>
        /// <param name="parameterDataType">The string that holds the DBType to be parsed.</param>
        /// <returns>Returns the correct datatype if it exists. Returns invalid if it isn't supported.</returns>
        public static ParseParameterDBDataType GetParameterDataType(string parameterDataType)
        {
            if (parameterDataType.ToLower() == "varchar2")
            {
                return ParseParameterDBDataType.Varchar2;
            }
            else if (parameterDataType.ToLower() == "boolean")
            {
                return ParseParameterDBDataType.Boolean;
            }
            else if (parameterDataType.ToLower() == "number")
            {
                return ParseParameterDBDataType.Number;
            }
            else if (parameterDataType.ToLower() == "integer")
            {
                return ParseParameterDBDataType.Number;
            }
            else if (parameterDataType.ToLower() == "varchar")
            {
                return ParseParameterDBDataType.Varchar;
            }
            else if (parameterDataType.ToLower() == "date")
            {
                return ParseParameterDBDataType.Date;
            }
            else if (parameterDataType.ToLower() == "ref_cur")
            {
                return ParseParameterDBDataType.Ref_Cur;
            }
            else
            {
                return ParseParameterDBDataType.Invalid;
            }
        }

        /// <summary>
        ///     Function for converting a parameter datatype to a string. Used when recreating a procedure.
        /// </summary>
        /// <param name="parameterDataType">The datatype.</param>
        /// <returns>Returns the converted string.</returns>
        public static string GetParameterDataTypeAsString(ParseParameterDBDataType parameterDataType)
        {
            switch (parameterDataType)
            {
                case ParseParameterDBDataType.Boolean: return "boolean";
                case ParseParameterDBDataType.Date: return "date";
                case ParseParameterDBDataType.Number: return "number";
                case ParseParameterDBDataType.Varchar: return "varchar";
                case ParseParameterDBDataType.Varchar2: return "varchar2";
                case ParseParameterDBDataType.Ref_Cur: return "ref_cur";
                default:
                    return "<Invalid>";
            }
        }

    }
}
