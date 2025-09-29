using System;

namespace ClickerEngine.Reactive.Extension
{
    public static class ObservableSubscribeExtension
    {
        public static IBinding Subscribe<T>(this IObservable<T> observable, Action action)
        {
            return observable.Subscribe(new AnonymousObserver<T>(action));
        }

        public static IBinding Subscribe<T>(this IObservable<T> observable, Action<T> action)
        {
            return observable.Subscribe(new AnonymousObserver<T>(action));
        }

        public static IBinding Subscribe<T>(this IObservable<T> observable, Action<T, T> action)
        {
            return observable.Subscribe(new AnonymousObserver<T>(action));
        }
    }
    
    internal sealed class AnonymousObserver<T> : IObserver<T>
    {
        private Action _actionWithoutParams;
        private Action<T> _actionWithParams;
        private Action<T, T> _actionWithAllParams;

        internal AnonymousObserver(Action actonWithoutParams) : this(actonWithoutParams, null, null) { }
        internal AnonymousObserver(Action<T> actionWithParams) : this(null, actionWithParams, null) { }
        internal AnonymousObserver(Action<T, T> actionWithAllParams) : this(null, null, actionWithAllParams) { }

        internal AnonymousObserver(Action actonWithoutParams, Action<T> actionWithParams, Action<T, T> actionWithAllParams)
        {
            _actionWithoutParams = actonWithoutParams;
            _actionWithParams = actionWithParams;
            _actionWithAllParams = actionWithAllParams;
        }

        public void NotifyObservableChanged(T oldValue, T newValue)
        {
            _actionWithoutParams?.Invoke();
            _actionWithParams?.Invoke(newValue);
            _actionWithAllParams?.Invoke(oldValue, newValue);
        }

        public void Dispose()
        {
            _actionWithoutParams = null;
            _actionWithParams = null;
            _actionWithAllParams = null;
        }
    }
}