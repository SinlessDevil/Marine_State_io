using Scripts.Entities.Type;
using System.Collections;
using UnityEngine;

namespace Scripts.Entities.Logic.Enemies
{
    public sealed class AllyBehavior : EntityBehavior
    {
        private float _waitToAttack = 8f;

        private void Awake()
        {
            _currentEntity = GetComponent<Ally>();
        }

        protected override IEnumerator ActionEntityRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_waitToAttack);

                if (!IsAttckEntity())
                {
                    AttackEntity();
                }
            }
        }
        protected override Entity GetTargetToAttack()
        {
            var target = _currentEntity;

            foreach (Entity item in _entities)
            {
                if (item is Empty || item is Enemy)
                {
                    return null;
                }
                else
                {
                    _currentEntity = item;
                    break;
                }
            }

            if (target == null)
            {
                Debug.LogError("Not a single enemy was found!");
                return null;
            }

            return target;
        }

        private void AttackEntity()
        {
            if (_currentEntity.CountUnit >= GetRandomNamberToAttack())
            {
                StartCoroutine(_unitFormationCoordinator.GenerateUnitsRoutine(_currentEntity, GetTargetToAttack(),
                    _unitFormationCoordinator.GetSpawnerUnit().GetPoolPlayer()));
            }
        }
        private bool IsAttckEntity()
        {
            return _unitFormationCoordinator.IsGenerateUnits;
        }
        private int GetRandomNamberToAttack()
        {
            var rand = Random.Range(10, 20);
            return rand;
        }
    }
} 