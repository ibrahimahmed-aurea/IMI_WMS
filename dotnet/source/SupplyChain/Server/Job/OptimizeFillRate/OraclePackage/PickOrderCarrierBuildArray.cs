#define ODP_NET
using System;
using System.Data;
#if ODP_NET
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
#else
using System.Data.OracleClient;
#endif
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.OptimizeFillRate
{
    //Overloaded to use ODP.NET array binding for better performance
    public partial class Pickordercarrierbuild
    {

        public void ConnectGroupRowToCarrier(string[] PBROWID_I,
                                             string[] PBROWGRP_ID_I,
                                             string[] ROWSPLIT_ID_I,
                                             Nullable<double>[] ORDQTY_I,
                                             string[] PBCARID_VIRTUAL_I,
                                             string[] CARTYPID_I)
        {
            if (this.sp_ConnectGroupRowToCarrier == null)
                CreateSP_ConnectGroupRowToCarrier();

            sp_ConnectGroupRowToCarrier.Transaction = connectionProvider.CurrentTransaction;
            sp_ConnectGroupRowToCarrier.Parameters.Clear();

            ((OracleCommand)sp_ConnectGroupRowToCarrier).ArrayBindCount = PBROWID_I.Length;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("PBROWID_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = PBROWID_I;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("PBROWGRP_ID_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = PBROWGRP_ID_I;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("ROWSPLIT_ID_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = ROWSPLIT_ID_I;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("ORDQTY_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = ORDQTY_I;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("PBCARID_VIRTUAL_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = PBCARID_VIRTUAL_I;
            ((OracleCommand)sp_ConnectGroupRowToCarrier).Parameters.Add("CARTYPID_I", OracleDbType.Varchar2, ParameterDirection.Input).Value = CARTYPID_I;
                        
            // Execute stored procedure

            sp_ConnectGroupRowToCarrier.Prepare();


            //Added retry loop to handle deadlocks in PL code.
            //The retry loop will try for 5 minutes to call the procedure repeatedly when running in to deadlocks.
            bool run = true;
            DateTime starttime = DateTime.Now;

            while (run)
            {
                try
                {
                    sp_ConnectGroupRowToCarrier.ExecuteNonQuery();
                    run = false;
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 60) //DeadLock try again
                    {
                        if (new TimeSpan(DateTime.Now.Ticks - starttime.Ticks).Minutes >= 5)
                        {
                            run = false;
                            throw;
                        }
                        else
                        {
                            run = true;
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        run = false;
                        throw;
                    }
                }
            }
        }
    }

}
