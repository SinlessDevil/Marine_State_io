using UnityEngine.SceneManagement;
using Scripts.Infrastructure.StateMachine.Game;
using Scripts.StaticData;
using Scirpts.Infrastructure.Services.StaticData.Game;

namespace Scripts.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly GameStaticData _gameStaticData;
        private string _firstSceneName;

        public BootstrapState(IStateMachine<IGameState> stateMachine, ISceneLoader sceneLoader, IGameStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameStaticData = staticDataService.GameConfig();
        }

        public void Enter()
        {
            _firstSceneName = FirstSceneName();
            _sceneLoader.Load(_gameStaticData.InitialScene, OnLevelLoad);
        }

        public void Exit()
        {

        }

        private void OnLevelLoad() =>
            _stateMachine.Enter<LoadLevelState, string>(_firstSceneName);

        private string FirstSceneName()
        {
            string name = _gameStaticData.FirstScene;

#if UNITY_EDITOR
            if (_gameStaticData.CanLoadCurrentOpenedScene)
                name = SceneManager.GetActiveScene().name;
#endif
            return name;
        }
    }
}