using Extensions;
using Scripts.Entities.Units;
using Scripts.Islands;
using UnityEngine;

namespace Scripts.Factory.Pool
{
    public class SpawnerUnitPool : MonoBehaviour
    {
        [Header("--------- Pool Factory ---------")]
        [SerializeField] private int _poolCount;
        [SerializeField] private bool _autoExpand;

        private Unit _unitPlayerPrefabs;
        private Unit _unitEnemyPrefabs;

        private PoolMono<Unit> _poolUnitPlayer;
        private PoolMono<Unit> _poolUnitEnemy;

        public void Initialize(Unit unitPlayer, Unit unitEnemy)
        {
            _unitPlayerPrefabs = unitPlayer;
            _unitEnemyPrefabs = unitEnemy;

            InitPoolsMono();
            Asserts();
        }

        private void InitPoolsMono()
        {
            _poolUnitPlayer = new PoolMono<Unit>(_unitPlayerPrefabs, _poolCount, this.transform)
            {
                autoExpand = _autoExpand
            };

            _poolUnitEnemy = new PoolMono<Unit>(_unitEnemyPrefabs, _poolCount, this.transform)
            {
                autoExpand = _autoExpand
            };
        }

        private void Asserts()
        {
            _unitPlayerPrefabs.LogErrorIfComponentNull();
            _unitEnemyPrefabs.LogErrorIfComponentNull();
        }

        public PoolMono<Unit> GetPoolPlayer()
        {
            return _poolUnitPlayer;
        }

        public PoolMono<Unit> GetPoolEnemy()
        {
            return _poolUnitEnemy;
        }

        public Unit InitUnit(Vector3 pos, Sprite sprite ,Color color, Island target, PoolMono<Unit> pool)
        {
            var unit = pool.GetFreeElement();
            unit.transform.position = pos;
            unit.Initalize(sprite, color, target);
            return unit;
        }
    }
}