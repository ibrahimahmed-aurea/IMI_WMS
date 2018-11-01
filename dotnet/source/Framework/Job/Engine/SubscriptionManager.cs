using System;
using System.Collections;
using System.Collections.Generic;
using Imi.Framework.Job.Interfaces;

namespace Imi.Framework.Job.Engine
{
    public class Subscriber
    {
        public ISubscriber subscriber;
        public string[] messageTypeList;

        public Subscriber(ISubscriber subscriber, string[] messageTypeList)
        {
            this.subscriber = subscriber;
            this.messageTypeList = messageTypeList;
        }
    }

    public class MessageSubscriptionList
    {
        private string name;
        private Dictionary<string, ISubscriber> subscriberDictionary;

        public MessageSubscriptionList(string name)
        {
            this.name = name;
            subscriberDictionary = new Dictionary<string, ISubscriber>();
        }

        public void AddSubscriber(ISubscriber subscriber)
        {
            subscriberDictionary.Add(subscriber.Name, subscriber);
        }

        public void RemoveSubscriber(string name)
        {
            subscriberDictionary.Remove(name);
        }

        public ICollection GetSubscriberList()
        {
            return (subscriberDictionary.Values);
        }

        public void Clear()
        {
            subscriberDictionary.Clear();
        }
    }

    internal class SubscriptionManager
    {
        private Dictionary<string, Subscriber> subscriberDictionary;
        private Dictionary<string, MessageSubscriptionList> subscriptionListDictionary;

        public SubscriptionManager()
        {
            subscriberDictionary = new Dictionary<string, Subscriber>();
            subscriptionListDictionary = new Dictionary<string, MessageSubscriptionList>();
        }
    
        public void AddSubscriber(ISubscriber subscriber, string[] messageTypeList)
        {
            Subscriber sub = new Subscriber(subscriber, messageTypeList);
            subscriberDictionary.Add(subscriber.Name, sub);

            foreach (string messageType in messageTypeList)
            {
                MessageSubscriptionList messageSubscriptionList = null;

                if (subscriptionListDictionary.ContainsKey(messageType))
                    messageSubscriptionList = subscriptionListDictionary[messageType];
                else
                {
                    messageSubscriptionList = new MessageSubscriptionList(messageType);
                    subscriptionListDictionary.Add(messageType, messageSubscriptionList);
                }

                messageSubscriptionList.AddSubscriber(subscriber);
            }
        }

        public void RemoveSubscriber(string name)
        {
            Subscriber sub = subscriberDictionary[name] as Subscriber;

            if (sub != null)
            {
                foreach (string messageType in sub.messageTypeList)
                {
                    MessageSubscriptionList messageSubscriptionList;

                    subscriptionListDictionary.TryGetValue(messageType, out messageSubscriptionList);

                    if (messageSubscriptionList != null)
                        messageSubscriptionList.RemoveSubscriber(name);
                }

                subscriberDictionary.Remove(name);
            }
        }

        public ICollection GetSubscriberList(string messageType)
        {
            MessageSubscriptionList messageSubscriberList;
            
            subscriptionListDictionary.TryGetValue(messageType, out messageSubscriberList);

            if (messageSubscriberList != null)
                return (messageSubscriberList.GetSubscriberList());
            else
                return (null);
        }

        public void Clear()
        {
            foreach (MessageSubscriptionList messageSubscriberList in subscriptionListDictionary.Values)
                messageSubscriberList.Clear();

            subscriptionListDictionary.Clear();
            subscriberDictionary.Clear();
        }

    }
}
