using Scripts.Infrastructure.Services.Factories.GameFactory;
using Scripts.Infrastructure.StateMachine.Game;
using Scripts.Infrastructure.StateMachine.States;
using Scripts.Infrastructure.StateMachine;
using Scirpts.Infrastructure.Services.StaticData.Game;
using Scirpts.Infrastructure.Services.StaticData.Level;
using Scripts.Infrastructure.Services.Factories.UIFactory;
using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Installer
{
    public class BootStrapInstance : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;

        public override void InstallBindings()
        {
            Debug.Log("Installer !");

            BindMonoServices();
            BindServices();

            BindGameStateMachine();
            BindGameStates();

            BootstrapGame();
        }

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(() => Container.InstantiatePrefabForComponent<ICoroutineRunner>(_coroutineRunner)).AsSingle();
            
            BindSceneLoader();
        }
        private void BindServices()
        {
            BindStaticDataService();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
        }

        private void BindSceneLoader()
        {
            ISceneLoader sceneLoader = new SceneLoader(Container.Resolve<ICoroutineRunner>());
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle();
        }

        private void BindStaticDataService()
        {
            IGameStaticDataService gameStaticDataService = new GameStaticDataService();
            gameStaticDataService.LoadData();
            Container.Bind<IGameStaticDataService>().FromInstance(gameStaticDataService).AsSingle();

            ILevelStaticDataService levelStaticDataService = new LevelStaticDataService();
            levelStaticDataService.LoadData();
            Container.Bind<ILevelStaticDataService>().FromInstance(levelStaticDataService).AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }
        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }

        private void BootstrapGame() =>
            Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
    }
}