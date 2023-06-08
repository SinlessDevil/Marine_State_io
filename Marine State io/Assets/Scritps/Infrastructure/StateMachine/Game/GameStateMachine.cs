using Scripts.Infrastructure.Services.Factories.GameFactory;
using Scripts.Infrastructure.StateMachine.Game;
using Zenject;

namespace Scripts.Infrastructure.StateMachine.Game
{
    public class GameStateMachine : StateMachine<IGameState>, ITickable
    {
        public GameStateMachine(GameStateFactory gameStateFactory) : base(gameStateFactory)
        {
        }

        public void Tick()
        {
            Update();
        }
    }
}