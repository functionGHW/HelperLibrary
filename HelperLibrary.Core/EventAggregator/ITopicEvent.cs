using System;

namespace HelperLibrary.Core.EventAggregator
{
    public interface ITopicEvent
    {
        void Publish(object arg, bool multithread = false);

        ISubscriptionObject Subscribe(Action<object> action);

        void Unsubscribe(Action<object> action);
    }
}