using System;

namespace ClickerEngine.DI
{
    public class DIEntrySingleton<T> : DIEntry<T>
    {
        private T _instance;
        public DIEntrySingleton(DIContainer container, Func<DIContainer, T> factory) : base(container, factory) { }

        public override T CreateFactory()
        {
            if (_instance is null)
                _instance = _factory(_container);

            return _instance;
        }

    }
}