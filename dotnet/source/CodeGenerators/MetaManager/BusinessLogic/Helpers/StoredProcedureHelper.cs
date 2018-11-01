using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.Framework.Parsing.PLSQLPackageSpecification;
using Cdc.Framework.Parsing.OracleSQLParsing;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class StoredProcedureHelper
    {
        public static StoredProcedure CreateStoredProcedure(PLSQLProcedure plsqlProcedure)
        {
            StoredProcedure storedProc = null;

            if (plsqlProcedure != null &&
                plsqlProcedure.Status == ProcedureStatus.Valid)
            {
                // Add procedures found in package specification that was parsed
                storedProc = new StoredProcedure();

                // Set the name of the procedure
                storedProc.ProcedureName = plsqlProcedure.Name;

                // If returning a ref cursor
                storedProc.IsReturningRefCursor = false;
                storedProc.RefCursorParameterName = string.Empty;

                // Counter to set sequence for parameters
                // First parameter has sequence = 1
                int paramSequenceCounter = 1;

                foreach (PLSQLProcedureParameter param in plsqlProcedure.Parameters)
                {
                    ProcedureProperty storedProcParam = ProcedurePropertyHelper.CreateProcedureProperty(param);

                    // Sequence of the parameters
                    storedProcParam.Sequence = paramSequenceCounter;

                    // Set the parent stored procedure for the parameter
                    storedProcParam.StoredProcedure = storedProc;

                    // Add the property to the stored procedure
                    storedProc.Properties.Add(storedProcParam);

                    // Add one to sequence counter
                    paramSequenceCounter++;
                }

                // Check if function.
                if (plsqlProcedure.IsFunction)
                {
                    // Add the return type as a property to the stored procedure.
                    ProcedureProperty storedProcParam = new ProcedureProperty();

                    // Add a name for the parameter
                    storedProcParam.Name = "Result";

                    // Length, precision is set to null since it can't be fetched here.
                    storedProcParam.Length = null;
                    storedProcParam.Precision = null;
                    storedProcParam.Scale = null;

                    // Set parameter of type function result
                    storedProcParam.PropertyType = DbPropertyType.Result;

                    // Sequence of the parameters
                    storedProcParam.Sequence = paramSequenceCounter;

                    // Set the parent stored procedure for the parameter
                    storedProcParam.StoredProcedure = storedProc;

                    // Not supported.
                    storedProcParam.Text = string.Empty;

                    storedProcParam.DbDatatype = string.Empty;
                    storedProcParam.OriginalTable = string.Empty;
                    storedProcParam.OriginalColumn = string.Empty;

                    if (plsqlProcedure.FunctionReturnType == FunctionReturnType.DataType)
                    {
                        storedProcParam.DbDatatype = PLSQLProcedure.GetFunctionReturnDBTypeAsString(plsqlProcedure.FunctionReturnTypeDataType_DataType);
                    }
                    else if (plsqlProcedure.FunctionReturnType == FunctionReturnType.TableColumn)
                    {
                        storedProcParam.OriginalTable = plsqlProcedure.FunctionReturnTypeTableColumn_Table;
                        storedProcParam.OriginalColumn = plsqlProcedure.FunctionReturnTypeTableColumn_Column;
                    }

                    // Not supported with defaultvalues
                    storedProcParam.DefaultValue = string.Empty;

                    // Set mandatory flag
                    storedProcParam.IsMandatory = false;

                    // Add the property to the stored procedure
                    storedProc.Properties.Add(storedProcParam);
                }
            }
            return storedProc;
        }

        public static StoredProcedure CreateStoredProcedure(PLSQLProcedure plsqlProcedure, RefCurStoredProcedure refCurStoredProcedure)
        {
            StoredProcedure storedProc = null;

            if (plsqlProcedure != null &&
                plsqlProcedure.Status == ProcedureStatus.Valid &&
                plsqlProcedure.Parameters.Count > 0 &&
                refCurStoredProcedure != null &&
                refCurStoredProcedure.Status == RefCurStoredProcedureStatus.Valid &&
                refCurStoredProcedure.ParameterList.Count > 0)
            {
                // Add procedures found in package specification that was parsed
                storedProc = new StoredProcedure();

                // Set the name of the procedure
                storedProc.ProcedureName = refCurStoredProcedure.Name;

                // If returning a ref cursor
                storedProc.IsReturningRefCursor = true;

                // Get the name of the column that is the Ref Cursor parameter
                storedProc.RefCursorParameterName = refCurStoredProcedure.InitialParameterList.Where(p => p.ProviderType == Oracle.DataAccess.Client.OracleDbType.RefCursor).ToList()[0].ColumnName;

                // Counter to set sequence for parameters
                // First parameter has sequence = 1
                int paramSequenceCounter = 1;

                foreach (PLSQLProcedureParameter param in plsqlProcedure.Parameters)
                {
                    ProcedureProperty storedProcParam = null;

                    if (param.ParameterType == ParameterType.DataType &&
                        param.ParameterTypeDataType_DataType == ParameterDBDataType.Ref_Cur)
                    {
                        foreach (OracleDataTypeInfo refCurParam in refCurStoredProcedure.ParameterList)
                        {
                            ProcedureProperty newProcParam = new ProcedureProperty();

                            // Set parameter name
                            newProcParam.Name = refCurParam.ColumnName;

                            newProcParam.Length = refCurParam.ColumnSize;
                            newProcParam.Precision = refCurParam.Precision;
                            newProcParam.Scale = refCurParam.Scale;

                            // Set direction of parameter
                            switch (refCurParam.Direction)
                            {
                                case System.Data.ParameterDirection.Input:
                                    newProcParam.PropertyType = DbPropertyType.In;
                                    break;
                                case System.Data.ParameterDirection.InputOutput:
                                    newProcParam.PropertyType = DbPropertyType.InOut;
                                    break;
                                case System.Data.ParameterDirection.Output:
                                    newProcParam.PropertyType = DbPropertyType.Out;
                                    break;
                                default:
                                    newProcParam.PropertyType = DbPropertyType.In;
                                    break;
                            }

                            // Sequence of the parameters
                            newProcParam.Sequence = paramSequenceCounter;

                            // Set the parent stored procedure for the parameter
                            newProcParam.StoredProcedure = storedProc;

                            // Not supported.
                            newProcParam.Text = string.Empty;

                            newProcParam.OriginalTable = string.Empty;
                            newProcParam.OriginalColumn = string.Empty;
                            newProcParam.DbDatatype = refCurParam.DataType;

                            // Not supported with defaultvalues
                            newProcParam.DefaultValue = string.Empty;

                            // Set mandatory flag
                            newProcParam.IsMandatory = false;

                            // Add the property to the stored procedure
                            storedProc.Properties.Add(newProcParam);

                            // Add one to sequence counter
                            paramSequenceCounter++;
                        }
                    }
                    else
                    {
                        storedProcParam = ProcedurePropertyHelper.CreateProcedureProperty(param);

                        // Sequence of the parameters
                        storedProcParam.Sequence = paramSequenceCounter;

                        // Set the parent stored procedure for the parameter
                        storedProcParam.StoredProcedure = storedProc;

                        // Add the property to the stored procedure
                        storedProc.Properties.Add(storedProcParam);

                        // Add one to sequence counter
                        paramSequenceCounter++;
                    }
                }
            }

            return storedProc;
        }
    }
}
