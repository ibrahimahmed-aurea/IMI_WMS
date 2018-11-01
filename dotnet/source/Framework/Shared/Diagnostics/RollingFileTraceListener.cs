using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Imi.Framework.Shared.Diagnostics
{
    /// <summary>
    /// Rolling file trace listener.
    /// </summary>
    public class RollingFileTraceListener : TextWriterTraceListener
    {
        private string logFilename;
        private ulong maxLength;
        private bool firstWrite;
        private StreamWriter rolledWriter;

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="RollingFileTraceListener"/> class.</para>
        /// </summary>
        /// <param name="logFilename">
        /// The filename to trace to.
        /// </param>
        /// <param name="maxLength">
        /// Maximum trace file size in bytes.
        /// </param>
        public RollingFileTraceListener(string logFilename, ulong maxLength) 
            : base(new StreamWriter(logFilename, true))
        {
            this.logFilename = logFilename;
            this.maxLength = maxLength;
            this.firstWrite = true;

            LockRolledFile();
        }

        public RollingFileTraceListener(string logFilename)
            : this(logFilename, 1048576)
        { 
        
        }

        /// <summary>Writes a message to this instance's <see cref="P:System.Diagnostics.TextWriterTraceListener.Writer"></see> followed by a line terminator. The default line terminator is a carriage return followed by a line feed (\r\n).</summary>
        /// <param name="message">A message to write. </param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override void WriteLine(string message)
        {
            lock (base.Writer)
            {
                RollFile();
                base.Writer.WriteLine(message);
                firstWrite = true;
            }
        }

        /// <summary>Writes a message to this instance's <see cref="P:System.Diagnostics.TextWriterTraceListener.Writer"></see>.</summary>
        /// <param name="message">A message to write. </param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override void Write(string message)
        {
            lock (base.Writer)
            {
                RollFile();

                if (firstWrite)
                {
                    base.Writer.Write("{0}: ",DateTime.Now.ToString());
                    firstWrite = false;
                }

                base.Write(message);
            }
        }

        /// <summary>Writes the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class, followed by a line terminator.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write. </param>
        /// <filterpriority>2</filterpriority>
        public override void WriteLine(object o)
        {
            if (o != null)
                WriteLine(o.ToString());
        }

        /// <summary>Writes the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write. </param>
        /// <filterpriority>2</filterpriority>
        public override void Write(object o)
        {
            if (o != null)
                Write(o.ToString());
        }
        
        private void RollFile()
        {
            if (maxLength > 0)
            {
                FileInfo info = new FileInfo(logFilename);

                if ((ulong)info.Length > (maxLength / 2))
                {
                    Writer.Close();

                    string rolledFilename = GetRolledFilename();

                    UnlockRolledFile();

                    if (File.Exists(rolledFilename))
                        File.Delete(rolledFilename);

                    File.Move(logFilename, rolledFilename);

                    LockRolledFile();

                    Writer = new StreamWriter(logFilename, false);
                }
            }
        }
        
        private string GetRolledFilename()
        {
            return Path.Combine(Path.GetDirectoryName(logFilename),
                Path.ChangeExtension(Path.GetFileNameWithoutExtension(logFilename) +
                "_", Path.GetExtension(logFilename)));
        }

        private void LockRolledFile()
        {
            rolledWriter = new StreamWriter(GetRolledFilename(), true);            
        }

        private void UnlockRolledFile()
        {
            if (rolledWriter != null)
            {
                rolledWriter.Dispose();
                rolledWriter = null;
            }
        }
    }
}
