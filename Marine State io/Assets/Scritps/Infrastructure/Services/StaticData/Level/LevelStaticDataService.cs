using Scripts.StaticData;
using UnityEngine;

namespace Scripts.Infrastructure.Services.StaticData.Level
{
    public class LevelStaticDataService : ILevelStaticDataService
    {
        private const string LevelConfigPath = "StaticData/LevelConfig";

        private LevelStaticData _levelStaticData;

        public void LoadData() =>
            _levelStaticData = Resources.Load<LevelStaticData>(LevelConfigPath);

        public LevelStaticData GameConfig() =>
            _levelStaticData;
    }
}