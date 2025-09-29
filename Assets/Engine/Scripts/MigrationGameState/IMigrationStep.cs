namespace ClickerEngine.MigrationGameState
{
    public interface IMigrationStep
    {
        int FromVersion { get; }
        int ToVersion { get; }
        
        GameStateBase Migrate(GameStateBase oldState);
        
    }
}