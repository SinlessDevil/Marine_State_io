using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Islands;
using Scripts.Entities.Type;
using Scripts.StaticData;
using Extensions;
using Scripts.Factory.Pool;
using Scripts.Entities.Obstacles.Type;
using Scripts.Entities.Obstacles.Type.Whirlpool;
using Scripts.Entities.Units;

namespace Scripts.Level
{
    public class LevelSetup : MonoBehaviour
    {
        public event Action OnStartGenerateWorldEvent;
        public event Action OnComplitedGeneratedWorldEvent;

        [Header("---------------------------------------- Level Settings ----------------------------------------")]
        [Space(10)]
        [Header("--- Mapa ---")]
        [Space(5)]
        [SerializeField] private float _offsetMapa = 2f;
        [SerializeField] private float _minDistanceToSpawn = 2f;
        [Space(5)]
        [Header("--- Other ---")]
        [Space(5)]
        [SerializeField] private GameObject _textYouPrefab;

        private int _countIsland;
        private int _countEnemy;
        private int _countWhirlpool;
        private int _countBarrel;

        private Vector3 _axisX;
        private Vector3 _axisY;

        private int _indexIslandSprite;
        private int _indesIconPlayerSprites;
        private int _indexPlayerColors;

        private Island[] _islands;
        private LevelStaticData _islandData;

        private Island _islandPrefab;
        private WhirlpoolCenter _whirlpoolCenter;
        private Barrel _barrelPrefab;

        private UnitFormationCoordinator _unitFormationCoordinator;

        public void Initialize(LevelStaticData islandData, Island island, WhirlpoolCenter whirlpool, Barrel barrel, UnitFormationCoordinator unitFormationCoordinator)
        {
            _islandData = islandData;
            _islandData.islandSprites.Shuffle();
            _islandData.PlayerColors.Shuffle();
            _islandData.iconPlayerSprites.Shuffle();

            _islandPrefab = island;
            _whirlpoolCenter = whirlpool;
            _barrelPrefab = barrel;
            _unitFormationCoordinator = unitFormationCoordinator;

            Asserts();
        }
        private void Asserts()
        {
            _textYouPrefab.LogErrorIfComponentNull();
            _islandData.LogErrorIfComponentNull();
            _islandPrefab.LogErrorIfComponentNull();
            _whirlpoolCenter.LogErrorIfComponentNull();
            _barrelPrefab.LogErrorIfComponentNull();
            _unitFormationCoordinator.LogErrorIfComponentNull();
        }
        public void StartLevel(int countIsland, int countEnemy, int countWhirlpool, int countBarrel)
        {
            OnStartGenerateWorldEvent?.Invoke();

            SetSettingLevel(countIsland, countEnemy, countWhirlpool, countBarrel);
            SetSizeMap();
            StartCoroutine(WaitEndingSpawnIslandRoutine());
        }

        private void SetSettingLevel(int countIsland, int countEnemy, int countWhirlpool, int countBarrel)
        {
            _countIsland = countIsland;
            _countEnemy = countEnemy;
            _countWhirlpool = countWhirlpool;
            _countBarrel = countBarrel;
        }
        private void SetSizeMap()
        {
            _axisX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            _axisY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            float mapWidth = _axisY.x - _axisX.x;
            float mapHeight = _axisY.y - _axisX.y;

            float offsetWidth = mapWidth * _offsetMapa / 100f;
            float offsetHeight = mapHeight * _offsetMapa / 100f;

            _axisX += new Vector3(offsetWidth, offsetHeight, 0f);
            _axisY -= new Vector3(offsetWidth, offsetHeight, 0f);
        }

        #region Methods Generate Island
        private IEnumerator SpawnObjectsLevelRoutine()
        {
            if( _countIsland != 0 )
                yield return GenerateObjectsRoutine<Island>(_countIsland, _islandPrefab);

            if(_countWhirlpool != 0)
                yield return GenerateObjectsRoutine<WhirlpoolCenter>(_countWhirlpool, _whirlpoolCenter);

            if(_countBarrel != 0)
                yield return GenerateObjectsRoutine<Barrel>(_countBarrel, _barrelPrefab);
        }

        private IEnumerator GenerateObjectsRoutine<T>(int count,T prefab) where T : Component
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = GenerateNewSpawnPos();
                var island = Instantiate(prefab, spawnPos, Quaternion.identity);

                Vector3 currentSpawnPos = spawnPos;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(currentSpawnPos, _minDistanceToSpawn);
                bool hasCollisions = colliders.Any(collider => collider.gameObject.GetComponent<Component>() != null);

                while (true)
                {
                    if (hasCollisions)
                    {
                        currentSpawnPos = GenerateNewSpawnPos();
                        colliders = Physics2D.OverlapCircleAll(currentSpawnPos, _minDistanceToSpawn);
                        hasCollisions = colliders.Any(collider => collider.gameObject.GetComponent<Component>() != null);
                        island.gameObject.transform.position = currentSpawnPos;
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        break;
                    }
                }

                yield return new WaitForSeconds(0.15f);
            }
        }

        private Vector3 GenerateNewSpawnPos()
        {
            float x = UnityEngine.Random.Range(_axisX.x, _axisY.x);
            float y = UnityEngine.Random.Range(_axisX.y, _axisY.y);
            float z = 0;
            return new Vector3(x, y, z);
        }
        #endregion

        #region Methods Initialized Island Entity
        private IEnumerator WaitEndingSpawnIslandRoutine()
        {
            yield return StartCoroutine(SpawnObjectsLevelRoutine());

            InitializedIsland();
        }

        private void InitializedIsland()
        {
            _islands = FindObjectsOfType<Island>();
            List<Island> islandsClone = new List<Island>(_islands);

            InitPlayer(islandsClone);
            InitEnemy(islandsClone);
            InityEmpty(islandsClone);

            StartCoroutine(ComplitedGenerateWorldRoutine());
        }
        private void InitPlayer(List<Island> islandsClone)
        {
            Player player = islandsClone[0].gameObject.AddComponent<Player>();
            player.Initialize(0);

            Island island = player.GetComponent<Island>();
            island.Initialize(GetIslandSprite(), _islandData.mainIconPlayerSprite, _islandData.mainPlayerColor);

            islandsClone.RemoveAt(0);

            Instantiate(_textYouPrefab, player.transform.position, Quaternion.identity);
        }
        private void InitEnemy(List<Island> islandsClone)
        {
            for (int i = 0; i < _countEnemy; i++)
            {
                Enemy enemy = islandsClone[i].gameObject.AddComponent<Enemy>();
                enemy.Initialize(0);

                Island island = enemy.GetComponent<Island>();
                island.Initialize(GetIslandSprite(), GetIconPalyerSprite(), GetPlayerCollor());
            }

            islandsClone.RemoveRange(0, _countEnemy);
        }
        private void InityEmpty(List<Island> islandsClone)
        {
            if (islandsClone.Count == 0)
                return;

            for (int i = 0; i < islandsClone.Count; i++)
            {
                Empty empty = islandsClone[i].gameObject.AddComponent<Empty>();
                empty.Initialize(GetCountUnit());

                Island island = empty.GetComponent<Island>();
                island.Initialize(GetIslandSprite(), null, _islandData.IslandByDefaultColor);
            }

            islandsClone.Clear();
        }

        private Sprite GetIslandSprite() => _islandData.islandSprites [_indexIslandSprite++];
        private Sprite GetIconPalyerSprite() => _islandData.iconPlayerSprites [_indesIconPlayerSprites++];
        private Color GetPlayerCollor() => _islandData.PlayerColors[_indexPlayerColors++];

        private int GetCountUnit()
        {
            if (_islandData.countsUnitByDefault.Length == 0)
            {
                Debug.LogError("Array of island sprites is empty!");
                return 0;
            }

            int randomIndex = UnityEngine.Random.Range(0, _islandData.countsUnitByDefault.Length);
            return _islandData.countsUnitByDefault[randomIndex];
        }
        private IEnumerator ComplitedGenerateWorldRoutine()
        {
            OnComplitedGeneratedWorldEvent?.Invoke();
            foreach (Island island in _islands)
            {
                island.transform.Deactivate();
            }
            yield return new WaitForSeconds(0.5f);
            foreach (Island island in _islands)
            {
                island.transform.Activate();
                island.ActivatedAnimation();
                island.ActivatedFirework();
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            Vector3 axisX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 axisY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            float mapWidth = axisY.x - axisX.x;
            float mapHeight = axisY.y - axisX.y;

            float offsetWidth = mapWidth * _offsetMapa / 100f;
            float offsetHeight = mapHeight * _offsetMapa / 100f;

            axisX += new Vector3(offsetWidth, offsetHeight, 0f);
            axisY -= new Vector3(offsetWidth, offsetHeight, 0f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(axisX.x, axisX.y, 0f), new Vector3(axisY.x, axisX.y, 0f));
            Gizmos.DrawLine(new Vector3(axisY.x, axisX.y, 0f), new Vector3(axisY.x , axisY.y, 0f));
            Gizmos.DrawLine(new Vector3(axisY.x, axisY.y, 0f), new Vector3(axisX.x, axisY.y, 0f));
            Gizmos.DrawLine(new Vector3(axisX.x, axisY.y, 0f), new Vector3(axisX.x, axisX.y, 0f));
        }
    }
}