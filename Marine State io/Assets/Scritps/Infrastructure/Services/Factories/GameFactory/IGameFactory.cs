using Scripts.UI;

namespace Scripts.Infrastructure.Services.Factories.GameFactory
{
    public interface IGameFactory
    {
        GameHud CreateGameHud();
    }
}