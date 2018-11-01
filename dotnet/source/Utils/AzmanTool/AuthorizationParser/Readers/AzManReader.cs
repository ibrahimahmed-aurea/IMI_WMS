using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Interop.Security.AzRoles;
using System.Reflection;
using System.IO;
using AuthorizationParser.Writers;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AuthorizationParser.Readers
{
    public static class AzManReader
    {
        /// <summary>
        /// Read all roles for this specific application
        /// </summary>
        /// <param name="aStoreName"></param>
        /// <param name="aPath"></param>
        /// <returns></returns>
        public static List<string> ReadRoles(string aStoreName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);

                List<string> roles = new List<string>();

                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    foreach (IAzRoleDefinition role in toApplication.RoleDefinitions)
                    {
                        if (role.Name.StartsWith("_"))
                        {
                            roles.Add(role.Name.Substring(1));
                        }
                        else if (role.Name.Equals("Administrator"))
                        {
                            roles.Add(role.Name);
                            AzManWriter.AddAdministrator(aStoreName);
                        }
                    }
                }
                return roles;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }

        public static List<string> ReadWinUsers(string aStoreName, string RoleName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);
                List<string> members = new List<string>();
                IAzRole iAzRole = null;
                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    if (RoleName.Equals("Administrator"))
                    {
                        iAzRole = toApplication.OpenRole(RoleName);
                    }
                    else
                    {
                        iAzRole = toApplication.OpenRole("_" + RoleName);
                    }
                    foreach (string member in iAzRole.MembersName)
                    {
                        members.Add(member);
                    }
                    return members;
                }
                return members;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }

        public static List<string> ReadOperations(string aStoreName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);

                List<string> operations = new List<string>();

                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    foreach (IAzOperation operation in toApplication.Operations)
                    {
                        if (!operations.Contains(operation.Name))
                        {
                            operations.Add(operation.Name);
                        }
                    }
                }
                return operations;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }
        /// <summary>
        /// Read all OK/Cancel operations. All roles will be given permission to these since they are useless without access to the dialog they belong to. 
        /// There is never a reason why access should be given to a dialog with the options OK/Cancel without having access to the OK/Cancel operations.
        /// </summary>
        /// <param name="aStoreName"></param>
        /// <param name="aPath"></param>
        /// <returns></returns>
        public static List<string> ReadOkCancelAndCloseOperations(string aStoreName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);

                List<string> operations = new List<string>();

                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    foreach (IAzOperation operation in toApplication.Operations)
                    {
                        if (!operations.Contains(operation.Name))
                        {
                            string description = operation.Description;
                            if (description.EndsWith(" - Cancel") || description.EndsWith(" - OK") || description.EndsWith(" - Close"))
                            {
                                operations.Add(operation.Name);
                            }
                        }
                    }
                }
                return operations;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }

        /// <summary>
        /// Reads all operations for a role.
        /// </summary>
        /// <param name="aStoreName"></param>
        /// <param name="aRole"></param>
        /// <param name="aPath"></param>
        /// <returns></returns>
        public static List<string> ReadOperationsRole(string aStoreName, string aRole)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);

                List<string> operations = new List<string>();

                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    foreach (IAzRoleDefinition role in toApplication.RoleDefinitions)
                    {
                        if (role.Name == "_" + aRole)
                        {
                            foreach (string roleOperation in role.Operations)
                            {
                                if (!operations.Contains(roleOperation))
                                {
                                    operations.Add(roleOperation);
                                }
                            }
                        }
                    }
                }
                return operations;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }

        public static List<string> ReadUserRoles(string aStoreName, string RoleName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = GetAuthStoreLocation(aStoreName);
                //0 = The authorization store is opened for use by the Update method and the AccessCheck method.
                store.Initialize(0, storeLocation, null);
                List<string> members = new List<string>();
                IAzRole iAzRole = null;
                foreach (IAzApplication3 toApplication in store.Applications)
                {
                    if (RoleName.Equals("Administrator"))
                    {
                        iAzRole = toApplication.OpenRole(RoleName);
                    }
                    else
                    {
                        iAzRole = toApplication.OpenRole("_" + RoleName);
                    }
                    foreach (string member in iAzRole.MembersName)
                    {
                        members.Add(member);
                    }
                    return members;
                }
                return members;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                return null;
            }
        }
        public static string GetAuthStoreLocation(string aStoreName)
        {
            //   string storeLocation = "msxml://{0}";
            string storeLocation;
            string executablePath = new System.IO.FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            //   storeLocation = string.Format(storeLocation, Path.Combine(executablePath, (aStoreName + "AuthStore.xml")));
            storeLocation = executablePath + "\\" + aStoreName + "AuthStore.xml";
            if (File.Exists(storeLocation))
            {
                storeLocation = storeLocation.Insert(0, "msxml://");
                return storeLocation;
            }
            else
            {
                MessageBox.Show(null, aStoreName + "AuthStore.xml not found!", "File not found");
                return null;
            }
        }
    }
}
