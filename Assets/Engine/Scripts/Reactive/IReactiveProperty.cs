namespace ClickerEngine.Reactive
{
    public interface IReactiveProperty<out T> : IObservable<T>
    {
        T Value { get; }
    }
}