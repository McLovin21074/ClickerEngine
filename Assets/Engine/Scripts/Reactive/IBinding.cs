using System;

namespace ClickerEngine.Reactive
{
    public interface IBinding : IDisposable
    {
        void Binded();
    }
}