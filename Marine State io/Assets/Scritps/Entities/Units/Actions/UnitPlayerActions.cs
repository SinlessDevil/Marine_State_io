using Scripts.Entities.Obstacles.Type.Whirlpool;
using Scripts.Entities.Obstacles.Type;
using Scripts.Entities.Type;
using Scripts.Utility;
using Extensions;
using Scripts.Entities.Logic;
using UnityEngine;

namespace Scripts.Entities.Units.Actions
{
    public sealed class UnitPlayerActions : UnitActions, IUnitVisitor
    {
        private Sprite _icon;
        private Color _color;

        public void Initialize(Sprite icon, Color color)
        {
            _icon = icon;
            _color = color;
        }

        public void Visit(Enemy entity)
        {
            entity.gameObject.ShakeGameObject(0.1f, 1f);
            SubtractUnitFromEnemy(entity);
        }
        public void Visit(Player entity)
        {
            entity.gameObject.ShakeGameObject(0.1f, 1f);
            AddUnitsToAllies(entity);
        }
        public void Visit(Ally entity)
        {
            entity.gameObject.ShakeGameObject(0.1f, 1f);
            AddUnitsToAllies(entity);
        }
        public void Visit(Empty entity)
        {
            entity.gameObject.ShakeGameObject(0.1f, 1f);
            SubtractUnitFromEnemy(entity);
        }
        public void Visit(Barrel entity)
        {
            Shake.ShakeCamera(1f, 1f);
            entity.Explosion();
        }
        public void Visit(WhirlpoolCenter entity)
        {
            Unit currentUnit = GetComponent<Unit>();
            currentUnit.ResetUnit();
        }
        public void Visit(WhirlpoolDetection entity)
        {
            Unit currentUnit = GetComponent<Unit>();
            currentUnit.UpdateTarget(entity.transform);
        }

        private void SubtractUnitFromEnemy(Entity entity)
        {
            entity.CountUnit -= 1;
            if (entity.CountUnit < 0)
            {
                CaptureTerritory(entity);
            }
        }
        private void CaptureTerritory(Entity entity)
        {
            if (!entity.gameObject.TryGetComponent<Player>(out var existingPlayerComponent))
            {
                var newPlayerComponent = entity.ReplaceComponent<Player>();

                newPlayerComponent.Initialize(0);

                newPlayerComponent.Island.Initialize(null, _icon, _color);
                newPlayerComponent.Island.ActivatedAnimation();
                newPlayerComponent.Island.ActivatedFirework();

                if (entity.gameObject.TryGetComponent<EntityBehavior>(out var entityBehavior))
                {
                    Destroy(entityBehavior);
                }
            }
            else
            {
                AddUnitsToAllies(existingPlayerComponent);
            }
        }
        private void AddUnitsToAllies(Entity entity)
        {
            entity.CountUnit += 1;
        }
    }
}