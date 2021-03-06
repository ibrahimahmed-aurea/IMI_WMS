﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Authentication.DataAccess
{
    class ModifyUserDetailsTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(ModifyUserDetailsParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;
            
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "EMPID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.UserIdentity;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "NODE_ID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.RecentNodeIdentity;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "LASTLOGONDTM";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(DateTime));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.LastLogonTime;
            parameterList.Add(dbParameter);
                                    
            return parameterList;
        }
    }
}
