using System;
using NUnit.Framework;

namespace ClickerEngineTest.Reactive
{

    public class ReactivePropertyTests
    {
        [Test]
        public void CheckCreateReactivePropertyTest()
        {
            var value = new ReactiveProperty<int>(15);
            Assert.That(value.Value , Is.EqualTo(15));
        }

        [Test]
        public void CheckGetterReactivePropertyTest()
        {
            var value = new ReactiveProperty<string>("param param");
            Assert.That(value.Value, Is.EqualTo("param param"));
        }

        [Test]
        public void CheckSetterReactivePropertyTest()
        {
            var value = new ReactiveProperty<float>(10);
            Assert.That(value.Value, Is.EqualTo(10));
            value.Value += 5.5f;
            Assert.That(value.Value, Is.EqualTo(15.5f));
        }

        [Test]
        public void CheckHaveGenericParamsInReactivePropertyTest()
        {
            var classInfo = typeof(ReactiveProperty<>);
            Assert.That(classInfo.GetGenericArguments().Length, Is.GreaterThan(1));
        }

        [Test]
        public void CheckSubscribeToReactivePropertyTest()
        {
            var value = new ReactiveProperty<int>(10);
            var fakeObserver = new FakeObserver<int>();
            var binding = value.Subscribe(fakeObserver);
            value.Value = 6;
            fakeObserver.CheckChangedValue(6);
        }

        [Test]
        public void CheckUnSubscribeToReactivePropertyTest()
        {
            var value = new ReactiveProperty<int>(10);
            var fakeObserver = new FakeObserver<int>();
            var binding = value.Subscribe(fakeObserver);
            value.Value = 6;
            fakeObserver.CheckChangedValue(6);
            value.Unsubscribe(fakeObserver);
            value.Value = 78;
            fakeObserver.CheckChangedValue(6);
        }

        [Test]
        public void CheckDisposeBindingSubscribeTest()
        {
            var value = new ReactiveProperty<int>(10);
            var fakeObserver = new FakeObserver<int>();
            var binding = value.Subscribe(fakeObserver);
            value.Value = 6;
            fakeObserver.CheckChangedValue(6);
            binding.Dispose();
            value.Value = 687;
            fakeObserver.CheckChangedValue(6);
        }

        [Test]
        public void CheckSubscribeActionTest()
        {
            var value = new ReactiveProperty<float>();
            var isCalled = false;
            value.Subscribe(() => { isCalled = true; });
            value.Value = 77;
            
            Assert.IsTrue(isCalled);
        }

        [Test]
        public void CheckSubscribeActionValueTest()
        {
            
            var value = new ReactivePoperty<float>(65);
            var resultValue = 0;
            var binding = value.Subscribe(newValue => { resultValue = newValue; });
            value.Value = 45;
            Assert.That(resultValue, Is.EqualTo(45));
            binding.Dispose();
            value.Value = 55;
            Assert.That(resultValue, Is.EqualTo(45));
        }

        [Test]
        public void CheckReactivePropertyToStringTest()
        {
            var value = new ReactiveProperty<int>(12);
            
            Assert.That(value.ToString(), Is.EqualTo("12"));
        }
        
    }

    internal class FakeObserver<T> : IObserver<T>
    {
        private T _value;

        public void Handle(T newValue)
        {
            _value = newValue;
        }
        
        internal void CheckChangedValue(T actualValue)
        {
            Assert.That(_value, Is.EqualTo(actualValue));
        }
        
    }
}
