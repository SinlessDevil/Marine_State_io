using Scripts.Entities.Units;

namespace Scripts.Entities.Type
{
    public class Enemy : Entity
    {
        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void OnEnable()
        {
            StartCoroutine(coroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(coroutine);
        }
    }
}