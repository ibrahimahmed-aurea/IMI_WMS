using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Imi.Framework.DataAccess;



namespace Imi.Framework.Services.ExceptionHandling
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class OracleExceptionHandler : IExceptionHandler
    {
        public OracleExceptionHandler(NameValueCollection ignore)
        { 

        }

        #region IExceptionHandler Members

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            OracleException ex = exception as OracleException;

            if (ex != null)
            {
                if (ex.Number == 1400)
                {
                    string[] data = ex.Message.Split(new string[] { "(\"", "\".\"", "\")" }, StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length > 3)
                        return new CheckConstraintException(ex.Message, data[2], data[3], true);
                }
                else if (ex.Number == 2290)
                { 
                    //"ORA-02290: check constraint (OWUSER.NULL_EMP_EMPNAME) violated\nORA-06512: at \"OWUSER.EMPLOYEE\", line 492\nORA-06512: at line 1"
                    

                    string[] data = ex.Message.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length > 2)
                    {
                        data = data[1].Split(new string[] { ".", "_" }, StringSplitOptions.RemoveEmptyEntries);
                                                
                        if (data.Length > 3)
                        {
                            string columnName = "";
                            string tableName = data[2];

                            for (int i = 3; i < data.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(columnName))
                                    columnName += "_";

                                columnName += data[i];
                            }

                            if (data[1] == "NULL")
                                return new CheckConstraintException(ex.Message, tableName, columnName, true);
                            else
                                return new CheckConstraintException(ex.Message, tableName, columnName, false);
                        }
                    }
                }
                else if (ex.Number == 2291)
                {
                    //ORA-02291: integrity constraint (OWUSER.FK_OLACOD_OLACODNL) violated - parent key not found
                    return new CheckConstraintException(ex.Message, string.Empty, string.Empty, false);
                }

                else if (ex.Number == 20201)
                {
                    return new MessageException(null);
                }
                else
                {
                    return GenericOraDbException(ex);
                }
            }

            return exception;
        }


        private DataAccessException GenericOraDbException(OracleException exception)
        {
            int errNr = exception.Number;
            string strErrCode = "ORA" + errNr.ToString();
            return new DataAccessException(exception.Message, strErrCode, exception); ;
        }

        #endregion
    }

}
