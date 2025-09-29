namespace ClickerEngine.Reactive
{
    public class ReactiveSubscription<T> : IBinding
    {
        private readonly IReactiveProperty<T> _reactiveProperty;
        private readonly IObserver<T> _observer;

        public ReactiveSubscription(IReactiveProperty<T> reactiveProperty, IObserver<T> observer)
        {
            _reactiveProperty = reactiveProperty;
            _observer = observer;
        }
        
        public void Dispose()
        {
            if (_reactiveProperty is null)
                return;
            
            _reactiveProperty.Unsubscribe(_observer);
            _observer.Dispose();
        }

        public void Binded()
        {
            _observer.NotifyObservableChanged(default, _reactiveProperty.Value);
        }
    }
}