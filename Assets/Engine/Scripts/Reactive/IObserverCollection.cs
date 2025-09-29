using System;
using System.Collections.Generic;

namespace ClickerEngine.Reactive
{
    public interface IObserverCollection<in T> : IDisposable
    {
        void NotifyCollectionAdded(IEnumerable<T> collection, T newValue);
        void NotifyCollectionRemoved(IEnumerable<T> collection, T newValue);
        void NotifyCollectionClear();
    }
}