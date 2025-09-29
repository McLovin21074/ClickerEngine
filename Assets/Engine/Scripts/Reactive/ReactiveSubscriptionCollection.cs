namespace ClickerEngine.Reactive
{
    public class ReactiveSubscriptionCollection<T> : IBinding
    {
        private IReactiveCollection<T> _reactiveOwner;
        private IObserverCollection<T> _observer;

        public ReactiveSubscriptionCollection(IReactiveCollection<T> reactiveOwner, IObserverCollection<T> observer)
        {
            _reactiveOwner = reactiveOwner;
            _observer = observer;
        }

        public void Binded()
        {
            foreach (var item in _reactiveOwner)
            {
                _observer.NotifyCollectionAdded(_reactiveOwner, item);
            }
        }

        public void Dispose()
        {
            if (_reactiveOwner is null)
                return;

            _reactiveOwner.Unsubscribe(_observer);
            _observer.Dispose();
            _reactiveOwner = null;
            _observer = null;
        }
    }
}