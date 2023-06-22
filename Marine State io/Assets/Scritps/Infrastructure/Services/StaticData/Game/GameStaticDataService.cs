using Scripts.StaticData;
using UnityEngine;

namespace Scripts.Infrastructure.Services.StaticData.Game
{
    public class GameStaticDataService : IGameStaticDataService
    {
        private const string GameConfigPath = "StaticData/GameConfig";

        private GameStaticData _gameStaticData;

        public void LoadData() =>
            _gameStaticData = Resources.Load<GameStaticData>(GameConfigPath);

        public GameStaticData GameConfig() =>
            _gameStaticData;
    }
}