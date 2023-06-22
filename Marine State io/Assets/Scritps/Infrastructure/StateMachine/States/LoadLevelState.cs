using UnityEngine;
using Scripts.Infrastructure.Services.Factories.GameFactory;
using Scripts.Infrastructure.Services.Factories.UIFactory;
using Scripts.Infrastructure.Services.StaticData.Level;
using Scripts.Infrastructure.StateMachine.Game;
using Scripts.Entities.Units;
using Scripts.Entities.Obstacles.Type.Whirlpool;
using Scripts.Entities.Obstacles.Type;
using Scripts.StaticData;
using Scripts.Islands;
using Scripts.Factory.Pool;
using Scripts.TouchControl;
using Scripts.Level;
using Scripts.UI;

namespace Scripts.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly LevelStaticData _levelStaticData;

        private const string IslandPath = "Entities/Island";
        private const string UnitPlayerPath = "Entities/Units/UnitPlayer";
        private const string UnitEnemyPath = "Entities/Units/UnitEnemy";
        private const string BarrelPath = "Entities/Barrel";
        private const string WhirlpoolPath = "Entities/Whirlpool";

        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, 
            IGameFactory gameFactory, IUIFactory uiFactory, ILevelStaticDataService levelStaticDataService)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _levelStaticData = levelStaticDataService.GameConfig();
        }

        public void Enter(string payload)
        {
            Debug.Log("Enter Loading Level");

            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            Debug.Log("Exit Loading Level");
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            _uiFactory.CreateUiRoot();

            var unitPlayerPrefab = InitUnitPlayerPrefab();
            var unitEnemyPrefab = InitUnitEnemyPrefab();
            InitSpawnerUnit(unitPlayerPrefab, unitEnemyPrefab);

            var unitFormationCoordinator = InitUnitFormationUnit();
            InitTouchScreen(unitFormationCoordinator);

            var levelSetup = InitLevelSetup(unitFormationCoordinator);
            InitGameHud(levelSetup);
        }

        private void InitSpawnerUnit(Unit unitPlayer, Unit unitEnemy)
        {
            SpawnerUnitPool spawnerUnitPool = Object.FindObjectOfType<SpawnerUnitPool>();
            spawnerUnitPool.Initialize(unitPlayer, unitEnemy);
        }
        private Unit InitUnitPlayerPrefab()
        {
            var unit = Resources.Load<Unit>(UnitPlayerPath);
            return unit;
        }
        private Unit InitUnitEnemyPrefab()
        {
            var unit = Resources.Load<Unit>(UnitEnemyPath);
            return unit;
        }

        private UnitFormationCoordinator InitUnitFormationUnit()
        {
            var unitFormationCoordinator = Object.FindObjectOfType<UnitFormationCoordinator>();
            return unitFormationCoordinator;
        }

        private void InitTouchScreen(UnitFormationCoordinator unitFormationCoordinator)
        {
           TouchScreen touchScreen = Object.FindObjectOfType<TouchScreen>();
            touchScreen.Initialize(unitFormationCoordinator);
        }

        private LevelSetup InitLevelSetup(UnitFormationCoordinator unitFormationCoordinator)
        {
            var levelSetup = Object.FindObjectOfType<LevelSetup>();
            levelSetup.Initialize(_levelStaticData, InitIslandPrefab(), InitWhirlpoolPrefab(), InitBarrelPrefab(), unitFormationCoordinator);
            return levelSetup;
        }
        private Island InitIslandPrefab()
        {
            var island = Resources.Load<Island>(IslandPath);
            return island;
        }
        private WhirlpoolCenter InitWhirlpoolPrefab()
        {
            var whirlpool = Resources.Load<WhirlpoolCenter>(WhirlpoolPath);
            return whirlpool;
        }
        private Barrel InitBarrelPrefab()
        {
            var barrel = Resources.Load<Barrel>(BarrelPath);
            return barrel;
        }

        private void InitGameHud(LevelSetup levelSetup)
        {
            GameHud gameHud = _gameFactory.CreateGameHud();

            InitLevelSettings(levelSetup, _levelStaticData, gameHud);
            InitLoader(levelSetup, gameHud);
        }
        private void InitLevelSettings(LevelSetup levelSetup, LevelStaticData levelStaticData, GameHud gameHud)
        {
            LevelSettings levelSettings = gameHud.GetComponentInChildren<LevelSettings>();
            levelSettings.Initialize(levelStaticData, levelSetup);
        }
        private void InitLoader(LevelSetup levelSetup, GameHud gameHud)
        {
            LoadingDisplayer loadingDisplayer = gameHud.GetComponentInChildren<LoadingDisplayer>();
            loadingDisplayer.Initialized(levelSetup);
        }
    }
}