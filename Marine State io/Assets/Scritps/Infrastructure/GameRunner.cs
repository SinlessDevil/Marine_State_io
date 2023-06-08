using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private SceneContext _emptySceneContext;

        private void Awake()
        {
            if (!FindObjectOfType<SceneContext>())
                Instantiate(_emptySceneContext);

            Destroy(gameObject);
        }
    }
}