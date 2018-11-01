using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Imi.SupplyChain.Deployment
{
    public static class VersionHandler
    {
        public enum Part { Major, Minor, Build, Revision };

        // Pattern for fetching all parts of a version
        private const string VersionInfoPattern = @"(?n:" +
                                                    @"^" +
                                                     @"(?<major>\d+)\." +
                                                     @"(?<minor>\d+)\." +
                                                     @"(?<build>\d+)\." +
                                                     @"(?<revision>\d+)" +
                                                    @"$)";

        private const string Major = "major";
        private const string Minor = "minor";
        private const string Build = "build";
        private const string Revision = "revision";

        // Check if string contains a version D.D.D.D where D is one or more digits.
        public static bool IsVersion(string verifyString)
        {
            if (!string.IsNullOrEmpty(verifyString))
            {
                return Regex.Match(verifyString, @"(?n:^(\d+\.){3}\d+$)").Success;
            }
            return false;
        }

        public static int GetPart(string versionString, Part versionPart)
        {
            if (IsVersion(versionString))
            {
                Regex regEx = new Regex(VersionInfoPattern);
                MatchCollection matches = regEx.Matches(versionString);

                if (matches.Count == 1)
                {
                    switch (versionPart)
                    { 
                        case Part.Major: return GetNumber(matches[0].Groups[Major].Value);
                        case Part.Minor: return GetNumber(matches[0].Groups[Minor].Value);
                        case Part.Build: return GetNumber(matches[0].Groups[Build].Value);
                        case Part.Revision: return GetNumber(matches[0].Groups[Revision].Value);
                    }
                }
            }
            return -1;
        }

        private static int GetNumber(string numberString)
        {
            if (!string.IsNullOrEmpty(numberString))
            {
                return Convert.ToInt32(numberString);
            }
            return -1;
        }

        public static string Increase(string versionString, Part versionPart)
        {
            if (string.IsNullOrEmpty(versionString))
            {
                return "1.0.0.0";
            }

            if (IsVersion(versionString))
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    versionPart == Part.Major ? (GetPart(versionString, Part.Major) + 1).ToString() : (GetPart(versionString, Part.Major)).ToString(),
                    versionPart == Part.Minor ? (GetPart(versionString, Part.Minor) + 1).ToString() : (GetPart(versionString, Part.Minor)).ToString(),
                    versionPart == Part.Build ? (GetPart(versionString, Part.Build) + 1).ToString() : (GetPart(versionString, Part.Build)).ToString(),
                    versionPart == Part.Revision ? (GetPart(versionString, Part.Revision) + 1).ToString() : (GetPart(versionString, Part.Revision)).ToString());

            }
            return null;
        }

        /// <summary>
        ///     Compares version strings.
        /// </summary>
        /// <param name="v1">First version to compare.</param>
        /// <param name="v2">Second version to compare.</param>
        /// <returns>Returns 1 if v1 is greater than v2, -1 if v1 is less than v2 and 0 if they are equal.</returns>
        public static int VersionStringCompare(string v1, string v2)
        {
            if (IsVersion(v1) && !IsVersion(v2))
            {
                return 1;
            }
            else if (!IsVersion(v1) && IsVersion(v2))
            {
                return -1;
            }
            else if (!IsVersion(v1) && !IsVersion(v2))
            {
                return 0;
            }

            // Check Major versions
            if (GetPart(v1, Part.Major) > GetPart(v2, Part.Major))
                return 1;
            else if (GetPart(v1, Part.Major) < GetPart(v2, Part.Major))
                return -1;
            else
            {
                // Major versions is same, check minor version
                if (GetPart(v1, Part.Minor) > GetPart(v2, Part.Minor))
                    return 1;
                else if (GetPart(v1, Part.Minor) < GetPart(v2, Part.Minor))
                    return -1;
                else
                {
                    // Minor versions is same, check build version
                    if (GetPart(v1, Part.Build) > GetPart(v2, Part.Build))
                        return 1;
                    else if (GetPart(v1, Part.Build) < GetPart(v2, Part.Build))
                        return -1;
                    else
                    {
                        // Build versions is same, check revision version
                        if (GetPart(v1, Part.Revision) > GetPart(v2, Part.Revision))
                            return 1;
                        else if (GetPart(v1, Part.Revision) < GetPart(v2, Part.Revision))
                            return -1;
                        else
                        {
                            // Version number is the same.
                            return 0;
                        }
                    }
                }
            }
        }

    }
}
