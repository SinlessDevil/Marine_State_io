using UnityEngine;
using Extensions;

namespace Scripts.Factory.Generic
{
    public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _pointToSpawn;

        private void Start()
        {
            Asserts();
        }

        private void Asserts()
        {
            _prefab.LogErrorIfComponentNull();
            _pointToSpawn.LogErrorIfComponentNull();
        }

        public T GetNewInstance()
        {
            Vector3 pos = _pointToSpawn.position;
            return Instantiate(_prefab,pos,Quaternion.identity,_pointToSpawn.parent);
        }
    }
}