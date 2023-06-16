using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Scripts.Entities.Type;
using Scripts.Factory.Pool;
using System;
using static UnityEngine.GraphicsBuffer;

namespace Scripts.Entities.Units
{
    public class UnitFormationCoordinator : MonoBehaviour
    {
        [SerializeField] private int _unitGroup = 3;
        [SerializeField] private float _speardTime = 0.5f;
        [SerializeField] private float _spawnOffset = 0.75f;

        private SpawnerUnitPool _spawnerUnitPool;

        private void Start()
        {
            _spawnerUnitPool = GetComponent<SpawnerUnitPool>();
        }

        public IEnumerator GenerateUnitsRoutine(Player player, Entity entity)
        {
            if (player.CountUnit != 0)
            {
                var PlayerCloneUnit = player.CountUnit;
                int risingUnit = 0;

                player.StopAccumulationUnit();
                player.CountUnit = 0;

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
                        var unit = _spawnerUnitPool.InitUnit(GetPositionInGroup(player.transform.position, i + 1),
                            player.Island.iconPlayer.sprite, player.Island.islandBody.color, entity.Island);
                        unit.transform.SetParent(player.Island.spawnPointUnitGroup);
                        units.Add(unit);
                    }

                    RotateToTarget(entity.Island.transform.position, player.Island.spawnPointUnitGroup);
                    foreach (var unit in units)
                    {
                        unit.transform.SetParent(null);
                    }
                    player.Island.spawnPointUnitGroup.rotation = Quaternion.identity;
                    units.Clear();

                    yield return new WaitForSecondsRealtime(_speardTime);
                }

                player.StartAccumulationUnit();
            }
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

        private Dictionary<int, float> groupOffsets = new Dictionary<int, float>()
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