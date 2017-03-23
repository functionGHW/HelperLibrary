/* 
 * FileName:    TopicEvent.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/23/2017 11:15:49 PM
 * Description:
 * */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.EventAggregator
{
    public class TopicEvent<TArg> : ITopicEvent
    {
        private readonly HashSet<Action<TArg>> subscribers = new HashSet<Action<TArg>>();
        private readonly object syncObj = new object();

        public void Publish(TArg arg, bool multithread = false)
        {
            Action<TArg>[] ary;
            lock (syncObj)
            {
                ary = subscribers.ToArray();
            }

            if (multithread)
            {
                foreach (var item in ary)
                {
                    item.BeginInvoke(arg, null, null);
                }
            }
            else
            {
                foreach (var item in ary)
                {
                    item.Invoke(arg);
                }
            }
        }

        public ISubscriptionObject Subscribe(Action<TArg> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (syncObj)
            {
                subscribers.Add(action);
            }
            return new SubscriptionObject(this, action);
        }

        public void Unsubscribe(Action<TArg> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (syncObj)
            {
                subscribers.Remove(action);
            }
        }

        private class SubscriptionObject : ISubscriptionObject
        {
            private TopicEvent<TArg> topicEvent;
            private Action<TArg> action;

            public SubscriptionObject(TopicEvent<TArg> topicEvent, Action<TArg> action)
            {
                this.topicEvent = topicEvent;
                this.action = action;
            }

            public void Dispose()
            {
                this.Unsubscribe();
            }

            public void Unsubscribe()
            {
                topicEvent.Unsubscribe(action);
            }
        }

    }
}
