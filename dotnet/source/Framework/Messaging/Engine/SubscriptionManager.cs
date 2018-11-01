using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Imi.Framework.Shared;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Messaging.Engine
{
    /// <summary>
    /// Manages message subscriptions.
    /// </summary>
    public class SubscriptionManager
    {
        private Dictionary<string, List<SubscriberBase>> subscriberDictionary;
        private readonly string allMessageTypes;

        internal SubscriptionManager()
        {
            subscriberDictionary = new Dictionary<string, List<SubscriberBase>>();
            allMessageTypes = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Subscribes to all message types.
        /// </summary>
        /// <param name="subscriber">A reference to the subscriber.</param>
        public void Subscribe(SubscriberBase subscriber)
        {
            Subscribe(allMessageTypes, subscriber);
        }
        
        /// <summary>
        /// Unsubscribes to all message types.
        /// </summary>
        /// <param name="subscriber">A reference to the subscriber to unsubscribe.</param>
        public void Unsubscribe(SubscriberBase subscriber)
        {
            Unsubscribe(allMessageTypes, subscriber);
        }

        /// <summary>
        /// Subscribes to the specified message type.
        /// </summary>
        /// <param name="messageType">The message type to subscribe to.</param>
        /// <param name="subscriber">A reference to the subscriber.</param>
        public void Subscribe(string messageType, SubscriberBase subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");

            if (!subscriberDictionary.ContainsKey(messageType))
            {
                lock (subscriberDictionary)
                {
                    if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Information) == SourceLevels.Information)
                        MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Information, 0, "Adding subscription for: \"{0}\" to: \"{1}\".", messageType, subscriber.GetType().Name);
                    
                    subscriberDictionary.Add(messageType, new List<SubscriberBase>());
                }
            }

            List<SubscriberBase> subscriberCollection = subscriberDictionary[messageType];

            subscriberCollection.Add(subscriber);
        }

        /// <summary>
        /// Unsubscribes to the specified message type.
        /// </summary>
        /// <param name="messageType">The message type to unsubscribe.</param>
        /// <param name="subscriber">A reference to the subscriber to unsubscribe.</param>
        public void Unsubscribe(string messageType, SubscriberBase subscriber)
        {
            if (subscriberDictionary.ContainsKey(messageType))
            {
                lock (subscriberDictionary)
                {
                    List<SubscriberBase> subscriberCollection = subscriberDictionary[messageType];
                    subscriberCollection.Remove(subscriber);
                }
            }
        }

        internal void DistributeMessage(MultiPartMessage msg)
        {
            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Distributing message: \"{0}\"...", msg.MessageType);
            
            if (subscriberDictionary.ContainsKey(msg.MessageType))
            {
                foreach (SubscriberBase subscriber in subscriberDictionary[msg.MessageType])
                {
                    InvokeSubscriber(msg, subscriber);
                }
            }

            if (subscriberDictionary.ContainsKey(allMessageTypes))
            {
                foreach (SubscriberBase subscriber in subscriberDictionary[allMessageTypes])
                {
                    InvokeSubscriber(msg, subscriber);
                }
            }
        }

        private static void InvokeSubscriber(MultiPartMessage msg, SubscriberBase subscriber)
        {

            if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose)
                MessageEngine.Instance.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Invoking subscriber: \"{0}\"...", subscriber.GetType().Name);    

            try
            {
                subscriber.InternalInvoke(msg);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsCritical(ex))
                {
                    throw;
                }
                else
                {
                    try
                    {
                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, ex);

                        if ((MessageEngine.Instance.Tracing.Switch.Level & SourceLevels.Error) == SourceLevels.Error)
                            MessageEngine.Instance.Tracing.TraceData(TraceEventType.Error, 0, string.Format("Message content:\n{0}", msg.ToXmlString()));
                    }
                    catch (Exception logEx)
                    {
                        if (ExceptionHelper.IsCritical(logEx))
                            throw;
                    }
                    
                }
            }
        }

        
    }

}
