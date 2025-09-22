using System;
using UnityEngine;
using NUnit.Framework;
using ClickerEngine.DI;
using UnityEngine.TestTools;

namespace ClickerEngineTest.DI
{

    public class DIContainerTests
    {

        [Test]
        public void CheckCreateDIContainerWithParentContainerTest()
        {
            var parentContainer = new DIContainer();

            parentContainer.RegisterSingleton(factory => new RegisterTestClass());
            
            var registerTestClass = parentContainer.Resolve<RegisterTestClass>();
            registerTestClass.TestCalledMethod();
            
            var container = new DIContainer(parentContainer);
            
            var result = container.Resolve<RegisterTestClass>();
            
            Assert.That(result, Is.EqualTo(registerTestClass));
            
        }

        [Test]
        public void CheckResolveServiceInDICotnainerTest()
        {
            var container = new  DIContainer();
            container.RegisterSingleton(factory => new RegisterTestClass());
            var registerTestClass = container.Resolve<RegisterTestClass>();
            Assert.IsTrue(registerTestClass is not null);

            container = new DIContainer();
            var instanceRegisterTestClass = new RegisterTestClass();
            container.RegisterInstance(instanceRegisterTestClass);
            registerTestClass = container.Resolve<RegisterTestClass>();
            Assert.IsTrue(registerTestClass is not null);
            
            container = new DIContainer();
            container.Register(factory => new RegisterTestClass());
            registerTestClass = container.Resolve<RegisterTestClass>();
            Assert.IsTrue(registerTestClass is not null);
            
        }
        
        [Test]
        public void CheckRegisterSingletonTest()
        {
            var container = new DIContainer();
            
            container.RegisterSingleton(factory => new RegisterTestClass());
            
            var registerTestClass = container.Resolve<RegisterTestClass>();
            registerTestClass.TestCalledMethod();
            
            var result = container.Resolve<RegisterTestClass>();
            result.CheckCountCalledChangeMethod(1);

            Assert.IsTrue(registerTestClass.Equals(result));
        }

        [Test]
        public void CheckRegisterTransientTest()
        {
            var container = new DIContainer();

            container.Register(factory => new RegisterTestClass());
            
            var registerTestClass = container.Resolve<RegisterTestClass>();
            
            var result = container.Resolve<RegisterTestClass>();
            registerTestClass.TestCalledMethod();
            
            result.CheckCountCalledChangeMethod(0);
            Assert.IsFalse(registerTestClass.Equals(result));
        }

        [Test]
        public void CheckRegisterInstanceTest()
        {
            var container = new DIContainer();

            var registerTestClass = new RegisterTestClass();
            registerTestClass.TestCalledMethod();
            
            container.RegisterInstance(registerTestClass);
            
            var result = container.Resolve<RegisterTestClass>();
            result.CheckCountCalledChangeMethod(1);
            
            Assert.IsTrue(registerTestClass.Equals(result));
        }

        [Test]
        public void CheckRegisterSingletonClassWhichRegisteredTest()
        {
            var key = ("", typeof(RegisterTestClass));
            
            var container = new DIContainer();
            
            container.RegisterSingleton(factory => new RegisterTestClass());
            
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterSingleton(factory => new RegisterTestClass());
        }

        [Test]
        public void CheckRegisterSingletonClassWhichRegisteredInParentContainerTest()
        {
            var key = ("", typeof(RegisterTestClass));
            var parentContainer = new DIContainer();
            
            parentContainer.RegisterSingleton(factory => new RegisterTestClass());
            
            var container = new DIContainer(parentContainer);
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterSingleton(factory => new RegisterTestClass());
        }

        [Test]
        public void CheckRegisterClassWhichRegisteredTest()
        {
            var key = ("", typeof(RegisterTestClass));
            
            var container = new DIContainer();
            
            container.Register(factory => new RegisterTestClass());
            
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.Register(factory => new RegisterTestClass());
        }

        [Test]
        public void CheckRegisterClassWhichRegisteredInParentContainerTest()
        {
            var key = ("", typeof(RegisterTestClass));
            var parentContainer = new DIContainer();
            
            parentContainer.Register(factory => new RegisterTestClass());
            
            var container = new DIContainer(parentContainer);
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.Register(factory => new RegisterTestClass());
            
        }
        
        [Test]
        public void CheckRegisterTransientWithTagTest()
        {
            var tagZero = "tag_zero";
            var tagOther = "tag_other";
            var container = new DIContainer();
            container.Register(factory => new RegisterTestClass(tagZero), tagZero);
            container.Register(factory => new RegisterTestClass(tagOther), tagOther);
            
            var resultZero = container.Resolve<RegisterTestClass>(tagZero);
            resultZero.CheckEqualsTagId(tagZero);
            
            var resultOther = container.Resolve<RegisterTestClass>(tagOther);
            resultOther.CheckEqualsTagId(tagOther);
        }

        [Test]
        public void CheckRegisterSingletonWithTagTest()
        {
            var tagDefault = "DefaultService";
            var tagCustom = "CustomService";
            var container = new DIContainer();
            
            container.RegisterSingleton(factory => new RegisterTestClass(), tagDefault);
            container.RegisterSingleton(factory => new RegisterTestClass(), tagCustom);
            
            var defaultService =  container.Resolve<RegisterTestClass>(tagDefault);
            defaultService.TestCalledMethod();

            var customService = container.Resolve<RegisterTestClass>(tagCustom);
            
            customService.CheckCountCalledChangeMethod(0);
            
            Assert.IsFalse(defaultService.Equals(customService));
        }

        [Test]
        public void CheckRegisterInstanceWithTagTest()
        {
            var tagDefault = "DefaultService";
            var tagCustom = "CustomService";

            var container = new DIContainer();
            var defaultService = new RegisterTestClass();
            var customService = new RegisterTestClass();
            
            container.RegisterInstance(defaultService, tagDefault);
            container.RegisterInstance(customService, tagCustom);
            
            var resultDefaultService = container.Resolve<RegisterTestClass>(tagDefault);
            resultDefaultService.TestCalledMethod();
            
            var resultCustomService = container.Resolve<RegisterTestClass>(tagCustom);
            resultCustomService.CheckCountCalledChangeMethod(0);
            resultCustomService.TestCalledMethod();
            
            resultDefaultService.CheckCountCalledChangeMethod(1);
            Assert.IsFalse(resultDefaultService.Equals(resultCustomService));
        }

        [Test]
        public void CheckRegisterSingletonClassWhichRegisteredWithTagTest()
        {
            
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            
            var container = new DIContainer();
            container.RegisterSingleton(factory => new RegisterTestClass(), tagCustom);
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterSingleton(factory => new RegisterTestClass(), tagCustom);
            
        }

        [Test]
        public void CheckRegisterSingletonClassWhichRegisteredInParentContainerWithTagTest()
        {
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            
            var parentContainer = new DIContainer();
            parentContainer.RegisterSingleton(factory => new RegisterTestClass(), tagCustom);
            
            var container = new DIContainer(parentContainer);
            
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterSingleton(factory => new RegisterTestClass(), tagCustom);
            
        }
        
        [Test]
        public void CheckRegisterClassWhichRegisteredWithTagTest()
        {
            
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            
            var container = new DIContainer();
            container.Register(factory => new RegisterTestClass(), tagCustom);
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.Register(factory => new RegisterTestClass(), tagCustom);
            
        }

        [Test]
        public void CheckRegisterClassWhichRegisteredInParentContainerWithTagTest()
        {
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            
            var parentContainer = new DIContainer();
            parentContainer.Register(factory => new RegisterTestClass(), tagCustom);
            
            var container = new DIContainer(parentContainer);
            
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.Register(factory => new RegisterTestClass(), tagCustom);
            
        }
        
        [Test]
        public void CheckRegisterInstanceClassWhichRegisteredWithTagTest()
        {
            
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            var customService = new RegisterTestClass();
            
            var container = new DIContainer();
            container.RegisterInstance(customService, tagCustom);
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterInstance(customService, tagCustom);
            
        }

        [Test]
        public void CheckRegisterInstanceClassWhichRegisteredInParentContainerWithTagTest()
        {
            var tagCustom = "CustomService";
            var key = (tagCustom, typeof(RegisterTestClass));
            var customService = new RegisterTestClass();
            
            var parentContainer = new DIContainer();
            parentContainer.RegisterInstance(customService, tagCustom);
            
            var container = new DIContainer(parentContainer);
            
            LogAssert.Expect(LogType.Warning, $"{typeof(RegisterTestClass).Name} contains with key: {key} in the DIContainer");
            container.RegisterInstance(customService, tagCustom);
            
        }

        [Test]
        public void CheckErrorWhenResolveClassWithErrorCreatedTest()
        {
            var container = new DIContainer();
            var errorMassage = "error: can't create register test class don't have params";
            container.RegisterSingleton(factory => new RegisterTestClass(true));
            LogAssert.Expect(LogType.Error, $"DI container error, when create object of type: {typeof(RegisterTestClass).Name}, error: {errorMassage}");
            Assert.Throws<Exception>(() => container.Resolve<RegisterTestClass>());
        }

        [Test]
        public void CheckLogWarningWhenNotFoundServiceInDIContainerTest()
        {
            var container = new DIContainer();
            
            LogAssert.Expect(LogType.Warning, "DI Container can't find type: " + typeof(RegisterTestClass));
            var result = container.Resolve<RegisterTestClass>();
            Assert.IsNull(result);
        }
    }
    
    internal class RegisterTestClass
    {
        private int _countCalledChangeMethod;
        private string _tagId;

        internal RegisterTestClass()
        {
            _tagId = "";
        }

        internal RegisterTestClass(bool invokeError)
        {
            if (invokeError)
                throw new Exception("error: can't create register test class don't have params");
        }
        
        internal RegisterTestClass(string tagId)
        {
            _tagId = tagId;
        }
        
        internal void TestCalledMethod()
        {
            _countCalledChangeMethod++;
        }

        internal void CheckEqualsTagId(string tagId)
        {
            Assert.That(_tagId, Is.EqualTo(tagId));
        }
        
        internal void CheckCountCalledChangeMethod(int count)
        {
            Assert.That(_countCalledChangeMethod, Is.EqualTo(count));
        }
    }
}