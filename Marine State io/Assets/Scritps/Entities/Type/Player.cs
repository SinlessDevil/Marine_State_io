using Scripts.Entities.Units;

namespace Scripts.Entities.Type
{
    public class Player : Entity
    {
        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void Start()
        {
            StartAccumulationUnit();
        }

        private void OnDestroy()
        {
            StopAccumulationUnit();
        }
    }
}