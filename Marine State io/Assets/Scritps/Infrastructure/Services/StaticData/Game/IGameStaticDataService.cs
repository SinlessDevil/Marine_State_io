using Scripts.StaticData;

namespace Scripts.Infrastructure.Services.StaticData.Game
{
    public interface IGameStaticDataService
    {
        void LoadData();
        GameStaticData GameConfig();
    }
}