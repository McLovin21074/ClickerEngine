using System;

namespace ClickerEngine.DI
{
    public class DIEntryTransient<T> : DIEntry<T>
    {
        public DIEntryTransient(DIContainer container, Func<DIContainer, T> factory) : base(container, factory) { }

        public override T CreateFactory()
        {
            return _factory(_container);
        }
    }
}