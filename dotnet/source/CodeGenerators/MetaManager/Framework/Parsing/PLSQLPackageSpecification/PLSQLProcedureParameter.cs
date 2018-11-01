using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Cdc.Framework.Parsing.PLSQLPackageSpecification
{
    /// <summary>
    ///     The direction of the parameter: In, Out or both In and Out.
    /// </summary>
    public enum ParameterDirection { In, InOut, Out };

    /// <summary>
    ///     The different kinds of parametertypes.
    ///         * NotSupported - All types that can be parsed but we're not supporting.
    ///         * DataType - This is parameters with "hardcoded" datatypes like varchar2, boolean, number etc.
    ///                      This also includes "custom" datatypes that are declared within the same spec file
    ///                      although custom datatypes are not supported and will result in an error.
    ///         * TableColumn - This is parameters that are declared like TABLE.COLUMN%type.
    /// </summary>
    public enum ParameterType { NotSupported, DataType, TableColumn };

    /// <summary>
    ///     Database datatypes that are supported as parameters.
    /// </summary>
    public enum ParameterDBDataType { Invalid, Varchar2, Varchar, Date, Number, Boolean, Ref_Cur };

    /// <summary>
    ///     Class for handling PLSQLProcedureParameters.
    /// </summary>
    public class PLSQLProcedureParameter
    {
        public struct ParameterTypeDataStruct
        {
            public string Param1;
            public string Param2;
            public ParameterDBDataType DataType;
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}", Name, Direction, ParameterType);
        }

        public PLSQLProcedureParameter()
        {
            Name = string.Empty;
            IsMandatory = false;
            Direction = ParameterDirection.In;
            ParameterType = ParameterType.DataType;
            ParameterTypeData = new ParameterTypeDataStruct();
        }

        /// <summary>
        ///     Property for setting and getting the DataType of the parameter with a normal datatype.
        /// </summary>
        public ParameterDBDataType ParameterTypeDataType_DataType
        {
            get { return ParameterTypeData.DataType; }
            set { ParameterTypeData.DataType = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Table of the parameter 
        ///     with a normal TABLE.COLUMN%type type parameter.
        /// </summary>
        public string ParameterTypeTableColumn_Table
        {
            get { return ParameterTypeData.Param1; }
            set { ParameterTypeData.Param1 = value; }
        }

        /// <summary>
        ///     Property for setting and getting the Column of the parameter 
        ///     with a normal TABLE.COLUMN%type type parameter.
        /// </summary>
        public string ParameterTypeTableColumn_Column
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
        public ParameterDirection Direction { get; set; }

        /// <summary>
        ///     The type of the parameter.
        /// </summary>
        public ParameterType ParameterType { get; set; }

        /// <summary>
        ///     Data for the parametertype
        /// </summary>
        public ParameterTypeDataStruct ParameterTypeData;

        /// <summary>
        ///     Function for retreiving the different datatypes as a string.
        /// </summary>
        /// <param name="dataType">The databasetype to get as string.</param>
        /// <returns>Returns the databasetype given as a string.</returns>
        public static string GetParameterDBTypeAsString(ParameterDBDataType dataType)
        {
            switch (dataType)
            {
                case ParameterDBDataType.Boolean: return "BOOLEAN";
                case ParameterDBDataType.Date: return "DATE";
                case ParameterDBDataType.Number: return "NUMBER";
                case ParameterDBDataType.Varchar: return "VARCHAR";
                case ParameterDBDataType.Varchar2: return "VARCHAR2";
                case ParameterDBDataType.Ref_Cur: return "REF_CUR";
                default:
                case ParameterDBDataType.Invalid: return "<INVALID DATATYPE>";
            }
        }
    }

}
