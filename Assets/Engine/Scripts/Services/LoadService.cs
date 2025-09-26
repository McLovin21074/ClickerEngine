using System;
using UnityEngine;
using ClickerEngine.DI;

namespace ClickerEngine.Services
{
    public class LoadService
    {
        private readonly DIContainer _container;
        
        public LoadService(DIContainer container)
        {
            _container = container;
        }

        public TView LoadPrefabView<TView>(string path) where TView : MonoBehaviour, IView
        {
            var resourcePath = string.IsNullOrWhiteSpace(path) ? typeof(TView).Name : path;
            var prefab = Resources.Load<TView>(resourcePath);

            if (prefab == null)
            {
                Debug.LogError($"Load service failed load prefab: {typeof(TView).Name} in path: {resourcePath}.");
                throw new ArgumentException($"Error loading prefab {typeof(TView).Name} in path: {resourcePath}.");
            }

            return prefab;
        }

        public TView LoadView<TService, TView>(TView prefab, string tag = "")
            where TService : class, IService
            where TView : MonoBehaviour, IView
        {
            if (prefab == null)
            {
                Debug.LogError("Load service failed load view prefab is null");
                throw new ArgumentNullException(nameof(prefab), "Failed load view prefab is null");
            }
            var service = _container.Resolve<TService>(tag);
            var viewInstance = UnityEngine.Object.Instantiate(prefab);
            viewInstance.Bind(service);
            
            if (service == null)
            {
                Debug.LogWarning($"Load service resolved null for service type: {typeof(TService).Name} with tag: {tag}");
            }

            return viewInstance;
        }
    }
}
