using UnityEngine;
using ClickerEngine;
using NUnit.Framework;
using ClickerEngine.Services;

namespace ClickerEngineTest.PrefabFakeTest
{
    public class FakeView : MonoBehaviour, IView
    {
        private int _countCalledFixedUpdate;
        private IService _service;
        private int _countCalledBind;

        private void FixedUpdate()
        {
            _countCalledFixedUpdate++;
        }

        internal void CheckCountCalledFixedUpdate(int count)
        {
            Assert.That(_countCalledFixedUpdate, Is.EqualTo(count));
        }

        internal void CheckCountCalledBind(int count)
        {
            Assert.That(_countCalledBind, Is.EqualTo(count));
        }

        internal void CheckBindingService(IService service)
        {
            Assert.That(_service, Is.EqualTo(service));
        }

        public void Bind(IService service)
        {
            _service = service;
            _countCalledBind++;
        }
    }
}

