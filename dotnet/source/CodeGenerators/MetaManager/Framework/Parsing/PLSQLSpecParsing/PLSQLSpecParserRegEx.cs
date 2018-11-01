using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    public static class PLSQLSpecParserRegEx
    {
        // -------------------------------------------------------------------------------------------

        // RegEx pattern for finding the packagename and all procedures in a long string.
        public const string SPEC_PACKAGE_FETCH =
                              @"(?imns:" +                              // Regex options: Ignore case, multi-line, Explicitcapture and Single-line
                               @".*?" +                                 // Ignore everything before "create" statement
                               @"^create\s+or\s+replace\s+package\s+" + // Find row where the actuall package creation starts.
                               @"(?<PackageName>\w+)" +                 // Fetch the name of the package that is created.
                               @"\s+as\s*" +
                               @"(?<Procedures>.*?)\s*" +               // Fetch all procedures
                               @"^end\s+\k<PackageName>\s*;\s*\/" +     // Match end of spec
                               ")";

        // Match group names for the PROCEDURE_COUNT regular expression.
        public const string SPEC_PACKAGE_PACKAGENAME = "PackageName";
        public const string SPEC_PACKAGE_PROCEDURES = "Procedures";

        // -------------------------------------------------------------------------------------------

        // RegEx pattern for finding all procedures and functions
        // in a multiline string. Used to compare against the other RegEx pattern
        // to know if all procedures and functions were fetched correctly.
        public const string PROCEDURE_COUNT = 
                              @"(?imns:" +                      // Regex options: Ignore case, multi-line, Explicitcapture and Single-line
                                "(" +                           // Start of matching a procedure
                                    @"(?<Type>^procedure)\s+" + // Catching the type (procedure)
                                    @"(?<Name>[^\s\(]+)" +      // Catching the name of the procedure
                                     "(" +                                 // Start of matching comments
                                     @"(--[^\n]*\s*)" +                    // Ignore all end-of-the-line comments
                                      "|" +                                // Or
                                     @"(/\*.*\*/\s*)" +                   // Ignore all /* */ comments
                                      "|" +                                // Or
                                     @"[^;]*" +
                                      ")*" +                               // Comments are optional
                                    @";" +
                                ")" +
                                "|" +                           // Or
                                "(" +                           // Start of matching a function
                                    @"(?<Type>^function)\s+" +  // Catching the type (function)
                                    @"(?<Name>[^\s\(]+)" +      // Catching the name of the function
                                     "(" +                      // Start of matching comments
                                     @"(--[^\n]*\s*)" +         // Ignore all end-of-the-line comments
                                      "|" +                     // Or
                                     @"(/\*.*\*/\s*)" +        // Ignore all /* */ comments
                                      "|" +                     // Or
                                     @"[^;]*" +
                                      ")*" +                    // Comments are optional
                                    @";" +
                                ")" +
                               ")";

        // Match group names for the PROCEDURE_COUNT regular expression.
        public const string PROCEDURE_COUNT_NAME = "Name";
        public const string PROCEDURE_COUNT_TYPE = "Type";

        // -------------------------------------------------------------------------------------------

        // RegEx pattern to fetch all procedures and functions from a multiline string.
        public const string PROCEDURE_FETCH = 
                         @"(?imns:" +                                   // Regex options: Ignore case, multi-line, Explicitcapture and Single-line
                            "(" +                                       // Start of matching a procedure
                                @"(?<Type>^procedure)\s+" +             // Catching the type (procedure)
                                @"(?<Name>.+?)\s*" +                    // Catching the name of the procedure
                                 "(" +
                                   @"\(\s*(?<Parameters>.*?)\s*\)" +    // Fetch parameters
                                 ")?" +                                 // Only fetch parameters if they exist
                                @"\s*?;" +                              // Ignore whitespace up until the ;
                            ")" +
                            "|" +                                       // Or
                            "(" +                                       // Start of matching a function
                                @"(?<Type>^function)\s+" +              // Catching the type (function)
                                @"(?<Name>.+?)\s*" +                    // Catching the name of the function
                                 "(" +
                                   @"\(\s*(?<Parameters>.*?)\s*\)\s*" + // Fetch parameters
                                 ")?" +                                 // Only fetch parameters if they exist
                                 "(" +                                  // Start of matching comments
                                  @"(--[^\n]*\s*)" +                    // Ignore all end-of-the-line comments
                                   "|" +                                // Or
                                  @"(/\*.*?\*/\s*)" +                   // Ignore all /* */ comments
                                 ")*" +                                 // Comments are optional
//                                @"return\s+(?<ReturnType>[^;]+);" +     // Catching the return data type of the function
                                @"return\s+" +                          // Catching the return data type of the function
                                 "(" +
                                  "(" +
                                  @"(?<ReturnTypeTable>\w+)\." +        // Catch of return datatype Table.
                                  @"(?<ReturnTypeColumn>\w+)%type\s*" + // Catch of return datatype Column.
                                  ")" +
                                  "|" +                                 // Or
                                  "(" +
                                  @"(?<ReturnTypeDBType>\w+)\s*" +      // Catch the specific return datatype
                                  ")" +
                                 ");" +
                            ")" +
                          ")";

        // Match group names for the PROCEDURE_FETCH regular expression.
        public const string PROCEDURE_FETCH_NAME = "Name";
        public const string PROCEDURE_FETCH_TYPE = "Type";
        public const string PROCEDURE_FETCH_PARAMETERS = "Parameters";
        public const string PROCEDURE_FETCH_RETURNTYPE_TABLE = "ReturnTypeTable";
        public const string PROCEDURE_FETCH_RETURNTYPE_COLUMN = "ReturnTypeColumn";
        public const string PROCEDURE_FETCH_RETURNTYPE_DBTYPE = "ReturnTypeDBType";

        // -------------------------------------------------------------------------------------------

        // RegEx pattern for finding all parameters in a multiline string. 
        // Used to compare against the other RegEx pattern
        // to know if all parameters were fetched correctly.
        public const string PARAMETER_COUNT =
                              @"(?imsn:" +                                  // Regex options: Ignore case, multi-line, Explicitcapture and Single-line
                                "(" +
                                    @"\s*" +                                // Ignore leading whitespace
                                    "(" +                                   // Start of matching comments
                                     @"(--[^\n]*\s*)" +                     // Ignore all end-of-the-line comments
                                      "|" +                                 // Or
                                     @"(/\*.*\*/\s*)" +                     // Ignore all /* */ comments
                                    ")*" +                                  // Comments are optional
                                    @"(?<Name>\w+)" +                       // Catching the name of the parameter
                                    @"[^,]*[,]?" +                          // Ignore everything else on the row until a comma "," is found.
                                ")" +
                               ")";

        // Match group names for the PARAMETER_COUNT regular expression.
        public const string PARAMETER_COUNT_NAME = "Name";

        // -------------------------------------------------------------------------------------------

        // RegEx pattern for fetching all parameters with datatypes etc in a multiline string
        // only containing everything between the parenthesis "( )" in a procedure or function.
        public const string PARAMETER_FETCH = 
                         @"(?imsn:" +
                            "(" +
                                @"\s*" +                                // Ignore leading whitespace
                                "(" +                                   // Start of matching comments
                                 @"(--[^\n]*\s*)" +                     // Ignore all end-of-the-line comments
                                  "|" +                                 // Or
                                 @"(/\*.*\*/\s*)" +                     // Ignore all /* */ comments
                                ")*" +                                  // Comments are optional
                                @"(?<Name>\w+)\s*" +                    // Catching the name of the parameter
                                "(" +
                                    @"(?<IsMandatory>" +                // Catch a comment including the word "mandatory".
                                        @"(/\*\s*?Mandatory\s*?\*/)" +
                                    @")\s*" +
                                    "|" +                               // Or
                                    @"(?<IsInAndOut>" +                 // Catch if the parameter is both in and out.
                                       @"(in\s+out)" +                  // Catch "in out"
                                        "|" +
                                       @"(out\s+in)" +                  // Catch the reverse "out in"
                                    @")\s+" +
                                    "|" +                               // Or
                                    @"(?<IsOut>(out))\s+" +             // Catch if parameter is only out
                                    "|" +                               // Or
                                    @"(in)\s+" +                        // Ignore if only "in" is set on a parameter since that is default anyway
                                    "|" +                               // Or
                                    @"(/\*.*\*/\s*)" +                  // Ignore rest of all /* */ comments
                                ")*" +                                  // The above group with Mandatory, In and Out, Out and the In is all optional.
                                "(" +                                   // Start of cathing the type of the parameter
                                    "(" +
                                    @"(?<Table>\w+)\." +                // Catch of Table.
                                    @"(?<Column>\w+)%type\s*" +         // Catch of Column.
                                    ")" +
                                    "|" +                               // Or
                                    "(" +
                                    @"(?<TableRow>\w+)%rowtype\s*" +    // Catch of the table rowtype
                                    ")" +
                                    "|" +                               // Or
                                    "(" +
                                    @"(?<Package>\w+)\." +              // Catch of Package name
                                    @"(?<PackageDataType>\w+)\s*" +     // Catch of datatype specified in Package
                                    ")" +
                                    "|" +                               // Or
                                    "(" +
                                    @"(?<DBType>\w+)\s*" +              // Catch the specific datatype
                                    ")" +
                                ")" +
                                "(" +
                                    @":=\s*(?<DefaultValue>([^\s,]+))" + // Catch the defaultvalue
                                ")?" +                                  // The defaultvalue is optional.
                                @"[^,]*[,]?" +                          // Ignore everything else on the row until a comma "," is found.
                            ")" +
                          ")";

        // Match group names for the PARAMETER_FETCH regular expression.
        public const string PARAMETER_FETCH_NAME = "Name";
        public const string PARAMETER_FETCH_ISMANDATORY = "IsMandatory";
        public const string PARAMETER_FETCH_ISINANDOUT = "IsInAndOut";
        public const string PARAMETER_FETCH_ISOUT = "IsOut";
        public const string PARAMETER_FETCH_TABLE = "Table";
        public const string PARAMETER_FETCH_COLUMN = "Column";
        public const string PARAMETER_FETCH_TABLEROW = "TableRow";
        public const string PARAMETER_FETCH_PACKAGE = "Package";
        public const string PARAMETER_FETCH_PACKAGEDATATYPE = "PackageDataType";
        public const string PARAMETER_FETCH_DBTYPE = "DBType";
        public const string PARAMETER_FETCH_DEFAULTVALUE = "DefaultValue";

        // -------------------------------------------------------------------------------------------

    }
}
