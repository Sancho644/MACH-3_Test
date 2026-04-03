using System.Threading.Tasks;
using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Game.Services
{
    public class NewGameActionStrategy : AbstractGameActionStrategy
    {
        public NewGameActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService,
            IUIFactoryService uiFactoryService) : base(sceneLoaderService, windowService, uiFactoryService)
        {
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.Gameplay, OnLoadComplete);
        }

        private async void OnLoadComplete()
        {
            await CreateGameCanvas();
            WindowService.Open(WindowType.Gameplay);
        }

        private async Task CreateGameCanvas() =>
            await UIFactoryService.CreateGameCanvas();
    }
}