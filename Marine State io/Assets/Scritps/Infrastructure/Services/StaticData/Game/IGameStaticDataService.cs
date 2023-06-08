using Scripts.StaticData;

namespace Scirpts.Infrastructure.Services.StaticData.Game
{
    public interface IGameStaticDataService
    {
        void LoadData();
        GameStaticData GameConfig();
    }
}