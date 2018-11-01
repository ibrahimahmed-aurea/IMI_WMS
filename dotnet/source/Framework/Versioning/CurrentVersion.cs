using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Framework.Versioning
{
    /// <summary>
    /// Represents the current system version.
    /// </summary>
    public sealed class CurrentVersion
    {
        /// <summary>
        /// The current version string of the system.
        /// </summary>
        private const string VersionNumber = "8.0.1";

        /// <summary>
        /// The current version string of the system.
        /// </summary>
        public const string AssemblyVersion = VersionNumber + ".0";

        /// <summary>
        /// The current version string of the system.
        /// </summary>
        public const string AssemblyFileVersion = VersionNumber + ".0";

        /// <summary>
        /// The name of the current system version.
        /// </summary>
        public const string VersionName = VersionNumber;
    }
}