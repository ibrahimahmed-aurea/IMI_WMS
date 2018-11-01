using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthorizationParser.Utilities
{
    /// <summary>
    /// Class used to skip special operations such as the operation tied to "Change User Settings" which can't be locked.
    /// </summary>
    public static class IdentityExceptionHelper
    {
        //Dictionary containing actions that shouldn't and/or can't be handled by this program.
        private static Dictionary<string, string>_exceptions = new Dictionary<string, string> 
        {
            { "8a06b5d2-5862-452f-8d4c-e4c5dd35dd03", "Change User Settings"},
            { "Change User Settings", "Change User Settings"}
        };
        /// <summary>
        /// Method that checks if the key exists in Dictionary
        /// </summary>
        /// <param name="aKey">Key that will be checked against the Dictionary</param>
        /// <returns>True if the key is found in the Dictionary, else false</returns>
        public static bool Contains(string aKey)
        {
            return _exceptions.ContainsKey(aKey);
        }
    }
}
