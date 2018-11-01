/**
 * Copyright (c) 2012, Yubico AB.  All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 *  CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 *  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 *  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 *  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS
 *  BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 *  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 *  ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 *  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
 *  THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 *  SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Imi.SupplyChain.Services.IdentityModel.Yubico
{
    public class YubicoValidate
    {
        public static YubicoResponse Validate(List<string> urls, string userAgent)
        {
            List<Task<YubicoResponse>> tasks = new List<Task<YubicoResponse>>();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            foreach (string url in urls)
            {
                Task<YubicoResponse> task = new Task<YubicoResponse>((o) =>
                    {
                        return Verify((string)o, userAgent);
                    }, url, cancellation.Token);

                tasks.Add(task);
                task.Start();
            }
            
            YubicoResponse response = null;
            AggregateException taskException = null;

            while (tasks.Count != 0)
            {
                int completed = Task.WaitAny(tasks.ToArray());
                Task<YubicoResponse> task = tasks[completed];
                tasks.Remove(task);

                try
                {
                    if (task.Result != null)
                    {
                        if (response == null || (task.Result.GetStatus() == YubicoResponseStatus.OK))
                        {
                            response = task.Result;
                        }

                        cancellation.Cancel();
                    }
                }
                catch (AggregateException ex)
                {
                    taskException = ex;
                }
            }

            if (response == null && taskException != null)
            {
                throw taskException.Flatten();
            }

            return response;
        }

        private static YubicoResponse Verify(string url, string userAgent)
        {
            LogEntry entry = new LogEntry();
            entry.Severity = TraceEventType.Verbose;
            entry.Priority = -1;

            if (Logger.ShouldLog(entry))
            {
                string serverName = null;

                int pos = url.IndexOf('?');
                
                if (pos > 0)
                {
                    serverName = url.Substring(0, pos);    
                }

                entry.Message = string.Format("Validating OTP against service: {0}...", serverName);
                Logger.Write(entry);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            if (userAgent == null)
            {
                request.UserAgent = System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName;
            }
            else
            {
                request.UserAgent = userAgent;
            }

            request.Timeout = 10000;
            
            HttpWebResponse rawResponse;
            
            rawResponse = (HttpWebResponse)request.GetResponse();
                        
            Stream dataStream = rawResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            YubicoResponse response = new YubicoResponseImpl(reader.ReadToEnd());
                                    
            return response;
        }
    }
}
