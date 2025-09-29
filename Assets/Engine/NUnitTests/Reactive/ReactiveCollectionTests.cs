using System;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using ClickerEngine.Reactive;
using System.Collections.Generic;
using System.Linq;
using ClickerEngine.Reactive.Extension;

namespace ClickerEngineTest.Reactive
{
    public class ReactiveCollectionTests
    {
        [Test]
        public void CheckCreateReactiveCollectionTest()
        {
            var reactiveCollection = new ReactiveCollection<int>();
            Assert.Pass();
        }

        [Test]
        public void CheckEnumerationCollectionTest()
        {
            var reactiveCollection = new ReactiveCollection<int>();
            reactiveCollection.Add(12);
            reactiveCollection.Add(15);
            reactiveCollection.Add(18);

            var actualList = new List<int>();
            actualList.Add(12);
            actualList.Add(15);
            actualList.Add(18);

            var index = 0;
            
            foreach (var value in reactiveCollection)
                Assert.That(value, Is.EqualTo(actualList[index++]));
            
        }

        [Test]
        public void CheckCreateReactiveCollectionWithListTest()
        {
            var actualList = new List<int>();
            actualList.Add(77);
            actualList.Add(56);
            actualList.Add(54);
            
            var reactiveCollection = new ReactiveCollection<int>(actualList);

            var index = 0;
            
            foreach (var value in reactiveCollection)
                Assert.That(value, Is.EqualTo(actualList[index++]));
            
        }

        [Test]
        public void CheckSubscribeObserverAddedCollectionTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();
            
            var actualList = new List<float>();
            actualList.Add(120);
            actualList.Add(54);
            actualList.Add(45);
            
            var reactiveCollection = new ReactiveCollection<float>(actualList);
            
            var binding = reactiveCollection.Subscribe(fakeObserverCollection);

            reactiveCollection.Add(90);
            
            fakeObserverCollection.CheckNotifyObserverAdded(actualList, 90);
            binding.Dispose();
            
        }

        [Test]
        public void CheckSubscribeObserverRemovedCollectionTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();

            var reactiveCollection = new ReactiveCollection<float>();
            reactiveCollection.Add(33);
            reactiveCollection.Add(56);
            reactiveCollection.Add(76);
            reactiveCollection.Add(32);

            var binding = reactiveCollection.Subscribe(fakeObserverCollection);

            var actualList = new List<float>();
            actualList.Add(33);
            actualList.Add(56);
            actualList.Add(76);
            actualList.Add(32);

            reactiveCollection.Remove(56);
            
            fakeObserverCollection.CheckNotifyObserverRemoved(actualList, 56);
            binding.Dispose();
        }

        [Test]
        public void CheckSubscribeObserverClearCollectionTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();
            var reactiveCollection = new ReactiveCollection<float>();
            var binding = reactiveCollection.Subscribe(fakeObserverCollection);
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            binding.Dispose();
        }

        [Test]
        public void CheckDisposeBindingAfterSubscribeObserverCollectionTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();
            var reactiveCollection = new ReactiveCollection<float>();
            var binding = reactiveCollection.Subscribe(fakeObserverCollection);
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            binding.Dispose();
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
        }

        [Test]
        public void CheckLogWarningAfterSubscribeObserverCollectionWhichSubscribeTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();
            var reactiveCollection = new ReactiveCollection<float>();
            var firstBinding = reactiveCollection.Subscribe(fakeObserverCollection);
            LogAssert.Expect(LogType.Warning, $"observer was subscribed already. It was called from {nameof(CheckLogWarningAfterSubscribeObserverCollectionWhichSubscribeTest)}.");
            var nullableBinding = reactiveCollection.Subscribe(fakeObserverCollection);
            Assert.IsNull(nullableBinding);
            firstBinding.Dispose();
        }
        
        //TODO: make test for Action and test that ReactiveCollection implemented IEnumerable<T>
        
        [Test]
        public void CheckReactiveCollectionImplementedIEnumerableTest()
        {
            var reactiveCollectionInfo = typeof(ReactiveCollection<>);
            Assert.IsTrue(typeof(IEnumerable).IsAssignableFrom(reactiveCollectionInfo));
        }

        [Test]
        public void CheckSubscribeActionToReactiveCollectionTest()
        {
            var reactiveCollection = new ReactiveCollection<float>();
            var fakeObserverCollection = new FakeObserverCollection<float>();

            var binding = reactiveCollection.Subscribe(fakeObserverCollection.NotifyCollectionAdded,
                fakeObserverCollection.NotifyCollectionRemoved,
                fakeObserverCollection.NotifyCollectionClear);
            
            reactiveCollection.Add(12);
            fakeObserverCollection.CheckNotifyObserverAdded(new List<float>(), 12);
            reactiveCollection.Add(16);
            
            var actualResult = new float[reactiveCollection.Count];
            reactiveCollection.CopyTo(actualResult, 0);
            
            reactiveCollection.Remove(12);
            
            fakeObserverCollection.CheckNotifyObserverRemoved(actualResult, 12);
            
            reactiveCollection.Clear();
            
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            
            binding.Dispose();
            
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            
            reactiveCollection.Remove(16);
            fakeObserverCollection.CheckNotifyObserverRemoved(actualResult, 12);
            
            reactiveCollection.Add(15);
            fakeObserverCollection.CheckNotifyObserverAdded(actualResult, 16);
            
        }

        [Test]
        public void CheckUnsubscribeObserverReactiveCollectionTest()
        {
            var fakeObserverCollection = new FakeObserverCollection<float>();
            var reactiveCollection = new ReactiveCollection<float>();
            reactiveCollection.Subscribe(fakeObserverCollection);
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            reactiveCollection.Unsubscribe(fakeObserverCollection);
            reactiveCollection.Clear();
            fakeObserverCollection.CheckCountCalledNotifyObserverClear(1);
            
        }
    }

    internal class FakeObserverCollection<T> : IObserverCollection<T>
    {
        private int _countCalledNotifyObserverClear;
        private T _valueAdded;
        private IEnumerable<T> _collection;
        private T _valueRemoved;
        
        public void NotifyCollectionClear()
        {
            _countCalledNotifyObserverClear++;
        }

        public void NotifyCollectionAdded(IEnumerable<T> collection, T itemAdded)
        {
            _collection = collection;
            _valueAdded = itemAdded;
        }

        public void NotifyCollectionRemoved(IEnumerable<T> collection, T itemRemoved)
        {
            _collection = collection;
            _valueRemoved = itemRemoved;
        }

        internal void CheckNotifyObserverAdded(IEnumerable<T> actualWasCollection, T actualItem)
        {
            Assert.That(_collection, Is.EqualTo(actualWasCollection));
            Assert.That(_valueAdded, Is.EqualTo(actualItem));
        }

        internal void CheckNotifyObserverRemoved(IEnumerable<T> actualWasCollection, T actualItem)
        {
            Assert.That(_collection, Is.EqualTo(actualWasCollection));
            Assert.That(_valueRemoved, Is.EqualTo(actualItem));
        }

        internal void CheckCountCalledNotifyObserverClear(int count)
        {
            Assert.That(_countCalledNotifyObserverClear, Is.EqualTo(count));
        }

        public void Dispose()
        {
            
        }
    }
}