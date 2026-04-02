using System.Threading.Tasks;
using Core.AssetManagement;
using GameEvents;
using StaticData;
using UI.Services.Windows;
using UI.Windows.Gameplay;
using UI.Windows.MainMenu;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Services.Factory
{
    public class UIFactoryService : IUIFactoryService
    {
        private const string GameCanvasPath = "GameCanvas";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameEventsDispatcher _gameEventsDispatcher;

        private Transform _uiRoot;

        public UIFactoryService(IAssets assets, IStaticDataService staticDataService,
            IGameEventsDispatcher gameEventsDispatcher)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _gameEventsDispatcher = gameEventsDispatcher;
        }

        public void CreateMainMenu()
        {
            var windowConfig = _staticDataService.GetWindowConfig(WindowType.MainMenu);
            var mainMenuWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as MainMenuWindow;
            if (mainMenuWindow != null)
            {
                mainMenuWindow.Init(_gameEventsDispatcher);
            }
        }

        public void CreateGamePlayWindow()
        {
            var windowConfig = _staticDataService.GetWindowConfig(WindowType.Gameplay);
            var gameplayWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as GameplayWindow;
            if (gameplayWindow != null)
            {
                gameplayWindow.Init(_staticDataService);
            }
        }

        public async Task CreateGameCanvas()
        {
            if (_uiRoot != null)
                return;

            GameObject root = await _assets.Instantiate(GameCanvasPath);
            _uiRoot = root.transform;
        }
    }
}