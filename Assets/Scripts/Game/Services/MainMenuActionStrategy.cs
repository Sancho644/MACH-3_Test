using System.Threading.Tasks;
using Data.Services;
using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Game.Services
{
    public class MainMenuActionStrategy : AbstractGameActionStrategy
    {
        private readonly IPlayerStatsService _playerStatsService;

        public MainMenuActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService,
            IUIFactoryService uiFactoryService, IPlayerStatsService playerStatsService) : base(sceneLoaderService, windowService, uiFactoryService)
        {
            _playerStatsService = playerStatsService;
        }

        public override void Execute()
        {
            SceneLoaderService.Load(SceneName.MainMenu, OnLoadComplete);
        }

        private async void OnLoadComplete()
        {
            _playerStatsService.LoadDefaultData();
            await CreateGameCanvas();
            WindowService.Open(WindowType.MainMenu);
        }

        private async Task CreateGameCanvas() =>
            await UIFactoryService.CreateGameCanvas();
    }
}