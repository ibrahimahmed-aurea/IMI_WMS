using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Imi.Framework.Job.Configuration;

namespace Imi.Framework.Job.Engine
{
    public interface IJournalManager
    {
        void JobStart(string name, int threadId);
        void JobStop(string name);
        void JobStopping(string name);
        void JobWait(string name);
        void JobRun(string name);
        void JobEndRun(string name, TimeSpan usedTime);
        JobRuntimeType[] GetJobs();
    }

    /// <summary>
    /// Summary description for JournalManager.
    /// </summary>
    public class JournalManager : IJournalManager
    {
        private Dictionary<string, JobRuntimeType> jobDictionary;

        public JournalManager()
        {
            jobDictionary = new Dictionary<string, JobRuntimeType>();
        }

        public JobRuntimeType[] GetJobs()
        {
            JobRuntimeType[] jobArray = new JobRuntimeType[jobDictionary.Count];
            jobDictionary.Values.CopyTo(jobArray, 0);
            return (jobArray);
        }

        public void AddJob(string name)
        {
            JobRuntimeType job = new JobRuntimeType();
            job.Name = name;
            job.Status = JobRuntimeTypeStatus.Stopped;
            jobDictionary.Add(name, job);
        }

        public void JobStart(string name, int threadId)
        {
            JobRuntimeType job = jobDictionary[name];
            if (job != null)
            {
                job.ThreadId = threadId;
                job.Status = JobRuntimeTypeStatus.Starting;
                job.Started = DateTime.Now;
                job.RunCount = 0;
                job.TotalRealTime = TimeSpan.Zero;
            }
        }

        public void JobStopping(string name)
        {
            JobRuntimeType job = jobDictionary[name];

            if (job != null)
            {
                job.Status = JobRuntimeTypeStatus.Stopping;
            }
        }

        public void JobStop(string name)
        {
            JobRuntimeType job = jobDictionary[name];
            
            if (job != null)
            {
                job.ThreadId = 0;
                job.Status = JobRuntimeTypeStatus.Stopped;
                job.Stopped = DateTime.Now;
            }
        }

        public void JobWait(string name)
        {
            JobRuntimeType job = jobDictionary[name];
            
            if (job != null)
            {
                job.Status = JobRuntimeTypeStatus.Wait;
            }
        }

        public void JobRun(string name)
        {
            JobRuntimeType job = jobDictionary[name];
            
            if (job != null)
            {
                job.Status = JobRuntimeTypeStatus.Run;
                job.RunStarted = DateTime.Now;
            }
        }

        public void JobEndRun(string name, TimeSpan usedTime)
        {
            JobRuntimeType job = jobDictionary[name];
            
            if (job != null)
            {
                job.RunCount++;
                TimeSpan totalTime = TimeSpan.Zero;

                if (job.TotalRealTime != null)
                {
                    totalTime = job.TotalRealTime;
                }

                job.TotalRealTime = totalTime.Add(usedTime);
            }
        }

    }
}
