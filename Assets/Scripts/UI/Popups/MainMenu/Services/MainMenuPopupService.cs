using System;
using System.Collections.Generic;
using Scenes;

namespace UI.Popups.MainMenu.Services
{
    public class MainMenuPopupService : IMainMenuPopupService
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly Dictionary<MainMenuButtonType, AbstractMainMenuActionStrategy> _mainMenuStrategies;

        public MainMenuPopupService(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;

            _mainMenuStrategies = new Dictionary<MainMenuButtonType, AbstractMainMenuActionStrategy>()
            {
                { MainMenuButtonType.NewGame, new NewGameActionStrategy(_sceneLoaderService) },
                { MainMenuButtonType.Records, new RecordsActionStrategy(_sceneLoaderService) },
                { MainMenuButtonType.About, new AboutActionStrategy(_sceneLoaderService) },
                { MainMenuButtonType.Exit, new ExitActionStrategy(_sceneLoaderService) }
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
    }
}