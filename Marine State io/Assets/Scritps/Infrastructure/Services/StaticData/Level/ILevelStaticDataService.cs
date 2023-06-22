using Scripts.StaticData;

namespace Scripts.Infrastructure.Services.StaticData.Level
{
    public interface ILevelStaticDataService
    {
        void LoadData();
        LevelStaticData GameConfig();
    }
}