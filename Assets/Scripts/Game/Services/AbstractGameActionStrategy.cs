using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Game.Services
{
    public abstract class AbstractGameActionStrategy
    {
        protected readonly ISceneLoaderService SceneLoaderService;
        protected readonly IWindowService WindowService;
        protected readonly IUIFactoryService UIFactoryService;

        protected AbstractGameActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService, IUIFactoryService uiFactoryService)
        {
            SceneLoaderService = sceneLoaderService;
            WindowService = windowService;
            UIFactoryService = uiFactoryService;
        }

        public abstract void Execute();
    }
}