using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Cdc.Framework.Parsing.PLSQLPackageSpecification
{
    public class PLSQLSpec
    {
        private PLSQLProcedureCollection procCollection;

        public PLSQLProcedureCollection Procedures
        {
            get { return procCollection; }
        }

        /// <summary>
        ///     If there are any errors anywhere in spec.
        /// </summary>
        public bool HasErrors { get; set; }

        /// <summary>
        ///     Name of the spec package
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        ///     Hash of the read string to be able to compare this to another read of this same
        ///     package to know if it's the same or not.
        /// </summary>
        public string PackageHash { get; set; }

        /// <summary>
        ///     Filename that was parsed
        /// </summary>
        public string FileNameParsed { get; set; }

        /// <summary>
        ///     The length of the read package string.
        /// </summary>
        public int PackageLength { get; set; }

        public PLSQLSpec()
        {
            procCollection = new PLSQLProcedureCollection(this);
        }

        public PLSQLProcedure this[int procedureIndex]
        {
            get
            {
                return procCollection[procedureIndex];
            }
        }

        public PLSQLProcedure this[string procedureName]
        {
            get
            {
                foreach (PLSQLProcedure proc in procCollection)
                {
                    if (proc.Name == procedureName)
                    {
                        return proc;
                    }
                }
                return null;
            }
        }

        public override string ToString()
        {
            string specString = string.Empty;

            specString += string.Format("{0,-26}{1}{2}", "Filename:", FileNameParsed, Environment.NewLine);
            specString += string.Format("{0,-26}{1:#####0}{2}", "PackageLength:", PackageLength, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "PackageName:", PackageName, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "PackageHash:", PackageHash, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "AnyErrors:", HasErrors.ToString(), Environment.NewLine);

            if (HasErrors)
            {
                specString += string.Format("{0,-26}{1:##0}{2}", "Parse Errors:", Procedures.CountParseErrors, Environment.NewLine);
                specString += string.Format("{0,-26}{1:##0}{2}", "NotSupported Errors:", Procedures.CountNotSupported, Environment.NewLine);
            }

            specString += string.Format("{0,-26}{1:##0}/{2:##0}{3}", "Procedures (Valid/Total):", Procedures.Count - Procedures.CountParseErrors - Procedures.CountNotSupported, Procedures.Count, Environment.NewLine);

            specString += Procedures.ToString();

            return specString;
        }
    }
}
