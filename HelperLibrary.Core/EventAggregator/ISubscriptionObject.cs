using System;

namespace HelperLibrary.Core.EventAggregator
{
    public interface ISubscriptionObject : IDisposable
    {
        void Unsubscribe();
    }
}
