using Extensions;
using UnityEngine;

namespace Scripts.Islands
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fireworkFx;

        public SpriteRenderer islandBody;
        public SpriteRenderer iconPlayer;
        public Transform spawnPointUnitGroup;

        [HideInInspector] public SpriteOutline outline;

        private CircleCollider2D _cirecleCollider2D;
        private Animator _anim;

        private const string AnimSpawn = "AnimSpawn";

        public void Initialize(Sprite islandSprite, Sprite iconPlayer, Color islandColor)
        {
            InitComponent();

            if (islandSprite != null)
            {
                islandBody.sprite = islandSprite;
                outline.mainSprite.sprite = islandSprite;
            }

            islandBody.color = islandColor;

            if(iconPlayer != null)
            {
                this.iconPlayer.sprite = iconPlayer;
            }

            SetSizeCollider();

            Asserts();
        }

        private void InitComponent()
        {
            _cirecleCollider2D = GetComponent<CircleCollider2D>();
            _anim = GetComponent<Animator>();
            outline = GetComponentInChildren<SpriteOutline>();
        }
        private void SetSizeCollider()
        {
            float spriteRadius = islandBody.bounds.extents.x;
            _cirecleCollider2D.radius = spriteRadius;
        }
        private void Asserts()
        {
            islandBody.LogErrorIfComponentNull();
            iconPlayer.LogErrorIfComponentNull();
            spawnPointUnitGroup.LogErrorIfComponentNull();
            outline.LogErrorIfComponentNull();
        }

        public void ActivatedAnimation()
        {
            _anim.SetTrigger(AnimSpawn);
        }

        public void ActivatedFirework()
        {
            _fireworkFx.Play();
        }
    }
}