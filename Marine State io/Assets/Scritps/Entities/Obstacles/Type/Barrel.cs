using Extensions;
using Scripts.Entities.Units;
using UnityEngine;

namespace Scripts.Entities.Obstacles.Type
{
    public class Barrel : Obstacle
    {
        [SerializeField] private float _radius;
        [SerializeField] private ParticleSystem _explosionFx;

        private float _minRotationAngle = -180f;
        private float _maxRotationAngle = 180f;
        private float _minRotationSpeed = 10f;
        private float _maxRotationSpeed = 50f;

        private float _maxMovementOffset = 0.2f;
        private float _movementSpeed = 0.1f;

        private Quaternion _initialRotation;
        private Quaternion _targetRotation;
        private float _rotationSpeed;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        private void Start()
        {
            _explosionFx.LogErrorIfComponentNull();

            _initialRotation = transform.rotation;
            GenerateNewRotation();

            _initialPosition = transform.position;
            GenerateNewPosition();
        }
        private void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.01f)
                GenerateNewRotation();

            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                GenerateNewPosition();
        }
        private void GenerateNewRotation()
        {
            float randomAngle = Random.Range(_minRotationAngle, _maxRotationAngle);
            float randomSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);

            _targetRotation = _initialRotation * Quaternion.Euler(0f, 0f, randomAngle);
            _rotationSpeed = randomSpeed;
        }
        private void GenerateNewPosition()
        {
            float offsetX = Random.Range(-_maxMovementOffset, _maxMovementOffset);
            float offsetY = Random.Range(-_maxMovementOffset, _maxMovementOffset);

            _targetPosition = _initialPosition + new Vector3(offsetX, offsetY, 0f);
        }

        public override void Accept(IUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Explosion()
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, _radius);

            foreach (Collider2D colisionadar in objects)
            {
                if (colisionadar.gameObject.TryGetComponent(out Unit unit))
                {
                    unit.ResetUnit();
                }
            }

            DestroyBarrel();
        }

        private void DestroyBarrel()
        {
            _explosionFx.transform.Activate();
            _explosionFx.Play();
            _explosionFx.transform.SetParent(null);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}