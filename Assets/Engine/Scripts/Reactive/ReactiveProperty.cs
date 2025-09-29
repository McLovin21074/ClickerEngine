using System.Collections.Generic;

namespace ClickerEngine.Reactive
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        public T Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                OnChanged(oldValue, value);
            }
        }

        
        private T _value;
        private List<IObserver<T>> _subscribers;

        public ReactiveProperty(T value = default)
        {
            _value = value;
            _subscribers = new List<IObserver<T>>();
        }
        
        public IBinding Subscribe(IObserver<T> observer)
        {
            if (!_subscribers.Contains(observer))
            {
                _subscribers.Add(observer);
                return new ReactiveSubscription<T>(this,  observer);
            }
            
            return null;
        }

        public void Unsubscribe(IObserver<T> observer)
        {
            if (_subscribers.Contains(observer))
            {
                _subscribers.Remove(observer);
            }
        }

        public override string ToString()
        {
            return _value.ToString();
        }
        
        private void OnChanged(T oldValue, T newValue, object sender = null)
        {
            foreach (var subscriber in _subscribers)
                subscriber.NotifyObservableChanged(oldValue, newValue);
        }
    }
}