using Scenes;

namespace UI.Popups.MainMenu.Services
{
    public class AboutActionStrategy : AbstractMainMenuActionStrategy
    {
        public AboutActionStrategy(ISceneLoaderService sceneLoaderService) : base(sceneLoaderService)
        {
        }


        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.About, () => { });
        }
    }
}