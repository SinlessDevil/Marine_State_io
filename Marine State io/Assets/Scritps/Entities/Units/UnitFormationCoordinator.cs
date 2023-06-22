using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Scripts.Factory.Pool;

namespace Scripts.Entities.Units
{
    public class UnitFormationCoordinator : MonoBehaviour
    {
        [SerializeField] private int _unitGroup = 3;
        [SerializeField] private float _speardTime = 0.5f;
        [SerializeField] private float _spawnOffset = 0.75f;

        public bool IsGenerateUnits { get; private set; }

        private SpawnerUnitPool _spawnerUnitPool;

        private void Start()
        {
            _spawnerUnitPool = GetComponent<SpawnerUnitPool>();
        }

        public SpawnerUnitPool GetSpawnerUnit()
        {
            return _spawnerUnitPool;
        }

        public IEnumerator GenerateUnitsRoutine(Entity mainEntity, Entity targetEntity, PoolMono<Unit> pool)
        {
            if (mainEntity.CountUnit != 0)
            {
                IsGenerateUnits = true;

                var PlayerCloneUnit = mainEntity.CountUnit;
                int risingUnit = 0;

                mainEntity.StopAccumulationUnit();
                mainEntity.CountUnit = 0;
                Debug.Log(mainEntity.ToString() + "  " + mainEntity.CountUnit);

                List<Unit> units = new();

                _unitGroup = SetSizeGroup(PlayerCloneUnit);

                while (0 < PlayerCloneUnit)
                {
                    PlayerCloneUnit -= _unitGroup;
                    risingUnit = _unitGroup;

                    if (PlayerCloneUnit < 0)
                    {
                        risingUnit = _unitGroup + PlayerCloneUnit;
                    }

                    for (int i = 0; i < risingUnit; i++)
                    {
                        var unit = _spawnerUnitPool.InitUnit(GetPositionInGroup(mainEntity.transform.position, i + 1),
                            mainEntity.Island.iconPlayer.sprite, mainEntity.Island.islandBody.color, targetEntity.Island, pool);
                        unit.transform.SetParent(mainEntity.Island.spawnPointUnitGroup);
                        units.Add(unit);
                    }

                    RotateToTarget(targetEntity.Island.transform.position, mainEntity.Island.spawnPointUnitGroup);
                    foreach (var unit in units)
                    {
                        unit.transform.SetParent(null);
                    }
                    mainEntity.Island.spawnPointUnitGroup.rotation = Quaternion.identity;
                    units.Clear();

                    yield return new WaitForSecondsRealtime(_speardTime);
                }

                mainEntity.StartAccumulationUnit();

                IsGenerateUnits = false;
            }

            Debug.Log("Stop");
        }

        private int SetSizeGroup(int currentCountUnits)
        {
            var sizeGroup = currentCountUnits switch
            {
                int n when n >= 50 => 5,
                int n when n >= 25 => 4,
                int n when n >= 15 => 3,
                int n when n >= 10 => 2,
                _ => 1,
            };
            return sizeGroup;
        }

        private Dictionary<int, float> groupOffsets = new()
        {
            { 2, 1f },
            { 3, -1f },
            { 4, 2f },
            { 5, -2f }
        };
        private Vector3 GetPositionInGroup(Vector3 target, int indexGroup)
        {
            if (groupOffsets.TryGetValue(indexGroup, out float yOffset))
            {
                return target + new Vector3(0f, _spawnOffset * yOffset, 0f);
            }

            return target;
        }

        private void RotateToTarget(Vector3 target, Transform body)
        {
            Vector3 direction = target - body.position;
            direction.z = 0f;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
    }
}