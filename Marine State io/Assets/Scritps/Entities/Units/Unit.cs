using Extensions;
using Scripts.Entities.Obstacles;
using Scripts.Islands;
using UnityEngine;

namespace Scripts.Entities.Units
{
    [RequireComponent(typeof(UnitActions))]
    public class Unit : MonoBehaviour
    {
        private SpriteRenderer _bodySprite;
        private Transform _target;
        private TrailRenderer _trailRenderer;

        private UnitActions _unitAction;

        private void Awake()
        {
            InitComponent();
        }

        public void Initalize(Sprite spriteEntity, Color color, Island target)
        {
            _bodySprite.color = color;
            _target = target.transform;

            _unitAction.Initialize(spriteEntity, color);
        }
        public void InitComponent()
        {
            _unitAction = GetComponent<UnitActions>();
            _bodySprite = GetComponentInChildren<SpriteRenderer>();
            _trailRenderer = GetComponentInChildren<TrailRenderer>();

            Asserts();
        }
        private void Asserts()
        {
            _bodySprite.LogErrorIfComponentNull();
            _trailRenderer.LogErrorIfComponentNull();
        }

        private void FixedUpdate()
        {
            float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);

            if(distanceToTarget > 5f)
            {
                MoveToForward(6f);
            }
            else
            {
                MoveTo(_target.transform.position, 4f);
                RotateTo(_target.transform.position, 125f);
                ScaleTo(1f);
            }
        }

        private void MoveTo(Vector3 direction, float factorTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, Time.fixedDeltaTime * factorTime);
        }
        private void MoveToForward(float factorTime)
        {
            Vector3 direction = transform.right;
            transform.position += direction * Time.deltaTime * factorTime;
        }
        private void RotateTo(Vector3 target, float rotationSpeed)
        {
            Vector3 direction = target - transform.position;
            direction.z = 0f;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
        private void ScaleTo(float scalingSpeed)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.fixedDeltaTime * scalingSpeed);
        }

        public void UpdateTarget(Transform target)
        {
            _target = target;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent(out Entity entity))
            {
                if (entity.transform != _target)
                    return;

                entity.Accept(_unitAction);
                ResetUnit();

                return;
            }

            if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.Accept(_unitAction);
            }
        }
        public void ResetUnit()
        {
            if (_trailRenderer != null)
                _trailRenderer.Clear();

            if (_bodySprite != null)
                _bodySprite.color = Color.white;

            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.position = Vector3.zero;

            transform.Deactivate();
        }
    }
}