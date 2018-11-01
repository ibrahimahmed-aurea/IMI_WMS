using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Cdc.Framework.Parsing.CSharpParsing
{
    public static class SourceParser
    {
        private static char[] separators = new char[] { ' ', ';' };

        public static List<string> Parse(string stringToParse)
        {
            List<string> result = new List<string>();

            StringReader reader = new StringReader(stringToParse);
            string[] lineParts;
            
            string line = reader.ReadLine();

            while (line != null)
            {
                lineParts = line.Split(separators,StringSplitOptions.RemoveEmptyEntries);

                if ((lineParts.Length > 0) && (lineParts[0].Equals("using")))
                {
                    if (lineParts.Length > 2)
                    {
                        result.Add(lineParts[3]);            
                    }
                    else
                    {
                        result.Add(lineParts[1]);
                    }
                }

                line = reader.ReadLine();
            }

            return result;
        }
    }
}
