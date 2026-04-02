using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace UI.Windows.MainMenu.Services
{
    public abstract class AbstractMainMenuActionStrategy
    {
        protected readonly ISceneLoaderService SceneLoaderService;
        protected readonly IWindowService WindowService;
        protected readonly IUIFactoryService UIFactoryService;

        protected AbstractMainMenuActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService, IUIFactoryService uiFactoryService)
        {
            SceneLoaderService = sceneLoaderService;
            WindowService = windowService;
            UIFactoryService = uiFactoryService;
        }

        public abstract void Execute();
    }
}