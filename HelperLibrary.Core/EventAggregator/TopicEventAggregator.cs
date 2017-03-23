using System;
using System.Collections.Concurrent;

namespace HelperLibrary.Core.EventAggregator
{
    public class TopicEventAggregator : ITopicEventAggregator
    {
        private ConcurrentDictionary<Type, ITopicEvent> dict = new ConcurrentDictionary<Type, ITopicEvent>();

        public TEvent GetTopicEvent<TEvent>() where TEvent : class, ITopicEvent, new()
        {
            var eventType = typeof(TEvent);
            var tmp = dict.GetOrAdd(eventType, t => new TEvent());
            return tmp as TEvent;
        }
    }
}
