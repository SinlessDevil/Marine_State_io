using UnityEngine;

namespace Extensions
{
    public static class ComponentExtensions
    {
        public static T ReplaceComponent<T>(this Component component) where T : Component
        {
            GameObject gameObject = component.gameObject;
            T newComponent = gameObject.AddComponent<T>();
            Object.Destroy(component);
            return newComponent;
        }
    }
}