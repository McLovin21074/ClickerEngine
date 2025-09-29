using System.Collections.Generic;

namespace ClickerEngine.Reactive
{
    public interface IReactiveCollection<T> : IObservableCollection<T>, ICollection<T>
    {
        
    }
}