using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperLibrary.Core.EventAggregator
{
    public class TopicEvent : ITopicEvent
    {
        private readonly HashSet<Action<object>> subscribers = new HashSet<Action<object>>();

        public ISubscriptionObject Subscribe(Action<object> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (subscribers)
            {
                subscribers.Add(action);
            }
            return new SubscriptionObject(action, this);
        }

        public void Unsubscribe(Action<object> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (subscribers)
            {
                subscribers.Remove(action);
            }
        }

        public void Publish(object arg, bool multithread = false)
        {
            Action<object>[] ary;
            lock (subscribers)
            {
                if (subscribers.Count == 0)
                    return;

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


        private sealed class SubscriptionObject : ISubscriptionObject
        {
            private readonly Action<object> action;
            private readonly TopicEvent topicEvent;
            private bool unsubscribed;

            public SubscriptionObject(Action<object> action, TopicEvent topicEvent)
            {
                this.action = action;
                this.topicEvent = topicEvent;
            }

            public void Dispose()
            {
                this.Unsubscribe();
            }

            public void Unsubscribe()
            {
                if (unsubscribed)
                    return;

                lock (this.topicEvent.subscribers)
                {
                    if (unsubscribed)
                        return;

                    this.topicEvent.subscribers.Remove(action);
                    unsubscribed = true;
                }
            }
        }
    }
}
