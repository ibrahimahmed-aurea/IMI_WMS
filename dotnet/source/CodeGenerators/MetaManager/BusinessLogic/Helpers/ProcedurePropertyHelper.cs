using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.Framework.Parsing.PLSQLPackageSpecification;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ProcedurePropertyHelper
    {
        public static ProcedureProperty CreateProcedureProperty(PLSQLProcedureParameter param)
        {
            ProcedureProperty storedProcParam = new ProcedureProperty();

            // Set parameter name
            storedProcParam.Name = param.Name;

            // Length, precision is set to null since it can't be fetched here.
            storedProcParam.Length = null;
            storedProcParam.Precision = null;
            storedProcParam.Scale = null;

            // Set direction of parameter
            switch (param.Direction)
            {
                case ParameterDirection.In:
                    storedProcParam.PropertyType = DbPropertyType.In;
                    break;
                case ParameterDirection.InOut:
                    storedProcParam.PropertyType = DbPropertyType.InOut;
                    break;
                case ParameterDirection.Out:
                    storedProcParam.PropertyType = DbPropertyType.Out;
                    break;
                default:
                    storedProcParam.PropertyType = DbPropertyType.In;
                    break;
            }

            // Not supported.
            storedProcParam.Text = string.Empty;

            storedProcParam.DbDatatype = string.Empty;
            storedProcParam.OriginalTable = string.Empty;
            storedProcParam.OriginalColumn = string.Empty;

            if (param.ParameterType == ParameterType.DataType)
            {
                storedProcParam.DbDatatype = PLSQLProcedureParameter.GetParameterDBTypeAsString(param.ParameterTypeDataType_DataType);
            }
            else if (param.ParameterType == ParameterType.TableColumn)
            {
                storedProcParam.OriginalTable = param.ParameterTypeTableColumn_Table;
                storedProcParam.OriginalColumn = param.ParameterTypeTableColumn_Column;
            }

            // Not supported with defaultvalues
            storedProcParam.DefaultValue = string.Empty;

            // Set mandatory flag
            storedProcParam.IsMandatory = param.IsMandatory;

            return storedProcParam;
        }
    }
}
