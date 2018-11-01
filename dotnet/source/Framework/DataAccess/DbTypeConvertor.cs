using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Imi.Framework.DataAccess
{
    public class DbTypeConvertor
    {
        private DbTypeConvertor()
        { 
        }

        public static TTarget Convert<TTarget>(object value)
        {
            return (TTarget)global::System.Convert.ChangeType(value, typeof(TTarget));
        }

        public static string ConvertBoolToString(bool? value)
        {
            if (value == null)
                return "";
            else if (value == true)
                return "1";
            else
                return "0";
        }

        public static bool? ConvertStringToBool(string value)
        {
            if (value == null)
                return null;
            else if (value == "1")
                return true;
            else
                return false;
        }

        public static DbType ConvertToDbType(Type type)
        {
            if (type == typeof(string))
            {
                return DbType.String;
            }
            else if (type == typeof(short))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(int))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(long))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(double))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            else if (type == typeof(bool))
            {
                return DbType.String;
            }
            else if (type == typeof(Byte[]))
            {
                return DbType.Binary;
            }
            else
            {
                throw new NotSupportedException("The parameter type is not supported.");
            }
        }
    }
}
