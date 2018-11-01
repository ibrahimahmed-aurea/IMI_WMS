using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Interop.Security.AzRoles;
using AuthorizationParser.Readers;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using System.Runtime.InteropServices;
using AuthorizationParser.Models;

namespace AuthorizationParser.Writers
{
    public static class AzManWriter
    {
        public static void SaveRole(List<string> anOperations, string aRole, string aStoreName, List<string> allTreeOperations)
        {
            try
            {
                //Make sure all operations exist in Azman
                List<string> allOperations = AzManReader.ReadOperations(aStoreName);
                List<string> excludedOperations = new List<string>();
                if (allOperations != null)
                {
                    List<string> addOperations = new List<string>();
                    foreach (string operation in anOperations)
                    {
                        if (allOperations.Contains(operation))
                        {
                            addOperations.Add(operation);
                        }
                    }
                    foreach (string operation in allOperations)
                    {
                        if (!allTreeOperations.Contains(operation)) 
                        {
                            excludedOperations.Add(operation);

                        }
                    }

                    //read OK and Cancel operations
                    List<string> oKAndCancelOperations = AzManReader.ReadOkCancelAndCloseOperations(aStoreName);
                    if (oKAndCancelOperations != null)
                    {
                        foreach (string operation in allOperations) 
                        {
                            if (oKAndCancelOperations.Contains(operation))
                            {
                                excludedOperations.Remove(operation);

                            }
                        }
                        addOperations.AddRange(excludedOperations);

                        //save down the operations into the role in AuthStore
                        AzAuthorizationStore store = new AzAuthorizationStore();

                        string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                        string roleName = "_" + aRole;
                        //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                        store.Initialize(4, storeLocation, null);

                        foreach (IAzApplication3 application in store.Applications)
                        {
                            foreach (IAzRoleDefinition role in application.RoleDefinitions)
                            {
                                if (role.Name != roleName) continue;
                                //remove all existing operations in the role
                                foreach (string operation in role.Operations)
                                {
                                    role.DeleteOperation(operation);
                                }
                                //Role needs to be submitted after deleting operations otherwise Azman freaks out
                                role.Submit();
                                //Save all selected operations to the role
                                foreach (string operationString in addOperations)
                                {
                                    role.AddOperation(operationString);
                                }
                                foreach (string oKOrCancelOperation in oKAndCancelOperations)
                                {
                                    role.AddOperation(oKOrCancelOperation);
                                }
                                //Submit role so changes are saved
                                role.Submit();
                                MessageBox.Show("Setting for " + aRole + " has been saved.", "Role Settings Saved", MessageBoxButtons.OK);
                            }
                            //Submit everything just to be sure
                            application.Submit();
                        }
                        store.Submit();
                    }
                }
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied to " + aStoreName + "AuthStore.xml. Maybe it is read-only?", "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Failed to save configuration", "", MessageBoxButtons.OK);
                }
            }
        }

        public static bool CreateRole(string role, string aStoreName)
        {
            bool success = false;
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();

                string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                string roleName = "_" + role;
                //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                store.Initialize(4, storeLocation, null);
                foreach (IAzApplication3 application in store.Applications)
                {
                    //Create a new role definition
                    IAzRoleDefinition newRole = application.CreateRoleDefinition(roleName);
                    //Create a new role assignment
                    IAzRoleAssignment newRoleAssignment = application.CreateRoleAssignment(roleName);

                    newRole.Submit();
                    newRoleAssignment.AddRoleDefinition(roleName);
                    newRoleAssignment.Submit();
                    application.Submit();
                }
                success = true;
            }
            catch (COMException ce)
            {
                if (ce.ErrorCode.Equals(-2147024713))
                {
                    MessageBox.Show(null, "Role already exist in this application.", "Role already exist");
                }
                else
                {
                    MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
                }
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    MessageBox.Show("Access denied to " + aStoreName + "AuthStore.xml. Maybe it is read-only?", "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Could not create role. Maybe it already exists?", "", MessageBoxButtons.OK);
                }
                success = false;
            }
            return success;
        }

        public static bool AddWindowsUserToRole(string role, string aStoreName, string windowsUser)
        {
            bool success = false;
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();

                string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                if (role != "Administrator")
                {
                    role = "_" + role;
                }
                //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                store.Initialize(4, storeLocation, null);
                foreach (IAzApplication3 application in store.Applications)
                {
                    IAzRole iAzRole = application.OpenRole(role);
                    iAzRole.AddMemberName(windowsUser);
                    iAzRole.Submit();
                    application.Submit();
                }
                success = true;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public static bool DeleteWindowsUserFromRole(string role, string aStoreName, string windowsUser)
        {
            bool success;
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();

                string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                if (role != "Administrator")
                {
                    role = "_" + role;
                }
                //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                store.Initialize(4, storeLocation, null);
                foreach (IAzApplication3 application in store.Applications)
                {
                    IAzRole iAzRole = application.OpenRole(role);
                    iAzRole.DeleteMemberName(windowsUser);
                    iAzRole.Submit();
                    application.Submit();
                }
                success = true;
            }

            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public static bool DeleteRole(string deleteRole, string aStoreName)
        {
            bool success = false;
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();

                string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                string roleName = "_" + deleteRole;
                //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                store.Initialize(4, storeLocation, null);
                foreach (IAzApplication3 application in store.Applications)
                {
                    //Delete role assignment
                    application.DeleteRoleAssignment(roleName);
                    //Delete role definition
                    application.DeleteRoleDefinition(roleName);
                    application.Submit();
                }
                success = true;
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        internal static void AddAdministrator(string aStoreName)
        {
            try
            {
                AzAuthorizationStore store = new AzAuthorizationStore();
                string storeLocation = AzManReader.GetAuthStoreLocation(aStoreName);
                //4 = AZ_AZSTORE_FLAG_BATCH_UPDATE
                store.Initialize(4, storeLocation, null);
                foreach (IAzApplication3 application in store.Applications)
                {
                    //Create a new role assignment
                    IAzRoleAssignments roleAssignments = application.RoleAssignments;
                    bool hasAdministrator = false;
                    foreach (IAzRoleAssignment roleassignment in roleAssignments)
                    {
                        if (roleassignment.Name.Equals("Administrator"))
                        {
                            hasAdministrator = true;
                        }
                    }
                    if (!hasAdministrator)
                    {
                        IAzRoleAssignment newRoleAssignment = application.CreateRoleAssignment("Administrator");
                        newRoleAssignment.AddRoleDefinition("Administrator");
                        newRoleAssignment.Submit();
                        application.Submit();
                    }
                }
            }
            catch (COMException ce)
            {
                MessageBox.Show(null, ce.Message + "\n" + ce.ErrorCode.ToString(), "COMException occurred");
            }
        }
    }
}