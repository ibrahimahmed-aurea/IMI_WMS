using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using Imi.SupplyChain.Server.Job.StandardJob.OraclePackage;


namespace Imi.SupplyChain.Server.Job.StandardJob
{
    public class StandardOracleJob : OracleJob
    {
        protected DbJob dbJob;

        public StandardOracleJob(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        protected override void CreateProcedure(IDbConnectionProvider connection)
        {
            dbJob = new DbJob(connection);
        }

        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            //
            // Execute stored procedure
            //
            Array eventArr = null;

            StartTransaction();

            dbJob.Execute(args, ref eventArr);

            Commit();

            //
            // Handle Eventlist
            //
            SignalEvents(eventArr);
        }

        protected override void CancelProcedure()
        {
            if (dbJob != null)
            {
                dbJob.Cancel();
            }

        }
    }
}
