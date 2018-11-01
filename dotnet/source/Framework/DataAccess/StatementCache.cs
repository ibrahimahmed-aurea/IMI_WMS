using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Imi.Framework.DataAccess
{
    public class StatementCache
    {
        private class StatementCacheInstance
        {
            static StatementCacheInstance()
            {

            }

            internal static readonly StatementCache instance = new StatementCache();
        }

        private Dictionary<string, string> statementDictionary;
        private ReaderWriterLock syncLock;


        private StatementCache()
        {
            statementDictionary = new Dictionary<string, string>();
            syncLock = new ReaderWriterLock();
        }
                
        public static StatementCache Instance
        {
            get
            {
                return StatementCacheInstance.instance;
            }
        }

        public string GetCachedStatement(string statementName)
        {
            string statement = null;

            syncLock.AcquireReaderLock(-1);

            try
            {
                if (statementDictionary.ContainsKey(statementName))
                    statement = statementDictionary[statementName];
            }
            finally
            {
                syncLock.ReleaseReaderLock();
            }

            if (statement == null)
            {
                syncLock.AcquireWriterLock(-1);

                try
                {
                    if (statementDictionary.ContainsKey(statementName))
                    {
                        statement = statementDictionary[statementName];
                    }
                    else
                    {
                        
                        Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(statementName);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            statement = reader.ReadToEnd();
                        }
                                                                        
                        statementDictionary[statementName] = statement;
                    }
                }
                finally
                {
                    syncLock.ReleaseWriterLock();
                }
            }

            return statement;

        }
    }
}
