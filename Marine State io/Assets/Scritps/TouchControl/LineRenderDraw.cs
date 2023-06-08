using Extensions;
using UnityEngine;

namespace Scripts.TouchControl
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRenderDraw : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowPrefab;

        private Transform[] _points;
        private LineRenderer _line;
        private GameObject _arrow;

        public void SetupLine()
        {
            Asserts();

            _line = GetComponent<LineRenderer>();
            _line.positionCount = 0;

            _arrow = Instantiate(_arrowPrefab, transform.position, Quaternion.identity, this.transform);
            _arrow.SetActive(false);
        }
        public void SetStateLine(bool isActive)
        {
            _line.enabled = isActive;
            _arrow.SetActive(isActive);
            this.enabled = isActive;

            if (!isActive)
            {
                _points = null;
                _line.positionCount = 0;
            }
        }
        public void SetPoint(Transform[] point)
        {
            _line.positionCount = point.Length;
            _points = point;
            _arrow.transform.position = _points[1].position;
        }

        private void Asserts()
        {
            _arrowPrefab.LogErrorIfComponentNull();
        }

        public void DrawLine()
        {
            if (_points == null || _points.Length != 2)
                return;

            for (int i = 0; i < _points.Length; i++)
            {
                _line.SetPosition(i, _points[i].position);
            }

            UpdateTransfomArrow();
            TilingTexture();
        }

        private void TilingTexture()
        {
            float distance = Vector2.Distance(_line.GetPosition(0), _line.GetPosition(1));
            float newTiling = distance * 1.0f;
            Vector2 textureScale = _line.material.GetTextureScale("_MainTex");
            _line.material.SetTextureScale("_MainTex", new Vector2(newTiling, textureScale.y));
        }
        private void UpdateTransfomArrow()
        {
            _arrow.transform.position = _points[1].position;

            Vector3 direction = _points[1].position - _points[0].position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}