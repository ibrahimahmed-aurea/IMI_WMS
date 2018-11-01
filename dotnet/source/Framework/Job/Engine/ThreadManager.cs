using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Job;
using Imi.Framework.Job.Interfaces;
using Imi.Framework.Job.Engine;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;


namespace Imi.Framework.Job.Engine
{
    public enum ThreadType { Unknown, System, Normal };
    public delegate void JournalEventDelegate(string name);
    
    public class ThreadManager
    {
        private static ThreadManager instance;
        private Dictionary<string, ManagedThread> threadDictionary;
        private object syncLock;
        private object syncLockAll;

        internal ThreadManager()
        {
            threadDictionary = new Dictionary<string, ManagedThread>();
            instance = this;
            syncLock = new object();
            syncLockAll = new object();
        }

        public Collection<ManagedThread> GetThreads()
        {
            Collection<ManagedThread> threadCollection = new Collection<ManagedThread>();

            foreach (ManagedThread thread in threadDictionary.Values)
            {
                threadCollection.Add(thread);
            }

            return threadCollection;

        }

        public ManagedThread CreateThread(ManagedJob job, bool signal, bool wait)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = new ManagedThread(job, ThreadType.Normal, signal, wait);
                threadDictionary.Add(job.Name, managedThread);
                return managedThread;
            }
        }

        public ManagedThread CreateSystemThread(ManagedJob job, bool signal, bool wait)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = new ManagedThread(job, ThreadType.System, signal, wait);
                threadDictionary.Add(job.Name, managedThread);
                return managedThread;
            }
        }
        
        public void RemoveThread(string name)
        {
            lock (syncLock)
            {
                threadDictionary.Remove(name);
            }
        }

        public bool StartThread(string name)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[name];

                if (managedThread != null)
                    return managedThread.Start();
            }

            return false;
        }

        public void StopThread(string name)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[name];

                if (managedThread != null)
                {
                    ThreadPool.QueueUserWorkItem(StopThreadCallback, managedThread);
                }
            }
        }

        private void StopThreadCallback(object state)
        {
            ManagedThread managedThread = (ManagedThread)state;

            if (managedThread != null)
                managedThread.Stop();
        }
        
        public void StartAll(bool includeSystem)
        {
            lock (syncLockAll)
            {
                foreach (ManagedThread thread in threadDictionary.Values)
                {
                    if ((thread.IsSystemThread) && (!includeSystem))
                        continue;
                    
                    thread.Start();
                }
            }
        }

        public void StopAll(bool includeSystem)
        {
            lock (syncLockAll)
            {
                foreach (ManagedThread managedThread in threadDictionary.Values)
                {
                    if ((managedThread.IsSystemThread) && (!includeSystem))
                        continue;
                
                    StopThread(managedThread.Name);
                }
            }
        }
        
        public void SetArguments(string jobName, JobArgumentCollection args)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[jobName];

                if (managedThread != null)
                    managedThread.SetArguments(args);
            }
        }

        public JobArgumentCollection GetArguments(string jobName)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[jobName];

                if (managedThread != null)
                    return managedThread.GetArguments();
                else
                    return null;
            }
        }

        public void UpdateArgumentsForAllJobs(Dictionary<string, JobArgumentCollection> argsDictionary)
        {
            lock (syncLock)
            {
                foreach (string name in argsDictionary.Keys)
                {
                    ManagedThread managedThread = threadDictionary[name];

                    if (managedThread != null)
                        managedThread.SetArguments(argsDictionary[name]);
                }
            }
        }

        public void SetTraceLevel(string jobName, string level)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[jobName];

                if (managedThread != null)
                    managedThread.TraceLevel = (SourceLevels)Enum.Parse(typeof(SourceLevels), level, false);
            }
        }

        public string GetTraceLevel(string jobName)
        {
            lock (syncLock)
            {
                ManagedThread managedThread = threadDictionary[jobName];

                if (managedThread != null)
                    return managedThread.TraceLevel.ToString();
                else
                    return null;
            }
        }

    }
}
