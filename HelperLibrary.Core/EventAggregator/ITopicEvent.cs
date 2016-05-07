using System;

namespace HelperLibrary.Core.EventAggregator
{
    public interface ITopicEvent
    {
        void Publish(object arg);

        ISubscriptionObject Subscribe(Action<object> action);

        void Unsubscribe(Action<object> action);
    }
}