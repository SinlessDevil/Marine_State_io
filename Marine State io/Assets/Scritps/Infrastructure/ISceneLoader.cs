using System;

namespace Scripts.Infrastructure
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLevelLoad);
    }
}