using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Game.Services
{
    public class AboutActionStrategy : AbstractGameActionStrategy
    {
        public AboutActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService, IUIFactoryService uiFactoryService) : base(sceneLoaderService, windowService, uiFactoryService)
        {
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.About, () => { });
        }
    }
}