using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting;
using System.Reflection;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Services;
using Imi.Framework.Services;
using System.Threading;
using System.ServiceModel.Security;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Services
{    
    public class WcfServiceProxy<TChannel> : RealProxy
        where TChannel : class
    {
        private TChannel channel;
        private object hashCode;
        private ChannelPool<TChannel> channelPool;
        IUserSessionService userSessionService;
        private int maxRetryCount;
                                
        public WcfServiceProxy(ChannelPool<TChannel> channelPool, IUserSessionService userSessionService)
            : base(typeof(TChannel))
        {
            this.channelPool = channelPool;
            this.userSessionService = userSessionService;
            hashCode = new object();
            maxRetryCount = 1;
        }
                
        private void Abort()
        {
            if (channel != null)
                channelPool.CloseChannel(channel, true);
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMsg = msg as IMethodCallMessage;

            if (callMsg != null)
            {
                IMethodReturnMessage resultMsg = InvokeMethod(callMsg);

                return resultMsg;
            }
            else
                throw new NotSupportedException();
        }

        IMethodReturnMessage InvokeMethod(IMethodCallMessage callMsg)
        {
            if (callMsg.MethodName == "GetHashCode")
            {
                return new ReturnMessage(hashCode.GetHashCode(), null, 0, null, callMsg);
            }
            else if (callMsg.MethodName == "Equals")
            {
                object o = callMsg.Args[0];
                
                if (o is TChannel)
                {
                    if (o.GetHashCode() == hashCode.GetHashCode())
                        return new ReturnMessage(true, null, 0, null, callMsg);
                }

                return new ReturnMessage(false, null, 0, null, callMsg);
            }
            else if (callMsg.MethodName == "GetType")
            {
                return new ReturnMessage(typeof(TChannel), null, 0, null, callMsg);
            }
            else if (callMsg.MethodName == "ToString")
            {
                return new ReturnMessage(typeof(TChannel).ToString(), null, 0, null, callMsg);
            }
            else if (callMsg.MethodName == "Abort")
            {
                Abort();

                return new ReturnMessage(null, null, 0, null, callMsg);
            }
            else
            {
                return InvokeChannelMethod(callMsg, 0);
            }
        }

        private IMethodReturnMessage InvokeChannelMethod(IMethodCallMessage callMsg, int retryCount)
        {
            try
            {
                if (retryCount == 0)
                    channel = channelPool.OpenChannel();
                else
                    channel = channelPool.OpenChannel(true);

                foreach (object arg in callMsg.Args)
                {
                    RequestMessageBase request = arg as RequestMessageBase;

                    if (request != null)
                    {
                        request.UserId = userSessionService.UserId;
                        request.TerminalId = userSessionService.TerminalId;

                        if (request.UICulture == null)
                        {
                            request.UICulture = userSessionService.UICulture.ToString();
                        }
                        
                        request.SessionId = userSessionService.SessionId;
                        break;
                    }
                }

                /* Call method on underlying channel object */
                try
                {
                    object retVal = typeof(TChannel).InvokeMember(callMsg.MethodName, BindingFlags.InvokeMethod, null, channel, callMsg.Args);
                    return new ReturnMessage(retVal, null, 0, null, callMsg);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    else
                        throw;
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (SecurityNegotiationException)
            {
                throw;
            }
            catch (CommunicationObjectAbortedException)
            {
                channelPool.CloseChannel(channel, false);
                channel = null;
                throw;
            }
            catch (CommunicationException ex)
            {
                channelPool.CloseChannel(channel, true);
                channel = null;

                if ((retryCount < maxRetryCount))
                    return InvokeChannelMethod(callMsg, retryCount + 1);
                else
                    return new ReturnMessage(ex, callMsg);
            }
            finally
            {
                if (channel != null)
                    channelPool.ReleaseChannel(channel);
            }
        }
    }
}
