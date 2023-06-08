using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "UI/UiRoot";

        private readonly IInstantiator _instantiator;

        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator) : base(instantiator)
        {
            _instantiator = instantiator;
        }

        public void CreateUiRoot()
        {
            _uiRoot = InstantiateOnActiveScene(UiRootPath).transform;
        }
    }
}