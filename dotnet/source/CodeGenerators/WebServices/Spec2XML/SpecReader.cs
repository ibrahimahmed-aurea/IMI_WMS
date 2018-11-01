using System;
using System.Collections.Specialized;
using System.Collections;
using System.Data;

// ----------------------------------------------------------------------------

namespace Imi.CodeGenerators.WebServices.Spec2XML
{

    // ----------------------------------------------------------------------------

    class PLSQLSpecReader
    {
        private StringCollection spec;

        public PLSQLSpecReader(IDbConnection conn, string Package)
        {
            spec = GetLines(conn, Package);
        }

        private StringCollection GetLines(IDbConnection conn, string Package)
        {
            IDbCommand aQuery;
            IDataReader r;
            string text;
            StringCollection Lines = new StringCollection();

            aQuery = conn.CreateCommand();
            r = null;

            aQuery.CommandText =
              "select text from user_source" +
              " where name = upper('" + Package + "')" +
              "   and type = 'PACKAGE'" +
              " order by line";

            aQuery.Prepare();

            r = aQuery.ExecuteReader();

            while (r.Read())
            {
                text = r["TEXT"].ToString();
                Lines.Add(text);
            }

            r.Close();

            if (r != null)
            {
                r.Close();
            }

            return Lines;
        }

        // ----------------------------------------------------------------------------

        public string GetSpec()
        {
            string s = "";

            foreach (string line in spec)
                s += line + "\r\n";

            return s;
        }

    }

    // ----------------------------------------------------------------------------

}
