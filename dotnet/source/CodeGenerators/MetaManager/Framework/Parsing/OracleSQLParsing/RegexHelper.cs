using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public static class RegexHelper
    {
        public static MatchCollection Parse(string regEx, string input)
        {
            Regex re = new Regex(regEx);

            try
            {
                return re.Matches(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Match ParseGetFirstMatch(string regEx, string input)
        {
            Regex re = new Regex(regEx);

            try
            {
                return re.Match(input);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
