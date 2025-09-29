using ClickerEngine.Reactive;

namespace ClickerEngine.Engine.Scripts
{
    public class SingleReactiveProperty<T> : IReactiveProperty<T>
    {
        public T Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                OnChanged(oldValue, _value);
            }
        }

        private T _value;
        private IObserver<T> _observer;

        public SingleReactiveProperty()
        {
            _value = default(T);
            _observer = null;
        }

        
        public IBinding Subscribe(IObserver<T> observer)
        {
            if (_observer != null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("Cannot subscribe more one in singleReactiveProperty");
#endif
                return null;
            }

            _observer = observer;
            return new ReactiveSubscription<T>(this, _observer);
        }

        public void Unsubscribe(IObserver<T> observer)
        {
            if (!ReferenceEquals(_observer, observer))
                return;

            _observer = null;
        }

        public void SetValue(T newValue)
        {
            if (newValue.Equals(_value))
                return;
            
            var oldValue = _value;
            _value = newValue;
            
            OnChanged(oldValue, newValue);
        }

        private void OnChanged(T oldValue, T newValue)
        {
            if (_observer != null)
            {
                _observer?.NotifyObservableChanged(oldValue, newValue);
            }
        }
        
    }
}