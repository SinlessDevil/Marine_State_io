using System.Collections;
using UnityEngine;
using TMPro;
using Scripts.Level;
using Extensions;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class LoadingDisplayer : MonoBehaviour
    {
        private Animator _anim;
        private TMP_Text _textLoading;

        private LevelSetup _levelSetup;

        private const string TextLoading = "Loading";

        private const string AnimIsShow = "IsShow";
        private const string AnimIsHide = "IsHide";

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _textLoading = GetComponentInChildren<TMP_Text>();

            _textLoading.text = TextLoading;
            StartCoroutine(StartAniTextLoadingRoutine());

            DontDestroyOnLoad(this);
        }
        private IEnumerator StartAniTextLoadingRoutine()
        {
            int tick = 0;
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                _textLoading.text += " .";
                tick++;
                if (tick == 4)
                {
                    _textLoading.text = TextLoading;
                    tick = 0;
                }
            }
        }

        public void Initialized(LevelSetup levelSetup)
        {
            _levelSetup = levelSetup;
            Subscribe();

            Asserts();
        }

        private void Asserts()
        {
            _levelSetup.LogErrorIfComponentNull();
            _textLoading.LogErrorIfComponentNull();
        }

        private void Subscribe()
        {
            _levelSetup.OnComplitedGeneratedWorldEvent += ReadAnimShow;
            _levelSetup.OnStartGenerateWorldEvent += ReadAnimHide;
        }
        private void OnDisable()
        {
            _levelSetup.OnComplitedGeneratedWorldEvent -= ReadAnimShow;
            _levelSetup.OnStartGenerateWorldEvent -= ReadAnimHide;
        }
        private void ReadAnimShow()
        {
            _anim.SetTrigger(AnimIsShow);
        }
        private void ReadAnimHide()
        {
            _anim.SetTrigger(AnimIsHide);
        }
    }
}