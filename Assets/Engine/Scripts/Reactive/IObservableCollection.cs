namespace ClickerEngine.Reactive
{
    public interface IObservableCollection<out T>
    {
        IBinding Subscribe(IObserverCollection<T> observer, string caller = "");
        void Unsubscribe(IObserverCollection<T> observer);
    }
}