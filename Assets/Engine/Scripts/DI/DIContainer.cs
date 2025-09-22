using System;
using UnityEngine;
using System.Collections.Generic;

namespace ClickerEngine.DI
{
    public class DIContainer
    {
        private readonly DIContainer _parentContainer;

        private readonly Dictionary<(string, Type), DIEntry> _container;
        private readonly HashSet<(string, Type)> _cachedKey;

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;

            _container = new Dictionary<(string, Type), DIEntry>();
            _cachedKey = new HashSet<(string, Type)>();
            
        }
        
        public void Register<T>(Func<DIContainer, T> factory)
        {
            Register(("", typeof(T)), factory);
        }

        public void Register<T>(Func<DIContainer, T> factory, string tag)
        {
            Register((tag, typeof(T)), factory);
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(("", typeof(T)), factory);
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory, string tag)
        {
            RegisterSingleton((tag, typeof(T)), factory);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(("", typeof(T)), instance);
        }

        public void RegisterInstance<T>(T instance, string tag)
        {
            RegisterInstance((tag, typeof(T)), instance);
        }

        public T Resolve<T>(string tag = "")
        {
            var key = (tag, typeof(T));
            
            if (_cachedKey.Contains(key))
            {
                
#if UNITY_EDITOR
                Debug.LogWarning($"factory with key: {key} is already being searched in the DIContainer");
#endif
                return default;
            }

            _cachedKey.Add(key);
            T result = FindFactory<T>(key);
            _cachedKey.Remove(key);
            
            return result;
        }
        
        private T FindFactory<T>((string, Type) key)
        {
            T result;
            if (!_container.ContainsKey(key))
            {
                if (_parentContainer is null)
                {
                    Debug.LogWarning("DI Container can't find type: " + key.Item2);
                    return default;
                }

                result = _parentContainer.FindFactory<T>(key);
            }
            else
            {
                result = _container[key].CreateFactory<T>();
            }
                
            return result;
            
        }
        
        private void Register<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (CheckKey<T>(key))
                return;

            var dIEntry = new DIEntryTransient<T>(this, factory);
            _container[key] = dIEntry;
        }

        private void RegisterSingleton<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (CheckKey<T>(key))
                return;
            
            var dIEntry = new DIEntrySingleton<T>(this, factory);
            _container[key] = dIEntry;
        }

        private void RegisterInstance<T>((string, Type) key, T instance)
        {
            if (CheckKey<T>(key))
                return;
            
            var dIEntry = new DIEntryInstance<T>(this, instance);
            _container[key] = dIEntry;
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

