using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Cdc.CodeGeneration.Caching
{
    public class CacheEntry
    {
        public string FileName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Hash { get; set; }
        public Guid Guid { get; set; }
    }

    public class FileCacheManager
    {
        private static bool cacheNeedsSaving;
        private static int fileWrites;
        private static int fileWritesToDisk;
        private static Dictionary<string, List<string>> extensionDictionary;
        private static Dictionary<string, CacheEntry> fileCache;
        private static Dictionary<string, string> usingCache;
        private static List<string> filesWrittenToDisk;
        private static List<string> filesNotSaved;
        private static string CacheFileName;

        // load cache file
        public static void Open(string directory)
        {
            usingCache = new Dictionary<string, string>();
            extensionDictionary = new Dictionary<string, List<string>>();
            fileCache = new Dictionary<string, CacheEntry>();
            filesWrittenToDisk = new List<string>();
            filesNotSaved = new List<string>();
            fileWrites = 0;
            fileWritesToDisk = 0;
            CacheFileName = Path.Combine(directory, "fileCache.xml");

            FileStream cf = null;

            try
            {
                try
                {
                    if (File.Exists(CacheFileName))
                    {
                        cf = new FileStream(CacheFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                        XmlSerializer ser = new XmlSerializer(typeof(List<CacheEntry>));
                        try
                        {
                            List<CacheEntry> l = ser.Deserialize(cf) as List<CacheEntry>;

                            fileCache.Clear();

                            foreach (CacheEntry e in l)
                            {
                                fileCache.Add(e.FileName, e);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(directory);
                }
            }
            finally
            {
                if (cf != null)
                {
                    cf.Close();
                }
            }
        }

        // Commits new checklist to disk
        public static void Commit()
        {
            if (cacheNeedsSaving)
            {
                FileStream cf = null;

                try
                {
                    cf = new FileStream(CacheFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    XmlSerializer ser = new XmlSerializer(typeof(List<CacheEntry>));
                    List<CacheEntry> l = new List<CacheEntry>(fileCache.Values);
                    ser.Serialize(cf, l);
                }
                finally
                {
                    if (cf != null)
                    {
                        cf.Close();
                    }

                }
            }

            extensionDictionary.Clear();
            extensionDictionary = null;
            usingCache.Clear();
            usingCache = null;
            fileCache.Clear();
            fileCache = null;

        }

        public static Guid GetGuidForFile(string fileName)
        {
            if (!fileCache.ContainsKey(fileName))
            {
                fileCache[fileName] = new CacheEntry() { FileName = fileName, Guid = Guid.NewGuid() };
            }
            
            CacheEntry e = fileCache[fileName];

            if (e.Guid == null)
            {
                e.Guid = Guid.NewGuid();
            }

            return e.Guid;
        }

        private static void AddFileToDictionary(string fileName)
        {
            string directory = Path.GetDirectoryName(fileName);

            string[] directories = directory.Split(Path.DirectorySeparatorChar);
            string path = "";
            foreach (string d in directories)
            {
                path += d;
                // Keep files organized by extension
                if (!extensionDictionary.ContainsKey(path))
                {
                    extensionDictionary[path] = new List<string>();
                }

                extensionDictionary[path].Add(fileName);

                path += Path.DirectorySeparatorChar;
            }
        }


        public static void WriteFile(string fileName, string contents, FileMode fileMode)
        {
            WriteFile(fileName, contents, fileMode, Encoding.UTF8);
        }

        public static string GetCachedUsingStatement(string fileName)
        {
            if (usingCache.ContainsKey(fileName))
            {
                return usingCache[fileName];
            }
            else
            {
                return null;
            }
        }

        public static void AddExistingFileToCache(string fileName)
        {
            string contents = File.ReadAllText(fileName);

            if (fileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
            {
                CacheUsingStatements(fileName, contents);
            }

            AddFileToDictionary(fileName);
        }

        // Check if we need to write to disk
        public static void WriteFile(string fileName, string contents, FileMode fileMode, Encoding encoding)
        {
            if (fileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
            {
                CacheUsingStatements(fileName, contents);
            }

            AddFileToDictionary(fileName);

            fileWrites++;

            string newFileHash = Hashing.Get(HashTypes.MD5, contents);
            string diskFileHash = "";

            CacheEntry e = null;

            // check cache
            if (fileCache.ContainsKey(fileName))
            {
                e = fileCache[fileName];

                DateTime timeOnDisk = File.GetLastWriteTime(fileName);

                if (!(timeOnDisk.Year < 2000)) // File found ?
                {
                    if (e.LastWriteTime != timeOnDisk)
                    {
                        // do new hash of file on disk and update cache
                        string diskContents = File.ReadAllText(fileName, encoding);
                        diskFileHash = Hashing.Get(HashTypes.MD5, diskContents);
                        e.Hash = diskFileHash;
                        e.LastWriteTime = timeOnDisk;
                    }
                    else
                    {
                        // use cached hash
                        diskFileHash = e.Hash;
                    }
                }
            }
            // We don't have file on cache, but does it already exist anyway?
            else if (File.Exists(fileName))
            {
                // Read info from file and create a new cache entry
                string diskContents = File.ReadAllText(fileName, encoding);
                diskFileHash = Hashing.Get(HashTypes.MD5, diskContents);

                e = new CacheEntry();
                e.FileName = fileName;
                e.Hash = diskFileHash;
                e.LastWriteTime = File.GetLastWriteTime(fileName);

                fileCache[fileName] = e;
                cacheNeedsSaving = true;
            }

            if (newFileHash != diskFileHash)
            {
                // write to disk
                bool goOn = false;

                do
                {
                    try
                    {
                        if (fileMode == FileMode.Create)
                            File.WriteAllText(fileName, contents, encoding);
                        else
                            File.AppendAllText(fileName, contents, encoding);

                        goOn = false;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        try
                        {
                            // Set attributes to Archive because file is readonly
                            File.SetAttributes(fileName, FileAttributes.Archive);
                        }
                        catch
                        {
                            // If we get another error here it's no possible to save
                            // so we have to skip it.
                            // This is usually the case when trying to change a file
                            // in a dynamic view in clearcase for example.
                            filesNotSaved.Add(fileName);
                            return;
                        }
                        goOn = !(goOn);
                    }
                    catch
                    {
                        // If we get another error here it's no possible to save
                        // so we have to skip it.
                        // This is for all unknown errors.
                        filesNotSaved.Add(fileName);
                        return;
                    }
                }
                while (goOn);

                // Do it this way to preserve any guids that have been generated
                if(e == null) 
                {
                   e = new CacheEntry();
                }

                e.FileName = fileName;
                e.Hash = newFileHash;
                e.LastWriteTime = File.GetLastWriteTime(fileName);

                fileCache[fileName] = e;
                cacheNeedsSaving = true;
                fileWritesToDisk++;
                filesWrittenToDisk.Add(fileName);
            }
        }

        private static void CacheUsingStatements(string fileName, string contents)
        {
            // Quick extract of using section

            int pos = contents.IndexOf("namespace");

            if(pos < 1)
            {
                pos = contents.IndexOf("public class");
            }

            if(pos > 0)
            {
                if (usingCache != null)
                {
                    usingCache[fileName] = contents.Substring(0, pos);
                }
            }
        }

        // return list of files generated in this session for a specific directory
        public static IEnumerable<string> GetSessionFilesInDirectory(string directory, string extension)
        {
            if (extensionDictionary.ContainsKey(directory))
            {
                var fileList = extensionDictionary[directory];
                
                IEnumerable<string> dirFiles = from string fileName in fileList
                                               where Path.GetExtension(fileName).ToLower() == extension.ToLower()
                                               select fileName;

                return dirFiles;
            }
            else
            {
                return new List<string>();
            }
        }
                
        public static int GetWrites()
        {
            return fileWrites;
        }

        public static int GetWritesToDisk()
        {
            return fileWritesToDisk;
        }

        public static IList<string> GetWrittenFilesToDisk()
        {
            return filesWrittenToDisk.OrderBy(f => f).ToList();
        }

        public static IList<string> GetFilesNotSaved()
        {
            return filesNotSaved.OrderBy(f => f).ToList();
        }
    }
}
