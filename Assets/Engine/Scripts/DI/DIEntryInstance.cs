namespace ClickerEngine.DI
{
    public class DIEntryInstance<T> : DIEntry<T>
    {
        private T _instance;
        
        public DIEntryInstance(DIContainer container, T instance) : base(container, _ => instance) { }

        public override T CreateFactory()
        {
            if (_instance is null)
                _instance = _factory(_container);

            return _instance;
        }
    }
}