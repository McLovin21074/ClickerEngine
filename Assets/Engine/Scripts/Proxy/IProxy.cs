namespace ClickerEngine.Proxy
{
    public interface IProxy
    {
        
    }

    public interface IProxy<TEntityState> : IProxy
    {
        TEntityState OriginState { get; }
    }
}