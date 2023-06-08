using Scripts.Entities.Units;
using UnityEngine;

namespace Scripts.Entities
{
    public abstract class Visitor : MonoBehaviour
    {
        public abstract void Accept(IUnitVisitor visitor);
    }
}