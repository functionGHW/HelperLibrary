using System;
using System.Collections.Concurrent;

namespace HelperLibrary.Core.EventAggregator
{
    public class TopicEventAggregator : ITopicEventAggregator
    {
        private ConcurrentDictionary<string, ITopicEvent> dict = new ConcurrentDictionary<string, ITopicEvent>();

        public ITopicEvent GetTopicEvent(string topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            var tmp = dict.GetOrAdd(topic, t => new TopicEvent());
            return tmp;
        }
    }
}
