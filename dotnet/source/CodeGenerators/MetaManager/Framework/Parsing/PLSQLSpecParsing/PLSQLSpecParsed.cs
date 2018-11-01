using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.PLSQLSpecParsing
{
    public class PLSQLSpecParsed
    {
        /// <summary>
        ///     The collection of all procedures in the spec file
        /// </summary>
        private PLSQLSpecParsedProcedureCollection procedureCollection;

        /// <summary>
        ///     Property for handling the procedurelist.
        /// </summary>
        public PLSQLSpecParsedProcedureCollection Procedures
        {
            get { return procedureCollection; }
        }

        /// <summary>
        ///     If there are any errors anywhere in parsing. Spec, procedures or any of the
        ///     parameters.
        /// </summary>
        public bool HasParseError { get; set; }

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

        /// <summary>
        ///     Constructor
        /// </summary>
        public PLSQLSpecParsed()
        {
            procedureCollection = new PLSQLSpecParsedProcedureCollection(this);
            PackageName = string.Empty;
            HasParseError = false;
        }

        public override string ToString()
        {
            string specString = string.Empty;

            specString += string.Format("{0,-26}{1}{2}", "Filename:", FileNameParsed, Environment.NewLine);
            specString += string.Format("{0,-26}{1:#######}{2}", "FileLength:", PackageLength, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "PackageName:", PackageName, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "PackageHash:", PackageHash, Environment.NewLine);
            specString += string.Format("{0,-26}{1}{2}", "AnyParseErrors:", HasParseError.ToString(), Environment.NewLine);

            if (HasParseError)
            {
                specString += string.Format("{0,-26}{1:###}{2}", "Parse Errors:", Procedures.CountErrors, Environment.NewLine);
            }

            specString += string.Format("{0,-26}{1:###}/{2:###}{3}", "Procedures (Valid/Total):", Procedures.Count - Procedures.CountErrors, Procedures.Count, Environment.NewLine);

            specString += Procedures.ToString();

            return specString;
        }
    }
}
