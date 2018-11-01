using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Imi.SupplyChain.Deployment
{
    public static class VerifyString
    {
        static VerifyString() { }

        // Check if string contains only [a-zA-Z0-9_]
        public static bool IsWord(string verifyString)
        {
            return Regex.Match(verifyString, @"(?n:^\w+$)").Success;
        }

        // Check if string contains a version D.D.D.D where D is one or more digits.
        public static bool IsVersion(string verifyString)
        {
            return VersionHandler.IsVersion(verifyString);
        }

    }
}
