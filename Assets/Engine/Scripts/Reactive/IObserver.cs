using System;

namespace ClickerEngine.Reactive
{
    public interface IObserver<in T> : IDisposable
    {
        void NotifyObservableChanged(T oldValue, T newValue);
        
    }
}
