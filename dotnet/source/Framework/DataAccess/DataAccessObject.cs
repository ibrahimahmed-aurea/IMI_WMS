using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Imi.Framework.DataAccess
{
    public class DataAccessObject : MarshalByRefObject
    {
        private string connectionString;
        
        protected DataAccessObject(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
                
        protected void LogDbCommand(IDbCommand command)
        { 
            LogEntry entry = new LogEntry();
            entry.Categories.Add("DataAccessLayer");
            entry.Priority = -1;

            if (Logger.ShouldLog(entry))
            {
                //Fix line breaks
                string commandText = command.CommandText.Replace("\r\n", "\n").Replace("\n", "\r\n");

                string message = string.Format("Executing command: {0}", commandText);

                Dictionary<string, object> properties = new Dictionary<string, object>();

                foreach (IDbDataParameter parameter in command.Parameters)
                {
                    string name = string.Format("Name={0} Type={1} Direction={2} Size={3} Scale={4} Precision={5}", parameter.ParameterName, parameter.DbType, parameter.Direction, parameter.Size, parameter.Scale, parameter.Precision);
                    object value = parameter.Value;
                    
                    if (value == null)
                        value = "<null>";
                    
                    properties.Add(name, value);
                }
                entry.Message = message;
                entry.Severity = TraceEventType.Verbose;
                entry.ExtendedProperties = properties;
                
                Logger.Write(entry);
            }
        }
                
    }
}
