using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Imi.MWFramework.Messaging;
using System.Threading;

namespace Imi.MWFramework.Adapter.File
{
    public class FileAdapter : BaseAdapter
    {
                
        private string path;
        private string filter;
        private FileSystemWatcher watcher;

        public FileAdapter(BasePropertyCollection configuration, string adapterId)
            : base(configuration, adapterId)
        { 
            
        }

        public override void Initialize()
        {
            path = Configuration.ReadAsString("Path");
            filter = Configuration.ReadAsString("Filter");

            watcher = new FileSystemWatcher(path, filter);
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Created += new FileSystemEventHandler(FileCreatedEventHandler);
            watcher.EnableRaisingEvents = true;
        }
        
        private void FileCreatedEventHandler(object sender, FileSystemEventArgs e)
        {
            string filename = e.FullPath;
            
            FileStream fileStream = null;
            int failCount = 0;

            while (failCount < 5)
            {
                try
                {
                    fileStream = System.IO.File.OpenRead(filename);
                    break;
                }
                catch
                {
                    failCount++;
                    Thread.Sleep(1000);
                }
            }

            if (fileStream != null)
            {
                BaseMessage msg = new BaseMessage(Path.GetFileName(filename), fileStream);

                FileAdapterEndPoint endPoint = new FileAdapterEndPoint(this, filename);

                OnEndPointCreated(endPoint);

                OnMessageReceived(msg, endPoint);

                try
                {
                    fileStream.Close();
                }
                finally
                {
                    OnEndPointDestroyed(endPoint);
                }
            }
        }

        public override void TransmitMessage(BaseMessage msg)
        {
            FileStream writer = new FileStream(msg.Metadata.Read("SendUri").ToString(), FileMode.Create, FileAccess.Write);
            
            msg.Data.Seek(0, SeekOrigin.Begin);
            
            int bufferSize = 1024;
            Byte[] buffer = new Byte[bufferSize];
            
            int bytesRead;
            
            while ((bytesRead = msg.Data.Read(buffer, 0, bufferSize)) > 0)
            {
                writer.Write(buffer, 0, bytesRead);
            }
                        
            writer.Close();
        }

        public override string ProtocolType
        {
            get 
            {
                return "file";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                watcher.Dispose();
            }
        }
    }
}
