using ClickerEngine.DI;
using System.Collections;
using ClickerEngine.Reactive;

namespace ClickerEngine.EntryPoint
{
    public interface IEntryPoint
    {
        IEnumerator Initialization(DIContainer parentContainer, SceneEnterParams sceneEnterParams);
        IObservable<SceneExitParams> Run();
    }
}