using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace UI.Windows.MainMenu.Services
{
    public class RecordsActionStrategy : AbstractMainMenuActionStrategy
    {
        public RecordsActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService, IUIFactoryService uiFactoryService) : base(sceneLoaderService, windowService, uiFactoryService)
        {
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.Records, () => { });
        }
    }
}