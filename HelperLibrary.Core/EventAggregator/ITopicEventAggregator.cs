namespace HelperLibrary.Core.EventAggregator
{
    public interface ITopicEventAggregator
    {
        ITopicEvent GetTopicEvent(string topic);
    }
}
