using System;
using System.Collections.Generic;
using Scripts.Infrastructure.StateMachine.States;
using Zenject;

namespace Scripts.Infrastructure.Services.Factories.GameFactory
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(LoadLevelState)] = container.Resolve<LoadLevelState>,
                [typeof(BootstrapState)] = container.Resolve<BootstrapState>,
                [typeof(GameLoopState)] = container.Resolve<GameLoopState>,
            };
        }
    }
}