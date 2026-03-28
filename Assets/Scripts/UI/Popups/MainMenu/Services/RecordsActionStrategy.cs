using Scenes;

namespace UI.Popups.MainMenu.Services
{
    public class RecordsActionStrategy : AbstractMainMenuActionStrategy
    {
        public RecordsActionStrategy(ISceneLoaderService sceneLoaderService) : base(sceneLoaderService)
        {
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.Records, () => { });
        }
    }
}