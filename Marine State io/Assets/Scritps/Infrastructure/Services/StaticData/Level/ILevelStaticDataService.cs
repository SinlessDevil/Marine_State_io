using Scripts.StaticData;

namespace Scirpts.Infrastructure.Services.StaticData.Level
{
    public interface ILevelStaticDataService
    {
        void LoadData();
        LevelStaticData GameConfig();
    }
}