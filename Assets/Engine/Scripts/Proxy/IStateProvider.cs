using ClickerEngine.Reactive;

namespace ClickerEngine.Proxy
{
    public interface IStateProvider<TProxy> where TProxy : IProxy
    {
        TProxy StateModel { get; }

        IObservable<bool> SaveState();

        IObservable<bool> ResetState();

        IObservable<TProxy> LoadState();
    }
}