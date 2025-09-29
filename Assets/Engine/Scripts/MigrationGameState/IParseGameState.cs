namespace ClickerEngine.MigrationGameState
{
    public interface IParseGameState
    {
        int Version { get; }
        
        GameStateBase ParseState(string rawJson);
    }
}