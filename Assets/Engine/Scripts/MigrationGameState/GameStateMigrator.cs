using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClickerEngine.MigrationGameState
{
    public class GameStateMigrator : IGameStateMigrator
    {
        private readonly HashSet<IMigrationStep> _steps;
        private readonly HashSet<IParseGameState> _parsers;

        public GameStateMigrator()
        {
            _steps = new HashSet<IMigrationStep>();
            _parsers = new HashSet<IParseGameState>();
        }
        
        public void RegisterMigrationStep(IMigrationStep migrationStep)
        {
            _steps.Add(migrationStep);
        }

        public void RegisterParser(IParseGameState parser)
        {
            _parsers.Add(parser);
        }

        public T Migrate<T>(GameStateBase oldState) where T : GameStateBase
        {
            var dataResult = oldState;
            var version = oldState.Version;

            while (true)
            {
                var step = _steps.FirstOrDefault(step => step.FromVersion.Equals(version));
                
                if (step is null)
                    break;
                
                dataResult = step.Migrate(dataResult);
                version = step.ToVersion;
            }
            
            return (T)dataResult;
        }

        public GameStateBase ParseState(string rawJson, int gameStateVersion)
        {
            foreach (var parser in _parsers)
            {
                if(parser.Version.Equals(gameStateVersion))
                    return parser.ParseState(rawJson);
            }
            
            Debug.Log("Error: Unsupported Game State Version: " + gameStateVersion);
            return null;
        }
    }
}