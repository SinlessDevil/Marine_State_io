using Scripts.Entities.Units;

namespace Scripts.Entities.Obstacles.Type.Whirlpool
{
    public class WhirlpoolDetection : Obstacle
    {
        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}