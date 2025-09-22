using System;
using UnityEngine;

namespace ClickerEngine.DI
{
    public abstract class DIEntry
    {
        protected DIContainer _container { get; private set; }

        public DIEntry(DIContainer container)
        {
            _container = container;
        }

        public T CreateFactory<T>()
        {
            try
            {
                return ((DIEntry<T>)this).CreateFactory();
            }
            catch (Exception exception)
            {
                Debug.LogError($"DI container error, when create object of type: {typeof(T).Name}, error: {exception.Message}");
                throw new Exception("error");
            }
        }
        
    }
    
    public abstract class DIEntry<T> : DIEntry
    {
        protected Func<DIContainer, T> _factory { get; }
        
        public DIEntry(DIContainer container, Func<DIContainer, T> factory) : base(container)
        {
            _factory = factory;
        }
        public abstract T CreateFactory();
    }
}