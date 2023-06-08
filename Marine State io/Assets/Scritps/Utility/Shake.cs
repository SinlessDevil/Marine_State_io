using UnityEngine;
using DG.Tweening;

namespace Scripts.Utility
{
    public static class Shake
    {
        public static void ShakeCamera(float intensity, float duration)
        {
            Camera.main.transform.DOShakePosition(duration, intensity);
        }

        public static void ShakeCamera(float intensity, float duration, Ease easeType)
        {
            Camera.main.transform.DOShakePosition(duration, intensity).SetEase(easeType);
        }

        public static void ShakeGameObject(this GameObject gameObject, float intensity, float duration)
        {
            gameObject.transform.DOShakePosition(duration, intensity);
        }

        public static void ShakeGameObject(this GameObject gameObject, float intensity, float duration, Ease easeType)
        {
            gameObject.transform.DOShakePosition(duration, intensity).SetEase(easeType);
        }
    }
}