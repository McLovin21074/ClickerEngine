namespace ClickerEngine.MigrationGameState
{
    public interface IGameStateMigrator
    {
        void RegisterMigrationStep(IMigrationStep migrationStep);
        void RegisterParser(IParseGameState parser);
        T Migrate<T>(GameStateBase oldState) where T : GameStateBase;

        GameStateBase ParseState(string rawJson, int gameStateVersion);
    }
}