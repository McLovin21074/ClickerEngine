using System;
using System.Linq;
using UnityEngine;
using ClickerEngine;
using NUnit.Framework;
using ClickerEngine.DI;
using System.Collections;
using UnityEngine.TestTools;
using ClickerEngine.Services;
using ClickerEngineTest.PrefabFakeTest;

namespace ClickerEngineTest.Services
{
    public class LoadServiceTest
    {
        [Test]
        public void CheckLoadPrefabTest()
        {
            var container = new DIContainer();
            var loadService = new LoadService(container);
            var prefabPath = "NUnitTests/FakeView";
            
            var prefab = loadService.LoadPrefabView<FakeView>(prefabPath);

            Assert.IsNotNull(prefab);
            Assert.That(prefab.GetType(), Is.EqualTo(typeof(FakeView)));
            
        }

        [Test]
        public void CheckErrorWhenLoadPrefabWithUnknownPathTest()
        {
            var container = new DIContainer();
            var loadService = new LoadService(container);
            var prefabPath = "tu tu tu";
            LogAssert.Expect(LogType.Error, $"Load service failed load prefab: {typeof(FakeView).Name} in path: {prefabPath}.");
            Assert.Throws<ArgumentException>(() => loadService.LoadPrefabView<FakeView>(prefabPath), $"Error loading prefab {typeof(FakeView).Name} in path: {prefabPath}.");
        }

        [Test]
        public void CheckGenericParameterIsAssignableFromIViewInLoadPrefabViewTest()
        {
            var methodInfo = typeof(LoadService).GetMethod("LoadPrefabView");
            var genericParameter = methodInfo.GetGenericArguments()[0];
            Assert.IsTrue(genericParameter.GetGenericParameterConstraints().Any(constraint => constraint.Equals(typeof(IView))));
        }

        [UnityTest]
        public IEnumerator CheckLoadingPrefabInSceneTest()
        {
            var prefabPath = "NUnitTests/FakeView";

            var container = new DIContainer();
            
            container.Register(factory => new FakeService());
            
            var loadService = new LoadService(container);
            
            var prefab = loadService.LoadPrefabView<FakeView>(prefabPath);

            var fakeView = loadService.LoadView<FakeService, FakeView>(prefab);
            
            Assert.That(fakeView.GetType(), Is.EqualTo(typeof(FakeView)));

            yield return new WaitForFixedUpdate();
            fakeView.CheckCountCalledFixedUpdate(1);
        }

        [Test]
        public void CheckErrorWhenLoadingViewButPrefabIsNullTest()
        {
            var container = new DIContainer();

            container.Register(factory => new FakeService());
            var loadService = new LoadService(container);
            
            LogAssert.Expect(LogType.Error, "Load service failed load view prefab is null");
            Assert.Throws<ArgumentNullException>(() => { loadService.LoadView<FakeService, FakeView>(null); }, "Failed load view prefab is null");
        }

        [Test]
        public void CheckFirstGenericParameterIsAssignableFromIServiceInLoadViewTest()
        {
            var methodInfo = typeof(LoadService).GetMethod("LoadView");
            var firstGenericParameter = methodInfo.GetGenericArguments()[0];
            Assert.IsTrue(firstGenericParameter.GetGenericParameterConstraints().Any(constraint => constraint.Equals(typeof(IService))));
        }

        [Test]
        public void CheckSecondGenericParameterIsAssignableFromIViewInLoadViewTest()
        {
            var methodInfo = typeof(LoadService).GetMethod("LoadView");
            var secondGenericParameter = methodInfo.GetGenericArguments()[1];
            Assert.IsTrue(secondGenericParameter.GetGenericParameterConstraints().Any(constraint => constraint.Equals(typeof(IView))));
        }

        [Test]
        public void CheckBindServiceInViewWhenLoadViewTest()
        {
            var prefabPath = "NUnitTests/FakeView";
            var container = new DIContainer();
            container.RegisterSingleton(factory => new FakeService());

            var loadService = new LoadService(container);

            var prefab = loadService.LoadPrefabView<FakeView>(prefabPath);
            var fakeView = loadService.LoadView<FakeService, FakeView>(prefab);
            
            var fakeService = container.Resolve<FakeService>();
            fakeView.CheckCountCalledBind(1);
            fakeView.CheckBindingService(fakeService);
        }

        [Test]
        public void CheckBindWithOtherServiceWhenLoadViewTest()
        {
            var prefabPath = "NUnitTests/FakeView";
            var container = new DIContainer();
            container.RegisterSingleton(factory => new FakeOtherService());

            var loadService = new LoadService(container);

            var prefab = loadService.LoadPrefabView<FakeView>(prefabPath);
            var fakeView = loadService.LoadView<FakeOtherService, FakeView>(prefab);
            
            var fakeService = container.Resolve<FakeOtherService>();
            fakeView.CheckBindingService(fakeService);
            
        }

        [Test]
        public void CheckLoadViewWithTagTest()
        {
            var prefabPath = "NUnitTests/FakeView";
            var tag = "customService";
            var container = new DIContainer();
            container.RegisterSingleton(factory => new FakeService(tag), tag);
            container.RegisterSingleton(factory => new FakeService());
            
            var loadService = new LoadService(container);
            
            var prefab = loadService.LoadPrefabView<FakeView>(prefabPath);
            var fakeView = loadService.LoadView<FakeService, FakeView>(prefab, tag);

            var fakeServiceWithTag = container.Resolve<FakeService>(tag);
            fakeView.CheckBindingService(fakeServiceWithTag);
        }
        
        //TODO: make test when prefab create in LoadView without called LoadPrefabView()
        
    }

    public class FakeService : IService
    {
        private string _tagId;

        internal FakeService(string tag = "")
        {
            _tagId = tag;
        }

        internal void CheckTag(string tag)
        {
            Assert.That(_tagId, Is.EqualTo(tag));
        }

    }

    internal class FakeOtherService : IService
    {
        
    }
}

