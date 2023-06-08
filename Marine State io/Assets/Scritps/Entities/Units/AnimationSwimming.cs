using UnityEngine;

namespace Scripts.Entities.Units
{
    public class AnimationSwimming : MonoBehaviour
    {
        [SerializeField] private float _wiggleSpeed;
        [SerializeField] private float _wiggleMagnitude;

        public void Update()
        {
            transform.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleMagnitude);
        }
    }
}