using Scripts.Entities.Obstacles.Type;
using Scripts.Entities.Obstacles.Type.Whirlpool;
using Scripts.Entities.Type;
using UnityEngine;

namespace Scripts.Entities.Units
{
    public interface IUnitVisitor
    {
        void Initialize(Sprite icon, Color color);

        void Visit(Enemy entity);
        void Visit(Player entity);
        void Visit(Empty entity);
        void Visit(Ally entity);
        void Visit(Barrel entity);
        void Visit(WhirlpoolCenter entity);
        void Visit(WhirlpoolDetection entity);
    }
}