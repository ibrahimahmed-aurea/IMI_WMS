using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Cdc.Framework.Parsing.PLSQLPackageSpecification
{
    public class PLSQLProcedureCollection : List<PLSQLProcedure>
    {
        /// <summary>
        ///     Backreference to the spec where the procedures are stored.
        ///     Mainly used for beeing able to aggregate the "HasErrors"
        ///     value to the spec.
        /// </summary>
        private PLSQLSpec spec = null;

        /// <summary>
        ///     If there are any errors on any procedure in the collection then this
        ///     is also set to true.
        /// </summary>
        private bool hasErrors = false;

        public bool HasErrors
        {
            get { return hasErrors; }
            set
            {
                hasErrors = value;

                if (value == true)
                {
                    spec.HasErrors = true;
                }
            }
        }

        /// <summary>
        ///     Get a count of how many procedures there are with parse errors.
        /// </summary>
        public int CountParseErrors
        {
            get
            {
                int cnt = 0;

                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Status == ProcedureStatus.ParseError)
                    {
                        cnt++;
                    }
                }

                return cnt;
            }
        }

        /// <summary>
        ///     Get a count of how many procedures there are that are not supported.
        /// </summary>
        public int CountNotSupported
        {
            get
            {
                int cnt = 0;

                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Status == ProcedureStatus.NotSupported)
                    {
                        cnt++;
                    }
                }

                return cnt;
            }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public PLSQLProcedureCollection(PLSQLSpec spec) 
        {
            this.spec = spec;
        }

        /// <summary>
        ///     Overloaded procedure Add which fetches the errors on the procedures
        ///     and aggregates it to this collection.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(PLSQLProcedure item)
        {
            if (item.Status != ProcedureStatus.Valid)
            {
                HasErrors = true;
            }

            base.Add(item);
        }

        public override string ToString()
        {
            string procString = string.Empty;

            foreach (PLSQLProcedure proc in this)
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
