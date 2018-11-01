using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public class OracleQuery
    {
        public string SQL { get; set; }

        private IList<OracleQueryParameter> parameters = null;

        public IList<OracleQueryParameter> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new List<OracleQueryParameter>();

                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        private IList<string> parseWarnings = null;

        public IList<string> ParseWarnings
        {
            get
            {
                if (parseWarnings == null)
                    parseWarnings = new List<string>();

                return parseWarnings;
            }
            set
            {
                parseWarnings = value;
            }
        }

        private IList<string> parseErrors = null;

        public IList<string> ParseErrors
        {
            get
            {
                if (parseErrors == null)
                    parseErrors = new List<string>();

                return parseErrors;
            }
            set
            {
                parseErrors = value;
            }
        }

        public virtual OracleQueryParameter GetPropertyByTypeAndSequence(OracleQueryParameterDirection direction, int sequence)
        {
            foreach (OracleQueryParameter qp in Parameters)
            {
                if (qp.Direction == direction && qp.Sequence == sequence)
                {
                    return qp;
                }
            }
            return null;
        }

        public virtual OracleQueryParameter GetPropertyByTypeAndName(OracleQueryParameterDirection direction, string name)
        {
            foreach (OracleQueryParameter qp in Parameters)
            {
                if (qp.Direction == direction && qp.Name == name)
                {
                    return qp;
                }
            }
            return null;
        }

    }
}
