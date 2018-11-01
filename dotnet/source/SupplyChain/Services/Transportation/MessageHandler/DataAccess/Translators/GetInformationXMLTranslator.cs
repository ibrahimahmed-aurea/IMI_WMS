using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.Transportation.MessageHandler.BusinessEntities;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Imi.SupplyChain.Transportation.MessageHandler.DataAccess.Translators
{
    class GetInformationXMLTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(GetInformationXMLParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            OracleParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "NLANGCOD_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.LanguageCode;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter("XML_O", OracleDbType.Clob);
            dbParameter.Direction = ParameterDirection.ReturnValue;
            parameterList.Add(dbParameter);

            return parameterList;
        }

        public static GetInformationXMLResult TranslateResult(IDataParameterCollection resultParameters)
        {
            GetInformationXMLResult result = new GetInformationXMLResult();
            object data;

            data = ((IDbDataParameter)resultParameters["XML_O"]).Value;

            if (data != null && !((OracleClob)data).IsNull)
                result.InformationXML = ((OracleClob)data).Value;
            
            return result;
        }
    }
}
