namespace ClickerEngine.EntryPoint
{
    public abstract class SceneExitParams
    {
        public SceneEnterParams TargetEnterParams { get; }
        
        public SceneExitParams(SceneEnterParams targetEnterParams)
        {
            TargetEnterParams = targetEnterParams;
        }
    }
}