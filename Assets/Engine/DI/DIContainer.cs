
using System;
using UnityEngine;
using System.Collections.Generic;

namespace ClickerEngine.DI
{
    public class DIContainer
    {
        private readonly DIContainer _parentContainer;

        private readonly Dictionary<(string, Type), object> _container;
        private readonly HashSet<(string, Type)> _cachedKey;

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;

            _container = new Dictionary<(string, Type), object>();
            _cachedKey = new HashSet<(string, Type)>();
            
        }
        
        public void Register<T>(Func<DIContainer, T> factory)
        {
            
        }

        public void Register<T>(Func<DIContainer, T> factory, string tag)
        {
            
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory, string tag)
        {
            
        }

        public void RegisterInstance<T>(T instance)
        {
            
        }

        public void RegisterInstance<T>(T instance, string tag)
        {
            
        }

        public T Resolve<T>(string tag = "")
        {
            
            return default(T);
        }

        private void Register<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (CheckKey<T>(key))
                return;
            
            
        }

        private void RegisterSingleton<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (CheckKey<T>(key))
                return;
        }

        private void RegisterInstance<T>((string, Type) key, T instance)
        {
            if (CheckKey<T>(key))
                return;
        }
        
        private bool CheckKey<T>((string, Type) key)
        {
            var result = _container.ContainsKey(key);
            
            //TODO: how to use container with include parentContainer
            if (!result && _parentContainer is not null)
            {
                result = _parentContainer.CheckKey<T>(key);
            }
            
#if UNITY_EDITOR
            if (result)
            {
                Debug.LogWarning($"{typeof(T).Name} contains with key: {key} in the DIContainer");
            }
#endif
            return result;
        }
    }
    
}

