using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    /// <summary>
    ///     The direction of the parameter: In, Out or both In and Out.
    /// </summary>
    public enum ParseParameterDirection { In, InOut, Out };

    /// <summary>
    ///     The different kinds of parametertypes.
    ///         * DataType - This is parameters with "hardcoded" datatypes like varchar2, boolean, number etc.
    ///                      This also includes "custom" datatypes that are declared within the same spec file.
    ///         * TableColumn - This is parameters that are declared like TABLE.COLUMN%type.
    ///         * TableRow - When the parameter is declared as TABLE%rowtype.
    ///         * PackageDataType - When the parameter is declared with PACKAGENAME.CUSTOMDATATYPE.
    /// </summary>
    public enum ParseParameterType { DataType, TableColumn, TableRow, PackageDataType };

    /// <summary>
    ///     Database datatypes that are supported (when parsing) as parameters.
    /// </summary>
    public enum ParseParameterDBDataType { Invalid, Varchar2, Varchar, Date, Number, Boolean, Ref_Cur };

    public class PLSQLSpecParsedProcedureParameter
    {
        public struct ParseParameterTypeDataStruct
        {
            public string Param1;
            public string Param2;
            public ParseParameterDBDataType DataType;
        }

        public PLSQLSpecParsedProcedureParameter()
        {
            Name = string.Empty;
            IsMandatory = false;
            Direction = ParseParameterDirection.In;
            DefaultValue = string.Empty;
            ParameterType = ParseParameterType.DataType;
            ParameterTypeData = new ParseParameterTypeDataStruct();
        }

        /// <summary>
        ///     Property for setting and getting the DataType of the parameter with a normal datatype.
        /// </summary>
        public ParseParameterDBDataType ParseParameterTypeDataType_DataType
        {
            get { return ParameterTypeData.DataType; }
            set { ParameterTypeData.DataType = value; } 
        }

        /// <summary>
        ///     Property for setting and getting the Table of the parameter 
        ///     with a normal TABLE.COLUMN%type type parameter.
        /// </summary>
        public string ParseParameterTypeTableColumn_Table 
        { 
            get { return ParameterTypeData.Param1; }
            set { ParameterTypeData.Param1 = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Column of the parameter 
        ///     with a normal TABLE.COLUMN%type type parameter.
        /// </summary>
        public string ParseParameterTypeTableColumn_Column
        { 
            get { return ParameterTypeData.Param2; }
            set { ParameterTypeData.Param2 = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Table of the parameter 
        ///     with a TABLE%rowtype type parameter.
        /// </summary>
        public string ParseParameterTypeTableRow_Table
        { 
            get { return ParameterTypeData.Param1; }
            set { ParameterTypeData.Param1 = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Package of the parameter 
        ///     with a normal PACKAGE.CUSTOMDATATYPE type parameter.
        /// </summary>
        public string ParseParameterTypePackageDataType_Package
        { 
            get { return ParameterTypeData.Param1; }
            set { ParameterTypeData.Param1 = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Custom Datatype of the parameter 
        ///     with a normal PACKAGE.CUSTOMDATATYPE type parameter.
        /// </summary>
        public string ParseParameterTypePackageDataType_CustomDataType
        { 
            get { return ParameterTypeData.Param2; }
            set { ParameterTypeData.Param2 = value; }
        }

        /// <summary>
        ///     The name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     If the parameter is mandatory or not.
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        ///     The direction of the parameter.
        /// </summary>
        public ParseParameterDirection Direction { get; set; }

        /// <summary>
        ///     The defaultvalue for the parameter.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        ///     The type of the parameter.
        /// </summary>
        public ParseParameterType ParameterType { get; set; }

        /// <summary>
        ///     Data for the parametertype
        /// </summary>
        public ParseParameterTypeDataStruct ParameterTypeData;

    }
}
