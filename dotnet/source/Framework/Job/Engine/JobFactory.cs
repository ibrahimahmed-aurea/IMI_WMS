using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Imi.Framework.Job.Interfaces;
using Imi.Framework.Job;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job.Engine
{
    /// <summary>
    /// Summary description for JobFactory.
    /// </summary>
    internal class JobFactory
    {
        private static Dictionary<string, Type> typeCacheDictionary;

        public JobFactory()
        {
            typeCacheDictionary = new Dictionary<string, Type>();
            
            if (InstanceConfig.CurrentInstance == null)
            {
                throw new NullReferenceException("Instance configuration is not loaded.");
            }
        }

        private static void InitializeTracing(ManagedJob job, LogElement logElement)
        {
            if (logElement == null)
            {
                logElement = new LogElement();
                logElement.FileName = job.Name + ".log";
                logElement.LogLevel = InstanceConfig.CurrentInstance.Log.LogLevel;
                logElement.MaxLogSize = InstanceConfig.CurrentInstance.Log.MaxLogSize;
            }

            // Check if path includes a directory part of some sort
            if (!logElement.FileName.Contains(Path.DirectorySeparatorChar.ToString()))
            {
                // Append server directory
                logElement.FileName = Path.Combine(Path.GetDirectoryName(InstanceConfig.CurrentInstance.Log.FileName), logElement.FileName);
            }

            job.Tracing.Switch.Level = ConfigurationHelper.ConvertLogLevel(logElement.LogLevel);
            RollingFileTraceListener listener = new RollingFileTraceListener(logElement.FileName, logElement.MaxLogSize);
            //listener.TraceOutputOptions = TraceOptions.DateTime |TraceOptions.ThreadId; // todo fixme
            job.Tracing.Listeners.Add(listener);
        }

        private static Type LoadImplementationClass(string typeName)
        {
            string[] typeParts = typeName.Split(new Char[] { ',' }, 2);
            string   className    = typeParts[0];
            string   assemblyName = typeParts[1];

            if (typeCacheDictionary.ContainsKey(typeName))
                return typeCacheDictionary[typeName];

            Assembly assembly = Assembly.Load(assemblyName);

            Type type = assembly.GetType(className);

            if (type != null)
                typeCacheDictionary.Add(typeName, type);

            return (type);
        }

        public static List<ManagedJob> CreateJobs()
        {
            List<ManagedJob> jobCollection = new List<ManagedJob>();
            List<JobElement> jobList = ConfigurationHelper.GetEnabledJobs();

            foreach (JobElement job in jobList)
            {
                // Look for implementation for the Job
                Type jobImplClass = LoadImplementationClass(ConfigurationHelper.GetJobType(job));

                if (jobImplClass != null)
                {
                    JobArgumentCollection jac = ConfigurationHelper.GetJobArgumentCollection(job);

                    List<object> arguments = new List<object>();
                    arguments.Add(job.Name);
                    arguments.Add(job.WaitForEvent);
                    arguments.Add(jac);

                    // Create an instance of the job
                    ManagedJob managedJob = Activator.CreateInstance(jobImplClass, arguments.ToArray()) as ManagedJob;

                    ISpawn spawn = managedJob as ISpawn;

                    if (spawn != null)
                    {
                        ManagedJob[] spawnedJobs = spawn.SpawnJobs();

                        foreach (ManagedJob j in spawnedJobs)
                        {
                            job.Log.FileName = Path.Combine(Path.GetDirectoryName(job.Log.FileName), j.Name) + Path.GetExtension(job.Log.FileName);
                            InitializeTracing(j, job.Log);
                            jobCollection.Add(j);
                        }
                    }
                    else
                    {
                        InitializeTracing(managedJob, job.Log);
                        jobCollection.Add(managedJob);
                    }
                }
            }

            return jobCollection;
        }
        
        public ManagedSchedule[] CreateSchedules()
        {
            List<ManagedSchedule> scheduleCollection = new List<ManagedSchedule>();
            List<JobElement> jobList = ConfigurationHelper.GetEnabledJobs();

            foreach (ScheduleElement scheduleType in InstanceConfig.CurrentInstance.ScheduleList)
            {
                ManagedSchedule managedSchedule = new ManagedSchedule();

                foreach (JobElement job in jobList)
                {
                    if (job.ScheduleName == scheduleType.Name)
                    {
                        managedSchedule.listeners.Add(job.Name);
                    }
                }

                // Only add schedule which is used
                if (managedSchedule.listeners.Count > 0)
                {
                    Schedule schedule = new Schedule(scheduleType.Name);
                    managedSchedule.Schedule = schedule;

                    foreach (RuleElement rule in scheduleType.RuleSet)
                    {
                        schedule.AddRule(rule.Second, rule.Minute, rule.Hour, rule.DayOfWeek, rule.DayOfMonth, rule.Month);
                    }

                    scheduleCollection.Add(managedSchedule);
                }
            }

            return (scheduleCollection.ToArray());
        }
        
        public Dictionary<string, JobArgumentCollection> CreateArgsForAllJobs()
        {
            Dictionary<string, JobArgumentCollection> argsDictionary = new Dictionary<string, JobArgumentCollection>();

            List<JobElement> jobList = ConfigurationHelper.GetEnabledJobs();

            foreach (JobElement job in jobList)
            {
              JobArgumentCollection jac = ConfigurationHelper.GetJobArgumentCollection(job);
                argsDictionary.Add(job.Name,jac);
            }

            return argsDictionary;

        }
    }
}
