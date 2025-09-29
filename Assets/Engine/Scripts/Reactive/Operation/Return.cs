namespace ClickerEngine.Reactive.Operation
{
    public static partial class Observable
    {
        public static IObservable<T> Return<T>(T value)
        {
            return new ImmediateScheduleReturn<T>(value);
        }
    }

    internal class ImmediateScheduleReturn<T> : IObservable<T>
    {
        private T _value;
        internal ImmediateScheduleReturn(T value)
        {
            _value = value;
        }

        public IBinding Subscribe(IObserver<T> observer)
        {
            observer.NotifyObservableChanged(default, _value);
            return null;
        }

        public void Unsubscribe(IObserver<T> observer)
        {

        }
    }
}