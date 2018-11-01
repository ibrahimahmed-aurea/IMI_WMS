using System;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Imi.Framework.Job;
using Imi.SupplyChain.Server.Job.StandardJob;
using Imi.SupplyChain.Server.Job.MailAgent.PLSQLInterface;
using Imi.SupplyChain.Server.Job.MailAgent.Config;
using Imi.Framework.Job.Data;

namespace Imi.SupplyChain.Server.Job.MailAgent
{
    class MailAgent : OracleJob
    {
        private DbMailAgent dbMailAgent;

        public MailAgent(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
        }

        private ArrayList FindMessagesToSend()
        {
            ArrayList messageList = new ArrayList();

            IDataReader r = dbMailAgent.GetMAILBOXCur();

            try
            {
                int cnt = 0;

                while (r.Read())
                {
                    MailBoxMessage m = new MailBoxMessage();

                    // Build MailMessage object

                    if (r["MAILID"] == DBNull.Value)
                        m.MailId = "";
                    else
                        m.MailId = r["MAILID"] as String;

                    if (r["MAILMESSAGE"] == DBNull.Value)
                        m.XmlMessageBody = "";
                    else
                        m.XmlMessageBody = r["MAILMESSAGE"] as String;

                    messageList.Add(m);
                    cnt++;
                }
                Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                   String.Format("Found {0} email messages in queue", cnt));
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return (messageList);
        }

        private bool CheckParameters()
        {
            // Check config
            if (args["MailServer"] == null)
            {
                Tracing.TraceEvent(TraceEventType.Critical, 0,
                                   "Parameter MailServer is not configured, no operation is possible. Exiting.");
                return false;
            }

            if (args["ReturnAddress"] == null)
            {
                Tracing.TraceEvent(TraceEventType.Critical, 0,
                                   "Parameter ReturnAddress is not configured, no operation is possible. Exiting.");
                return false;
            }
            
            return true;
        }


        protected override void CreateProcedure(IDbConnectionProvider connectionProvider)
        {
            dbMailAgent = new DbMailAgent(connectionProvider);
        }

        protected override void ExecuteProcedure(Imi.Framework.Job.JobArgumentCollection args)
        {
            if (!CheckParameters())
                return;

            // Find msg
            ArrayList transList = FindMessagesToSend();

            foreach (MailBoxMessage m in transList)
            {
                Tracing.TraceEvent(TraceEventType.Verbose, 0, String.Format(" found MailId {0}", m.MailId));

                string error = "";

                try
                {
                    AppMailMessage aMsg = new AppMailMessage(args["ReturnAddress"] as string, m.XmlMessageBody);

                    try
                    {
                        using (MailMessage eMail = new MailMessage())
                        {
                            if (args["FromAddress"] == null)
                            {
                                eMail.From = new MailAddress(args["Job"] as string);
                            }
                            else
                            {
                                eMail.From = new MailAddress(args["FromAddress"] as string, args["Job"] as string);
                            }                            

                            foreach (string address in aMsg.ToList)
                            {                                
                                eMail.To.Add(new MailAddress(address));
                            }


                            eMail.Subject = aMsg.Subject;
                            eMail.Body = aMsg.MessageBody;

                            SmtpClient sc = new SmtpClient(args["MailServer"] as string);
                            
                            if (args.ContainsKey("MailServerPort"))
                            {
                                if ((args["MailServerPort"] != null) && (!args["MailServerPort"].Equals("")))
                                {
                                    sc.Port = Convert.ToInt32(args["MailServerPort"] as string);
                                }
                            }

                            if ((args.ContainsKey("MailServerUser")) && (args.ContainsKey("MailServerPassword")))
                            {
                                if (((args["MailServerUser"] != null) && (!args["MailServerUser"].Equals(""))) && ((args["MailServerPassword"] != null) && (!args["MailServerPassword"].Equals(""))))
                                {
                                    sc.UseDefaultCredentials = false;

                                    sc.Credentials = new System.Net.NetworkCredential(args["MailServerUser"] as string, args["MailServerPassword"] as string);
                                }
                                else
                                {
                                    sc.UseDefaultCredentials = true;
                                }
                            }
                            else
                            {
                                sc.UseDefaultCredentials = true;
                            }

                            sc.Send(eMail);
                        }
                    }
                    catch (FormatException fe)
                    {
                        error = String.Format("Problems sending the email due to faulty email address list {0}. {1} {2}\n{3}",
                          aMsg.ToList,
                          fe.GetType().Name,
                          fe.Message,
                          fe.StackTrace);

                        Tracing.TraceEvent(TraceEventType.Error, 0, error);
                    }
                    catch (SmtpException e)
                    {
                        error = String.Format("Problems sending the email. {0} {1}\n{2}",
                          e.GetType().Name,
                          e.Message,
                          e.StackTrace);

                        Tracing.TraceEvent(TraceEventType.Error, 0, error);
                    }

                }
                catch (ConfigurationErrorsException e)
                {
                    error = String.Format("Problems decoding email. {0} {1}\n{2}",
                      e.GetType().Name,
                      e.Message,
                      e.StackTrace);

                    Tracing.TraceEvent(TraceEventType.Error, 0, error);
                }

                // -------------------------------------------
                // --- Update MAILBOX record
                // -------------------------------------------
                string ALMID_W = "";
                StartTransaction();

                if (error == "")
                    dbMailAgent.ModifySend(m.MailId, ref ALMID_W);
                else
                    dbMailAgent.ModifyError(m.MailId, error, ref ALMID_W);

                Commit();

                #region TracingOfResult
                if (error != "")
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                         String.Format("MAILID {0} error while sending.\n{1}.", m.MailId, error));
                }
                else
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0,
                                       String.Format("MAILID {0} was sent successfully.", m.MailId));
                }

                if (ALMID_W != "")
                {
                    Tracing.TraceEvent(TraceEventType.Error, 0,
                                       String.Format("Error {0} when updating MAILBOX record status.", ALMID_W));
                }
                #endregion

            }  // foreach ( MailMessage t in transList )

        }

        protected override void CancelProcedure()
        {
            if (dbMailAgent != null)
            {
                dbMailAgent.Cancel();
            }
        }
    }
}
