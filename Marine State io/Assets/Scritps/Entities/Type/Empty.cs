using Scripts.Entities.Units;

namespace Scripts.Entities.Type
{
    public class Empty : Entity
    {
        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}