using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    public class PLSQLSpecParsedProcedureCollection : List<PLSQLSpecParsedProcedure>
    {
        /// <summary>
        ///     Backreference to the spec where the procedures are stored.
        ///     Mainly used for beeing able to aggregate the "HasParseError"
        ///     value to the spec.
        /// </summary>
        private PLSQLSpecParsed specParsed = null;

        /// <summary>
        ///     If there are any errors on any procedure in the collection then this
        ///     is also set to true.
        /// </summary>
        private bool hasParseError = false;

        /// <summary>
        ///     If there are any errors on any procedure in the collection then this
        ///     is also set to true.
        /// </summary>
        public bool HasParseError 
        {
            get { return hasParseError; }
            set
            {
                hasParseError = value;

                if (value == true)
                {
                    specParsed.HasParseError = true;
                }
            }
        }

        /// <summary>
        ///     Get a count of how many procedures there are with parse errors.
        /// </summary>
        public int CountErrors 
        {
            get
            {
                int cnt = 0;

                for (int i = 0; i < Count; i++)
                {
                    if (this[i].HasParseError)
                    {
                        cnt++;
                    }
                }

                return cnt;
            }
        }

        /// <summary>
        ///     Error text for the parsing
        /// </summary>
        public string ParseErrorText { get; private set; }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public PLSQLSpecParsedProcedureCollection(PLSQLSpecParsed spec) 
        {
            HasParseError = false;
            ParseErrorText = string.Empty;
            specParsed = spec;
        }

        /// <summary>
        ///     Overloaded procedure Add which fetches the errors on the procedures
        ///     and aggregates it to this collection.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(PLSQLSpecParsedProcedure item)
        {
            if (item.HasParseError)
            {
                HasParseError = true;
            }

            base.Add(item);
        }

        /// <summary>
        ///     Procedure for setting the errortext.
        /// </summary>
        /// <param name="errorText">The errortext that can include {0} to represent the errorvalue.</param>
        /// <param name="errorFieldValue">The value that is the cause of the error.</param>
        public void SetParseErrorText(string errorText, string errorFieldValue)
        {
            ParseErrorText += string.Format(errorText, errorFieldValue) + Environment.NewLine;
        }

        /// <summary>
        ///     Show the string version of the procedure collection.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string procString = string.Empty;

            foreach (PLSQLSpecParsedProcedure proc in this)
            {
                procString += "----------------------------" + Environment.NewLine;
                procString += proc.ToString() + Environment.NewLine;
            }

            if (Count > 0)
            {
                procString += "----------------------------" + Environment.NewLine;
            }

            return procString;
        }

    }
}
