﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.Transportation.MessageHandler.BusinessEntities;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.Transportation.MessageHandler.DataAccess.Translators
{
    class InitializeTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(InitializeParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            OracleParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "ERRWARNXML_I";
            dbParameter.OracleDbType = OracleDbType.Clob;
            dbParameter.Direction = ParameterDirection.Input;

            if (parameters == null || string.IsNullOrEmpty(parameters.MessageXML))
                dbParameter.Value = DBNull.Value;
            else
                dbParameter.Value = parameters.MessageXML;

            parameterList.Add(dbParameter);
                        
            return parameterList;
        }
    }
}
