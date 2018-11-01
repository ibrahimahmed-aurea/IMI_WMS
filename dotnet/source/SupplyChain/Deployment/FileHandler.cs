using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Reflection;

namespace Imi.SupplyChain.Deployment
{
    public static class FileHandler
    {
        static FileHandler() { }

        // Copy directory structure recursively
        public static bool CopyDirectory(string sourceDir, string destinationDir)
        {
            return CopyDirectoryRenameFiles(sourceDir, destinationDir, string.Empty, string.Empty);
        }

        // Rename file
        public static string RenameFile(string original, string searchRegEx, string replaceString)
        {
            return Regex.Replace(original, searchRegEx, replaceString);
        }

        // Change name of files recursively
        public static bool RenameFiles(string sourceDir, string searchPattern, string replaceString, string ignorePattern)
        {
            bool ok = true;
            String[] Files = Directory.GetFileSystemEntries(sourceDir);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    if (!RenameFiles(Element, searchPattern, replaceString, ignorePattern))
                    {
                        ok = false;
                    }
                }
                else
                {
                    // Check if this file should be ignored
                    if (!Regex.Match(Element, ignorePattern).Success)
                    {
                        try
                        {
                            // Rename the file by moving it
                            File.Move(Element, Path.Combine(Path.GetDirectoryName(Element), RenameFile(Path.GetFileName(Element), searchPattern, replaceString)));
                        }
                        catch
                        {
                            ok = false;
                        }
                    }
                }
            }
            return ok;
        }

        // Copy and change name of files to add a suffix recursively
        public static bool CopyDirectoryAddSuffix(string sourceDir, string destinationDir, string suffix)
        {
            return CopyDirectoryRenameFiles(sourceDir, destinationDir, @"$", suffix);
        }

        // Copy and change name of files to add a prefix recursively
        public static bool CopyDirectoryAddPrefix(string sourceDir, string destinationDir, string prefix)
        {
            return CopyDirectoryRenameFiles(sourceDir, destinationDir, @"^", prefix);
        }

        // Copy and change name of files recursively
        public static bool CopyDirectoryRenameFiles(string sourceDir, string destinationDir, string searchRegEx, string replaceString)
        {
            bool ok = true;

            String[] Files;

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            Files = Directory.GetFileSystemEntries(sourceDir);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    if (!CopyDirectoryRenameFiles(Element, Path.Combine(destinationDir, Path.GetFileName(Element)), searchRegEx, replaceString))
                    {
                        ok = false;
                    }
                }
                else
                {
                    try
                    {
                        // Check if no replacement string. In that case just do a normal copy of the file.
                        if (string.IsNullOrEmpty(replaceString))
                        {
                            string destinationFileName = Path.Combine(destinationDir, Path.GetFileName(Element));

                            CopyFileForced(Element, destinationFileName);
                        }
                        else
                        {
                            string destinationFileName = Path.Combine(destinationDir, RenameFile(Path.GetFileName(Element), searchRegEx, replaceString));

                            CopyFileForced(Element, destinationFileName);
                        }
                    }
                    catch
                    {
                        ok = false;
                    }
                }
            }

            return ok;
        }

        public static void CopyFileForced(string sourceFileName, string destinationFileName)
        {
            CopyFile(sourceFileName, destinationFileName, true, true);
        }

        public static bool CopyFile(string sourceFileName, string destinationFileName, bool overwrite, bool forceOverwrite)
        {
            if (Directory.Exists(destinationFileName))
                destinationFileName = Path.Combine(destinationFileName, Path.GetFileName(sourceFileName));

            if (File.Exists(destinationFileName))
            {
                if (IsReadOnly(destinationFileName))
                {
                    if (forceOverwrite)
                    {
                        RemoveReadOnlyAttribute(destinationFileName);
                        File.Copy(sourceFileName, destinationFileName, true);
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    if (overwrite)
                    {
                        File.Copy(sourceFileName, destinationFileName, true);
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
            {
                File.Copy(sourceFileName, destinationFileName, true);
                return true;
            }
        }

        private static bool IsReadOnly(string fileName)
        {
            return (File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        private static void RemoveReadOnlyAttribute(string fileName)
        {
            // Check if destinationfile is readonly, in that case remove the readonly flag.
            if (IsReadOnly(fileName))
            {
                File.SetAttributes(fileName, File.GetAttributes(fileName) ^ FileAttributes.ReadOnly);
            }
        }

        // Remove readonly attribute for a whole directory
        public static void RemoveReadOnlyAttribute(string sourceDir, bool recursive)
        {
            String[] Files;

            Files = Directory.GetFileSystemEntries(sourceDir);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    if (recursive)
                        RemoveReadOnlyAttribute(Element, recursive);
                }
                else
                {
                    RemoveReadOnlyAttribute(Element);
                }
            }
        }

        public static string GetNewInstallDirectory(string tryDirectoryName, string installPath)
        {
            bool uniqueDirFound = false;

            // Remove all characters that aren't a-z or A-Z including whitespaces and other none
            // word characters.
            string uniqueDir = Regex.Replace(tryDirectoryName, @"[^a-zA-Z]", string.Empty); 

            do
            {
                if (Directory.Exists(Path.Combine(installPath, uniqueDir)))
                {
                    int counter = 1;

                    // Get last digits if any exists
                    Match counterMatch = Regex.Match(uniqueDir, @"(\d+)$");

                    if (counterMatch.Success)
                    {
                        uniqueDir = uniqueDir.Substring(0, counterMatch.Index);
                        int.TryParse(counterMatch.Groups[1].Value, out counter);
                        counter++;
                    }

                    uniqueDir += counter.ToString();
                }
                else
                    uniqueDirFound = true;
            }
            while (!uniqueDirFound);

            return Path.Combine(installPath, uniqueDir);
        }

        // Adds an ACL entry on the specified file for the specified account.
        public static bool AddFileSecurity(string fileName, string accountName, FileSystemRights rights, AccessControlType controlType)
        {
            // Cannot add filesecurity if account doesn't exist
            if (!string.IsNullOrEmpty(accountName) && 
                AccessControlList.AccountExist(accountName))
            {
                // Get a FileSecurity object that represents the 
                // current security settings.
                FileSecurity fSecurity = File.GetAccessControl(fileName);

                // Add the FileSystemAccessRule to the security settings. 
                fSecurity.AddAccessRule(new FileSystemAccessRule(accountName, rights, controlType));

                // Set the new access settings.
                File.SetAccessControl(fileName, fSecurity);

                return true;
            }
            else
                return false;
        }

        // Adds an ACL entry on the specified file for the specified account.
        public static bool RemoveFileSecurity(string fileName, string accountName)
        {
            // Cannot remove filesecurity if account doesn't exist
            if (!string.IsNullOrEmpty(accountName) &&
                AccessControlList.AccountExist(accountName))
            {
                SecurityIdentifier sid = AccessControlList.GetAccount(accountName);

                // Get a FileSecurity object that represents the 
                // current security settings.
                FileSecurity fSecurity = File.GetAccessControl(fileName);

                // Remove the FileSystemAccessRule from the security settings.
                fSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(accountName, FileSystemRights.ReadAndExecute, AccessControlType.Allow));

                // Set the new access settings.
                File.SetAccessControl(fileName, fSecurity);

                return true;
            }
            else
                return false;
        }

        // Assembly.GetExecutingAssembly();
        public static bool GetFileFromAssembly(string toFile, Assembly fromAssembly, string nameOfManifestResource)
        {
            try
            {
                if (fromAssembly == null)
                    return false;

                // Get the stream for the resource
                using (Stream fromStream = fromAssembly.GetManifestResourceStream(nameOfManifestResource))
                {
                    // Create / overwrite the file to write to
                    using (Stream toStream = File.Create(toFile))
                    {
                        // Create reader and writer
                        BinaryReader reader = new BinaryReader(fromStream);
                        BinaryWriter writer = new BinaryWriter(toStream);

                        // Copy all data from the fromStream to the toStream.
                        writer.Write(reader.ReadBytes((int)fromStream.Length));

                        // Save
                        writer.Flush();

                        // Close
                        writer.Close();
                        reader.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
            return true;
        }

    }
}
