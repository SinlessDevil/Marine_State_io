using UnityEngine;
using DG.Tweening;

namespace Scripts.Islands
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteOutline : MonoBehaviour
    {
        [SerializeField] private Color _colorByDefault;
        [SerializeField] private Color _colorByTarget;
        [SerializeField] private float _animationDuration = 1.0f;

        [HideInInspector] public SpriteRenderer mainSprite;

        private void Awake()
        {
            mainSprite = GetComponent<SpriteRenderer>();
        }

        public void SetColorByDefault()
        {
            mainSprite.DOColor(_colorByDefault, _animationDuration);
        }

        public void SetColorByTarget()
        {
            mainSprite.DOColor(_colorByTarget, _animationDuration);
        }
    }
}