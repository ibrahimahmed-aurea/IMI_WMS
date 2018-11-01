using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Job.Configuration;
using System.Collections;
using System.Diagnostics;

namespace Imi.Framework.Job.Engine
{
    public class ConfigurationHelper
    {

        public static List<JobElement> GetEnabledJobs()
        {
            ServerInstanceSection config = InstanceConfig.CurrentInstance;
            List<JobElement> l = new List<JobElement>();

            Dictionary<string, JobTypeElement> t = new Dictionary<string, JobTypeElement>();
            foreach (JobTypeElement jt in config.JobTypeList)
                t.Add(jt.Name, jt);

            
            foreach (JobElement j in config.JobList)
            {
                if (j.Enabled)
                {
                    if (t.ContainsKey(j.Implementation.Name))
                    {
                        l.Add(j);
                    }
                }
            }

            return (l);
        }

        public static string GetSystemId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public static string GetJobType(JobElement job)
        {
            foreach(JobTypeElement jt in InstanceConfig.CurrentInstance.JobTypeList) {
                if (jt.Name == job.Implementation.Name)	{
                    return (jt.Type);
                }
            }
            return (null);
        }

        public static JobArgumentCollection GetJobArgumentCollection(JobElement job)
        {
            JobArgumentCollection c = new JobArgumentCollection();

            // Add job name always
            c.Add("Job", job.Name + '@' + InstanceConfig.CurrentInstance.SystemId);

            foreach(ParameterElement p in job.Implementation) {
                c.Add(p.Name ,p.Value);
            }

            return c;
        }

        public static SourceLevels ConvertLogLevel(LogTypeLoglevel logLevel)
        {
            switch (logLevel)
            {
                case LogTypeLoglevel.Error:
                    return SourceLevels.Error;

                case LogTypeLoglevel.Information:
                    return SourceLevels.Information;

                case LogTypeLoglevel.Off:
                    return SourceLevels.Off;

                case LogTypeLoglevel.Verbose:
                    return SourceLevels.Verbose;

                case LogTypeLoglevel.Warning:
                    return SourceLevels.Warning;

                default:
                    return SourceLevels.Error;
            }
        }
    }
}
