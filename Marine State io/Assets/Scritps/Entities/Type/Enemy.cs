using Scripts.Entities.Units;
using Scripts.Entities.Logic;
using Extensions;
using Scripts.Entities.Logic.Enemies;

namespace Scripts.Entities.Type
{
    public class Enemy : Entity
    {
        private EntityBehavior _entityBehavior;
        private UnitFormationCoordinator _unitFormationCoordinator;

        public void InitBehavior(UnitFormationCoordinator unitFormationCoordinator)
        {
            this.gameObject.AddComponent<EnemyBehavior>();
            _entityBehavior = GetComponent<EnemyBehavior>();

            _unitFormationCoordinator = unitFormationCoordinator;

            _entityBehavior.Initialize(_unitFormationCoordinator);
            _entityBehavior.LogErrorIfComponentNull();
        }

        private void Start()
        {
            StartAccumulationUnit();
        }

        private void OnDestroy()
        {
            StopAccumulationUnit();
        }

        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}