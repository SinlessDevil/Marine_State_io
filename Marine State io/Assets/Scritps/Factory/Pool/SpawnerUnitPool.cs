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

        private Unit _unitPrefabs;
        private PoolMono<Unit> _pool;

        public void Initialize(Unit unit)
        {
            _unitPrefabs = unit;

            InitPoolMono();
            Asserts();
        }

        private void InitPoolMono()
        {
            _pool = new PoolMono<Unit>(_unitPrefabs, _poolCount, this.transform);
            _pool.autoExpand = _autoExpand;
        }
        private void Asserts()
        {
            _unitPrefabs.LogErrorIfComponentNull();
        }

        public Unit InitUnit(Vector3 pos, Sprite sprite ,Color color, Island target)
        {
            var unit = _pool.GetFreeElement();
            unit.transform.position = pos;
            unit.Initalize(sprite, color, target);
            return unit;
        }
    }
}