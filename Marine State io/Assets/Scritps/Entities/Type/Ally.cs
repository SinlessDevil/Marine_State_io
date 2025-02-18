using Scripts.Entities.Units;

namespace Scripts.Entities.Type
{
    public class Ally : Entity
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