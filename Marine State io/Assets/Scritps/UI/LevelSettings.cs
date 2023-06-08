using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts.StaticData;
using Scripts.Level;
using Scripts.Utility;
using Extensions;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class LevelSettings : MonoBehaviour
    {
        [Header("---- Level Settigns ----")]
        [Space(10)]
        [SerializeField] private Slider _scrollbarIslands;
        [SerializeField] private Slider _scrollbarPlayers;
        [SerializeField] private Slider _scrollbarWhirlpool;
        [SerializeField] private Slider _scrollbarBarrel;
        [Space(10)]
        [SerializeField] private Button _buttonStartGame;
        [SerializeField] private TMP_Text _textWarning;
        [Space(10)]
        [Header("---- Panel Main Player ----")]
        [SerializeField] private Image _iconPlayer;
        [SerializeField] private Image _imageColor;

        private LevelStaticData _levelStaticData;
        private LevelSetup _levelSetup;

        private Animator _anim;

        private const string AnimIsHide = "isHide";

        public void Initialize(LevelStaticData levelStaticData, LevelSetup levelSetup)
        {
            _levelStaticData = levelStaticData;
            _levelSetup = levelSetup;

            _textWarning.enabled = false;
            _anim = GetComponent<Animator>();

            InitSliderMaxValue();
            InitInfoPlayer();

            Asserts();
        }

        private void InitSliderMaxValue()
        {
            _scrollbarIslands.minValue = 2;
            _scrollbarIslands.maxValue = _levelStaticData.islandSprites.Length - 1;

            _scrollbarPlayers.minValue = 1;
            _scrollbarPlayers.maxValue = _levelStaticData.PlayerColors.Length;
        }
        private void InitInfoPlayer()
        {
            _iconPlayer.sprite = _levelStaticData.mainIconPlayerSprite;
            _imageColor.color = _levelStaticData.mainPlayerColor;
        }

        private void Asserts()
        {
            _scrollbarIslands.LogErrorIfComponentNull();
            _scrollbarPlayers.LogErrorIfComponentNull();
            _scrollbarWhirlpool.LogErrorIfComponentNull();
            _scrollbarBarrel.LogErrorIfComponentNull();
            _buttonStartGame.LogErrorIfComponentNull();
            _textWarning.LogErrorIfComponentNull();
            _iconPlayer.LogErrorIfComponentNull();
            _imageColor.LogErrorIfComponentNull();

            _levelStaticData.LogErrorIfComponentNull();
            _levelSetup.LogErrorIfComponentNull();
        }

        private void OnEnable()
        {
            _buttonStartGame.onClick.AddListener(StartLevel);
        }
        private void OnDisable()
        {
            _buttonStartGame.onClick.RemoveListener(StartLevel);
        }

        private void StartLevel()
        {
            if (!HasMoreIslandsThanPlayers())
                return;

            _levelSetup.StartLevel((int)_scrollbarIslands.value + 1, (int)_scrollbarPlayers.value,
                (int)_scrollbarWhirlpool.value, (int)_scrollbarBarrel.value);
            _buttonStartGame.enabled = false;

            ReadAnimHide();
        }

        private bool HasMoreIslandsThanPlayers()
        {
            bool hasMoreIslandsThanPlayers = _scrollbarIslands.value > _scrollbarPlayers.value;
            if (!hasMoreIslandsThanPlayers)
            {
                _textWarning.enabled = true;
                _textWarning.gameObject.ShakeGameObject(5f, 1.5f);
            }

            return hasMoreIslandsThanPlayers;
        }

        private void ReadAnimHide()
        {
            _anim.SetTrigger(AnimIsHide);
        }
    }
}