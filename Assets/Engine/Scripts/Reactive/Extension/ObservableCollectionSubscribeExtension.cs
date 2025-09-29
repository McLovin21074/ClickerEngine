using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClickerEngine.Reactive.Extension
{
    public static class ObservableCollectionSubscribeExtension
    {
        public static IBinding Subscribe<T>(this IObservableCollection<T> observableCollection, Action actionAdded, Action actionRemoved, Action actionClear)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, actionRemoved, actionClear));
        }

        public static IBinding Subscribe<T>(this IObservableCollection<T> observableCollection, Action<T> actionAdded, Action<T> actionRemoved, Action actionClear)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, actionRemoved, actionClear));
        }

        public static IBinding Subscribe<T>(this IObservableCollection<T> observableCollection, Action<IEnumerable<T>, T> actionAdded, Action<IEnumerable<T>, T> actionRemoved, Action actionClear)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, actionRemoved, actionClear));
        }

        public static IBinding SubscribeClear<T>(this IObservableCollection<T> observableCollection, Action actionClear)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionClear));
        }
        
        public static IBinding SubscribeAdded<T>(this IObservableCollection<T> observableCollection, Action actionAdded)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, null, null));
        }

        public static IBinding SubscribeAdded<T>(this IObservableCollection<T> observableCollection, Action<T> actionAdded)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, null, null));
        }

        public static IBinding SubscribeAdded<T>(this IObservableCollection<T> observableCollection, Action<IEnumerable<T>, T> actionAdded)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(actionAdded, null, null));
        }

        public static IBinding SubscribeRemoved<T>(this IObservableCollection<T> observableCollection, Action actionRemoved)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(null, actionRemoved, null));
        }

        public static IBinding SubscribeRemoved<T>(this IObservableCollection<T> observableCollection, Action<T> actionRemoved)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(null, actionRemoved, null));
        }

        public static IBinding SubscribeRemoved<T>(this IObservableCollection<T> observableCollection, Action<IEnumerable<T>, T> actionRemoved)
        {
            return observableCollection.Subscribe(new AnonymousObserverCollection<T>(null, actionRemoved, null));
        }
    }


    internal sealed class AnonymousObserverCollection<T> : IObserverCollection<T>
    {
        private Action _actionAddedWithoutParams;
        private Action<T> _actionAddedWithParams;
        private Action<IEnumerable<T>, T> _actionAddedWithParamsAndCollection;

        private Action _actionClearWithoutParams;

        private Action _actionRemovedWithoutParams;
        private Action<T> _actionRemovedWithParams;
        private Action<IEnumerable<T>, T> _actionRemovedWithParamsAndCollection;
        
        internal AnonymousObserverCollection(Action actionClear) : this (null, 
                                                                    actionClear,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null) { }
        
        internal AnonymousObserverCollection(Action actionAdded, Action actionRemoved, Action actionClear) :  this(actionAdded, actionClear, actionRemoved, 
                                                                                                                   null, 
                                                                                                                   null, 
                                                                                                                   null, 
                                                                                                                   null) { }

        internal AnonymousObserverCollection(Action<T> actionAdded, Action<T> actionRemoved, Action actionClear) : this(null, 
                                                                                                                        actionClear, 
                                                                                                                        null, 
                                                                                                                        actionAdded, 
                                                                                                                        actionRemoved, 
                                                                                                                        null, 
                                                                                                                        null) { }

        internal AnonymousObserverCollection(Action<IEnumerable<T>, T> actionAdded, Action<IEnumerable<T>, T> actionRemoved, Action actionClear) : this(null, 
                                                                                                                                                actionClear, 
                                                                                                                                                null, 
                                                                                                                                                null, 
                                                                                                                                                null, 
                                                                                                                                                actionAdded,
                                                                                                                                                actionRemoved) { }

        internal AnonymousObserverCollection(Action actionAddedWithoutParams, Action actionClearWithoutParams, Action actionRemovedWithoutParams,
                                             Action<T> actionAddedWithParams, Action<T> actionRemovedWithParams,
                                             Action<IEnumerable<T>, T> actionAddedWithParamsAndCollection, Action<IEnumerable<T>, T> actionRemovedWithParamsAndCollection)
        {
            _actionAddedWithoutParams = actionAddedWithoutParams;
            _actionAddedWithParams = actionAddedWithParams;
            _actionAddedWithParamsAndCollection = actionAddedWithParamsAndCollection;

            _actionClearWithoutParams = actionClearWithoutParams;

            _actionRemovedWithoutParams = actionRemovedWithoutParams;
            _actionRemovedWithParams = actionRemovedWithParams;
            _actionRemovedWithParamsAndCollection = actionRemovedWithParamsAndCollection;
        }

        public void Dispose()
        {
            _actionAddedWithoutParams = null;
            _actionAddedWithParams = null;
            _actionAddedWithParamsAndCollection = null;

            _actionClearWithoutParams = null;

            _actionRemovedWithoutParams = null;
            _actionRemovedWithParams = null;
            _actionRemovedWithParamsAndCollection = null;
        }

        public void NotifyCollectionAdded(IEnumerable<T> collection, T newValue)
        {
            _actionAddedWithoutParams?.Invoke();
            _actionAddedWithParams?.Invoke(newValue);
            _actionAddedWithParamsAndCollection?.Invoke(collection, newValue);
        }

        public void NotifyCollectionClear()
        {
            _actionClearWithoutParams?.Invoke();
        }

        public void NotifyCollectionRemoved(IEnumerable<T> collection, T newValue)
        {
            _actionRemovedWithoutParams?.Invoke();
            _actionRemovedWithParams?.Invoke(newValue);
            _actionRemovedWithParamsAndCollection?.Invoke(collection, newValue);
        }
    }
}