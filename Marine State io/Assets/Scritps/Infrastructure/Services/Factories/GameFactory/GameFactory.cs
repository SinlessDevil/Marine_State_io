using Scripts.UI;
using Zenject;

namespace Scripts.Infrastructure.Services.Factories.GameFactory
{
    public class GameFactory : Factory, IGameFactory
    {
        private const string GameHudPath = "Hud/Game HUD";

        public GameFactory(IInstantiator instantiator) : base(instantiator)
        {
        }

        public GameHud CreateGameHud() =>
            InstantiateOnActiveScene(GameHudPath).GetComponent<GameHud>();
    }
}