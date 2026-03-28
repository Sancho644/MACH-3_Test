using Scenes;

namespace UI.Popups.MainMenu.Services
{
    public class NewGameActionStrategy : AbstractMainMenuActionStrategy
    {
        public NewGameActionStrategy(ISceneLoaderService sceneLoaderService) : base(sceneLoaderService)
        {
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.Gameplay, () => { });
        }
    }
}