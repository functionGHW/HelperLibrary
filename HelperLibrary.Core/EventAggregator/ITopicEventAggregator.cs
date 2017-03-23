namespace HelperLibrary.Core.EventAggregator
{
    public interface ITopicEventAggregator
    {
        TEvent GetTopicEvent<TEvent>() where TEvent : class, ITopicEvent, new();
    }
}
