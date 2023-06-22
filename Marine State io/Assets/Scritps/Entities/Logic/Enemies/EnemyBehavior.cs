using System.Collections;
using UnityEngine;
using Scripts.Entities.Type;

namespace Scripts.Entities.Logic.Enemies
{
    public sealed class EnemyBehavior : EntityBehavior
    {
        private float _waitToAttack = 10f;

        private void Awake()
        {
            _currentEntity = GetComponent<Enemy>();
        }

        protected override IEnumerator ActionEntityRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_waitToAttack);

                if (_unitFormationCoordinator.IsGenerateUnits == false)
                {
                    Debug.Log("Attack");
                    AttackEntity();
                }
            }
        }
        protected override Entity GetTargetToAttack()
        {
            UpdateReferenceToEntity();

            var target = _currentEntity;

            foreach (Entity item in _entities)
            {
                if(item == null)
                {
                    continue;
                }


                if (item is Player || item is Empty || item is Ally)
                {
                    target = item;
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
            if(_currentEntity.CountUnit >= GetRandomNamberToAttack())
            {
                StartCoroutine(_unitFormationCoordinator.GenerateUnitsRoutine(_currentEntity, 
                    GetTargetToAttack(), _unitFormationCoordinator.GetSpawnerUnit().GetPoolEnemy()));
            }
            else
            {
                return;
            }
        }
        private int GetRandomNamberToAttack()
        {
            var rand = Random.Range(10, 15);
            return rand;
        }
    }
}