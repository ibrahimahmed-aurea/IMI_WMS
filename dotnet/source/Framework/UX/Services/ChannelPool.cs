using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace Imi.Framework.UX.Services
{
    public class ChannelPool<TChannel> : IDisposable
    {        
        private List<TChannel> channelList;
        private List<TChannel> inUseChannelList;
        private int maxPoolSize;
        private bool isDisposed;
        private ChannelFactory<TChannel> channelFactory;
                           
        public ChannelPool(int maxPoolSize, ChannelFactory<TChannel> channelFactory)
        {
            channelList = new List<TChannel>();
            inUseChannelList = new List<TChannel>();
            this.maxPoolSize = maxPoolSize;
            this.channelFactory = channelFactory;
        }

        ~ChannelPool()
        {
            Dispose(false);
        }
        
        public TChannel OpenChannel()
        {
            return OpenChannel(false);
        }
                
        public TChannel OpenChannel(bool forceNew)
        {
            lock (channelList)
            {
                bool isNew = false;

                while (channelList.Count == 0)
                {
                    if (inUseChannelList.Count < maxPoolSize)
                    {
                        channelList.Add(channelFactory.CreateChannel());
                        isNew = true;
                        break;
                    }
                    else
                    {
                        Monitor.Wait(channelList);
                    }
                }

                TChannel channel = channelList.Last();
                channelList.Remove(channel);

                if (forceNew && !isNew)
                {
                    channel = channelFactory.CreateChannel();
                    isNew = true;
                }

                if (isNew)
                {
                    ((ICommunicationObject)channel).Open();
                }

                inUseChannelList.Add(channel);
                                                                
                return channel;
            }
        }

        public void ReleaseChannel(TChannel channel)
        {
            lock (channelList)
            {
                if (inUseChannelList.Remove(channel))
                {
                    channelList.Add(channel);
                    Monitor.Pulse(channelList);
                }
            }
        }

        public void CloseChannel(TChannel channel)
        {
            CloseChannel(channel, false);
        }

        public void CloseChannel(TChannel channel, bool abort)
        {
            lock (channelList)
            {
                inUseChannelList.Remove(channel);

                if (abort)
                {
                    channelList.Remove(channel);
                }
            }

            ICommunicationObject communicationObject = channel as ICommunicationObject;

            if (communicationObject != null)
            {
                try
                {
                    if (!abort)
                        communicationObject.Close();
                }
                catch (InvalidOperationException)
                {
                }
                catch (CommunicationObjectFaultedException)
                {
                }
                catch (TimeoutException)
                {
                }
                finally
                {
                    communicationObject.Abort();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    try
                    {
                        channelFactory.Abort();
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (CommunicationException)
                    { 
                    }
                }
            }

            isDisposed = true;
        }
    }
}
