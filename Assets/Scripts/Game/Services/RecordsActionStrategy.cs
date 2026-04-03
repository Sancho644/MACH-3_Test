using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Game.Services
{
    public class RecordsActionStrategy : AbstractGameActionStrategy
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