using Scenes;

namespace UI.Popups.MainMenu.Services
{
    public abstract class AbstractMainMenuActionStrategy
    {
        protected readonly ISceneLoaderService SceneLoaderService;

        protected AbstractMainMenuActionStrategy(ISceneLoaderService sceneLoaderService)
        {
            SceneLoaderService = sceneLoaderService;
        }

        public abstract void Execute();
    }
}