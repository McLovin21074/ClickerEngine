using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ClickerEngine.Reactive
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        public int Count => _items.Count;

        public bool IsReadOnly => false;

        private readonly List<T> _items = new List<T>();
        private readonly List<IObserverCollection<T>> _observers = new ();

        public ReactiveCollection(List<T> items = null)
        {
            if (items is not null)
            {
                foreach (var item in items)
                {
                    _items.Add(item);
                }
            }
        }
        
        public void Add(T item)
        {
            var copyCollection = new T[Count];
            _items.CopyTo(copyCollection, 0);
            
            _items.Add(item);
            OnAddedElement(copyCollection, item);
        }
        
        public void Clear()
        {
            _items.Clear();
            OnClear();
        }
        

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(T item)
        {
            if (!_items.Contains(item))
                return false;
            
            var copyCollection = new T[Count];
            _items.CopyTo(copyCollection, 0);
            
            var result = _items.Remove(item);
            
            if(result)
                OnRemoveElement(copyCollection, item);
            
            return result;
        }
        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IBinding Subscribe(IObserverCollection<T> observer, [CallerMemberName] string caller = "")
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                return new ReactiveSubscriptionCollection<T>(this, observer);
            }
            else
            {
                Debug.LogWarning($"observer was subscribed already. It was called from {caller}.");
            }
            return null;
        }

        public void Unsubscribe(IObserverCollection<T> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public override bool Equals(object obj)
        {
            return _items.Equals(obj);
        }

        private void OnAddedElement(IEnumerable<T> collection, T item)
        {
            foreach (var observer in _observers)
            {
                observer.NotifyCollectionAdded(collection, item);
            }
        }

        private void OnRemoveElement(IEnumerable<T> collection, T item)
        {
            foreach (var observer in _observers)
            {
                observer.NotifyCollectionRemoved(collection, item);
            }
        }

        private void OnClear()
        {
            foreach (var observer in _observers)
            {
                observer.NotifyCollectionClear();
            }
        }
    }
}