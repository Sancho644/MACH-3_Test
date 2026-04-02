using System.Threading.Tasks;
using Core.AssetManagement;
using GameEvents;
using JetBrains.Annotations;
using StaticData;
using UI.Services.Windows;
using UI.Windows.MainMenu;
using UI.Windows.MainMenu.Services;
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

        public UIFactoryService(IAssets assets, [CanBeNull] IStaticDataService staticDataService,
            IGameEventsDispatcher gameEventsDispatcher)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _gameEventsDispatcher = gameEventsDispatcher;
        }

        public void CreateMainMenu()
        {
            var windowConfig = _staticDataService.ForWindow(WindowType.MainMenu);
            var mainMenuWindow = Object.Instantiate(windowConfig.Prefab, _uiRoot) as MainMenuWindow;
            if (mainMenuWindow != null)
            {
                mainMenuWindow.Init(_gameEventsDispatcher);
            }
        }

        public void CreateGamePlayWindow()
        {
            var windowConfig = _staticDataService.ForWindow(WindowType.Gameplay);
            Object.Instantiate(windowConfig.Prefab, _uiRoot);
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