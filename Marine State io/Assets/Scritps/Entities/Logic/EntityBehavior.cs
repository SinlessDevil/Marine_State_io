using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entities.Units;

namespace Scripts.Entities.Logic
{
    public abstract class EntityBehavior : MonoBehaviour
    {
        [SerializeField] protected List<Entity> _entities = new();
        private Transform _currentTransform;

        protected Entity _currentEntity;
        protected UnitFormationCoordinator _unitFormationCoordinator;

        public void Initialize(UnitFormationCoordinator unitFormationCoordinator)
        {
            _unitFormationCoordinator = unitFormationCoordinator;
        }

        private void Start()
        {
            UpdateReferenceToEntity();
            ActivatedEntityLogic();
        }

        protected void UpdateReferenceToEntity()
        {
            _entities.Clear();

            FindAllIslands();
            SortByDistance();
        }

        private void FindAllIslands()
        {
            var entities = FindObjectsOfType<Entity>();

            foreach (var entity in entities)
            {
                _entities.Add(entity);
            }

            _currentTransform = transform;
        }
        private void SortByDistance()
        {
            _entities.Sort((a, b) =>
                Vector3.Distance(a.transform.position, _currentTransform.position)
                .CompareTo(Vector3.Distance(b.transform.position, _currentTransform.position)));
            _entities.RemoveAt(0);
        }
        private void ActivatedEntityLogic()
        {
            StartCoroutine(ActionEntityRoutine());
        }

        protected abstract IEnumerator ActionEntityRoutine();
        protected abstract Entity GetTargetToAttack();
    }
}