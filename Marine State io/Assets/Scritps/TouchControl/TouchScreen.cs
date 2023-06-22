using UnityEngine;
using Scripts.Entities.Type;
using Scripts.Entities;
using Scripts.Entities.Units;
using Extensions;
using UniRx;

namespace Scripts.TouchControl
{
    public class TouchScreen : MonoBehaviour
    {
        [SerializeField] private PointTouch _pointTouchPrefab;
        [SerializeField] private LayerMask _layerMask;

        private LineRenderDraw _lineDraw;

        private Player _currentPlayer;
        private Transform _pointTouch;
        private Collider2D _colliderTarget;
        private Transform[] _pointsForLine;

        private UnitFormationCoordinator _unitFormationCoordinator;

        private readonly CompositeDisposable _disposable = new();

        private void Awake()
        {
            _lineDraw = GetComponentInChildren<LineRenderDraw>();
            _lineDraw.SetupLine();
            _lineDraw.enabled = false;

            _pointTouch = Instantiate(_pointTouchPrefab, transform.position, Quaternion.identity).transform;
            _pointTouch.Deactivate();
        }

        public void Initialize(UnitFormationCoordinator unitFormationCoordinator)
        {
            _unitFormationCoordinator = unitFormationCoordinator;
        }

        private void Start()
        {
            Asserts();
        }
        private void Asserts()
        {
            _pointTouchPrefab.LogErrorIfComponentNull();
            _lineDraw.LogErrorIfComponentNull();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TouchDown();
            if (Input.GetMouseButtonUp(0))
                TouchUp();
        }

        private void TouchDown()
        {
            _colliderTarget = GetTouchCollider();

            if (_colliderTarget == null)
                return;
          
            if (_colliderTarget.TryGetComponent(out Player player))
            {
                _currentPlayer = player;

                _pointsForLine = SetPointsForLine(player, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                _lineDraw.SetPoint(_pointsForLine);
                _lineDraw.SetStateLine(true);

                _pointTouch.Activate();

                SubscribeTouchStay();
            }
        }
        private void TouchUp()
        {
            _colliderTarget = GetTouchCollider();

            if (_colliderTarget != null && _currentPlayer != null)
            {
                if (_colliderTarget.TryGetComponent(out Entity entity))
                {
                    if(_currentPlayer != entity)
                    {
                        StartCoroutine(_unitFormationCoordinator.GenerateUnitsRoutine(_currentPlayer, entity,
                            _unitFormationCoordinator.GetSpawnerUnit().GetPoolPlayer()));
                    }
                }
            }

            _lineDraw.SetStateLine(false);

            _currentPlayer = null;
            _colliderTarget = null;

            _pointTouch.Deactivate();

            UnsubscribeTouchStay();
        }
        private void TouchStay()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _pointTouch.position = touchPosition;

            _lineDraw.DrawLine();
        }

        private void SubscribeTouchStay()
        {
            Observable.EveryUpdate().Subscribe(value =>
            {
                TouchStay();
            }).AddTo(_disposable);
        }
        private void UnsubscribeTouchStay()
        {
            _disposable.Clear();          
        }

        private Collider2D GetTouchCollider()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(touchPosition, _layerMask);

            return collider;
        }
        private Transform[] SetPointsForLine(Player player, Vector2 _touchPoint)
        {
            _currentPlayer = player;
            _pointTouch.position = _touchPoint;

            return new Transform[] { _currentPlayer.transform, _pointTouch.transform };
        }
    }
}