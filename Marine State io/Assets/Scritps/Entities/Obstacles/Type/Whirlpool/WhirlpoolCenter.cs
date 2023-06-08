using Scripts.Entities.Units;

namespace Scripts.Entities.Obstacles.Type.Whirlpool
{
    public class WhirlpoolCenter : Obstacle
    {
        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}