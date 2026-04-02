using System;
using System.Collections.Generic;
using GameEvents;
using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;
using UI.Windows.MainMenu.GameEvents;

namespace UI.Windows.MainMenu.Services
{
    public class MainMenuWindowService : IMainMenuWindowService, IDisposable
    {
        private readonly IWindowService _windowService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IUIFactoryService _iuiFactoryServiceService;
        private readonly IGameEventsDispatcher _gameEventsDispatcher;
        private readonly Dictionary<MainMenuButtonType, AbstractMainMenuActionStrategy> _mainMenuStrategies;

        public MainMenuWindowService(ISceneLoaderService sceneLoaderService, IWindowService windowService,
            IUIFactoryService iuiFactoryServiceService, IGameEventsDispatcher gameEventsDispatcher)
        {
            _sceneLoaderService = sceneLoaderService;
            _windowService = windowService;
            _iuiFactoryServiceService = iuiFactoryServiceService;
            _gameEventsDispatcher = gameEventsDispatcher;
            
            _gameEventsDispatcher.AddListener<MainMenuActionEvent>(OnMainMenuAction);

            _mainMenuStrategies = new Dictionary<MainMenuButtonType, AbstractMainMenuActionStrategy>()
            {
                {
                    MainMenuButtonType.NewGame,
                    new NewGameActionStrategy(_sceneLoaderService, _windowService, _iuiFactoryServiceService)
                },
                {
                    MainMenuButtonType.Records,
                    new RecordsActionStrategy(_sceneLoaderService, _windowService, _iuiFactoryServiceService)
                },
                {
                    MainMenuButtonType.About,
                    new AboutActionStrategy(_sceneLoaderService, _windowService, _iuiFactoryServiceService)
                },
                {
                    MainMenuButtonType.Exit,
                    new ExitActionStrategy(_sceneLoaderService, _windowService, _iuiFactoryServiceService)
                }
            };
        }

        public void DoMainMenuAction(MainMenuButtonType buttonType)
        {
            if (_mainMenuStrategies.TryGetValue(buttonType, out var strategy))
            {
                strategy.Execute();
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, "Unhandled main menu action.");
        }

        public void Dispose()
        {
            _gameEventsDispatcher.RemoveListener<MainMenuActionEvent>(OnMainMenuAction);
        }

        private void OnMainMenuAction(MainMenuActionEvent @event)
        {
            DoMainMenuAction(@event.ButtonType);
        }
    }
}