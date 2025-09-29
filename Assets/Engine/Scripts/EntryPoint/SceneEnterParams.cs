namespace ClickerEngine.EntryPoint
{
    public abstract class SceneEnterParams
    {
        public T As<T>() where T : SceneEnterParams
        {
            return (T)this;
        }
    }
}