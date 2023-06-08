using System.Collections;
using UnityEngine;

namespace Scripts.Auxiliary
{
    [RequireComponent(typeof(Animator))]
    public class Marker : MonoBehaviour
    {
        [SerializeField] private float _waitTime = 5f;

        private Animator _anim;
        private static readonly int AnimHide = Animator.StringToHash("isHide");

        private void Start()
        {
            _anim = GetComponent<Animator>();
            StartCoroutine(StartDestroyRoutine());
        }

        private IEnumerator StartDestroyRoutine()
        {
            yield return new WaitForSecondsRealtime(_waitTime);
            ReadAnimHide();
            AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length + 0.2f;
            yield return new WaitForSecondsRealtime(animationLength);
            Destroy(gameObject);
        }

        private void ReadAnimHide()
        {
            _anim.SetTrigger(AnimHide);
        }
    }
}