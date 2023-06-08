using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.Infrastructure.Services.Factories
{
    public abstract class Factory
    {
        private readonly IInstantiator _instantiator;

        protected Factory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        protected GameObject InstantiateOnActiveScene(string uiRootPath)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string uiRootPath, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string uiRootPath, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, position, rotation, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiatePrefabOnActiveScene(GameObject prefab)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiatePrefab(GameObject prefab, Transform parent) =>
            _instantiator.InstantiatePrefab(prefab, parent);

        private GameObject MoveToCurrentScene(GameObject gameObject)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }
    }
}