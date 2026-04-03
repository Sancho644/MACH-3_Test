using System;
using System.Collections.Generic;
using Data.Services;
using Game.Services.GameEvents;
using GameEvents;
using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;
using UI.Windows.MainMenu;

namespace Game.Services
{
    public class GameActionService : IGameActionService, IDisposable
    {
        private readonly IGameEventsDispatcher _gameEventsDispatcher;
        private readonly Dictionary<GameActionType, AbstractGameActionStrategy> _gameActionStrategies;

        public GameActionService(ISceneLoaderService sceneLoaderService, IWindowService windowService,
            IUIFactoryService iuiFactoryServiceService, IGameEventsDispatcher gameEventsDispatcher,
            IPlayerStatsService playerStatsService)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
            _gameEventsDispatcher.AddListener<GameActionEvent>(OnGameAction);

            _gameActionStrategies = new Dictionary<GameActionType, AbstractGameActionStrategy>()
            {
                {
                    GameActionType.NewGame,
                    new NewGameActionStrategy(sceneLoaderService, windowService, iuiFactoryServiceService)
                },
                {
                    GameActionType.Records,
                    new RecordsActionStrategy(sceneLoaderService, windowService, iuiFactoryServiceService)
                },
                {
                    GameActionType.About,
                    new AboutActionStrategy(sceneLoaderService, windowService, iuiFactoryServiceService)
                },
                {
                    GameActionType.Exit,
                    new ExitActionStrategy(sceneLoaderService, windowService, iuiFactoryServiceService)
                },
                {
                    GameActionType.MainMenu,
                    new MainMenuActionStrategy(sceneLoaderService, windowService, iuiFactoryServiceService,
                        playerStatsService)
                }
            };
        }

        public void DoGameAction(GameActionType actionType)
        {
            if (_gameActionStrategies.TryGetValue(actionType, out var strategy))
            {
                strategy.Execute();
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(actionType), actionType, "Unhandled main menu action.");
        }

        public void Dispose()
        {
            _gameEventsDispatcher.RemoveListener<GameActionEvent>(OnGameAction);
        }

        private void OnGameAction(GameActionEvent @event)
        {
            DoGameAction(@event.ActionType);
        }
    }
}