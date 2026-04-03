using System.Threading.Tasks;
using Core.AssetManagement;
using Data.Services;
using GameEvents;
using StaticData;
using UI.Services.Windows;
using UI.Windows.Gameplay;
using UI.Windows.LowScore;
using UI.Windows.MainMenu;
using UI.Windows.ShowMainMenu;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Services.Factory
{
    public class UIFactoryService : IUIFactoryService
    {
        private const string GameCanvasPath = "GameCanvas";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticDataService;
        private readonly IPlayerStatsService _playerStatsService;
        private readonly IGameEventsDispatcher _gameEventsDispatcher;

        private Transform _uiRoot;

        public UIFactoryService(IAssets assets, IStaticDataService staticDataService,
            IGameEventsDispatcher gameEventsDispatcher, IPlayerStatsService playerStatsService)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _gameEventsDispatcher = gameEventsDispatcher;
            _playerStatsService = playerStatsService;
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
                gameplayWindow.Init(_staticDataService, _playerStatsService, _gameEventsDispatcher, this);
            }
        }

        public void CreateLowScoreWindow()
        {
            var windowConfig = _staticDataService.GetWindowConfig(WindowType.LowScore);
            var lowScoreWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as LowScoreWindow;
            if (lowScoreWindow != null)
            {
                lowScoreWindow.Init(_gameEventsDispatcher);
            }
        }

        public void CreateShowMainMenuWindow()
        {
            var windowConfig = _staticDataService.GetWindowConfig(WindowType.ShowMainMenu);
            var showMainMenuWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShowMainMenuWindow;
            if (showMainMenuWindow != null)
            {
                showMainMenuWindow.Initialize(_gameEventsDispatcher);
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