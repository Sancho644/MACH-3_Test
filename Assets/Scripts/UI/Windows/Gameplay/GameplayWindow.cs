using Data.Services;
using Game.Match3;
using Game.Match3.GameEvents;
using GameEvents;
using StaticData;
using UI.Services.Factory;
using UI.Windows.Gameplay.Buttons;
using UnityEngine;

namespace UI.Windows.Gameplay
{
    [RequireComponent(typeof(BoardController), typeof(CountersController))]
    public class GameplayWindow : AbstractWindow
    {
        [SerializeField] private BoardController boardController;
        [SerializeField] private CountersController countersController;
        [SerializeField] private ShowMainMenuButton showMainMenuButton;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private IUIFactoryService _uiFactoryService;

        public void Init(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher, IUIFactoryService uiFactoryService)
        {
            _uiFactoryService = uiFactoryService;
            _gameEventsDispatcher = gameEventsDispatcher;

            _gameEventsDispatcher.AddListener<OutOfMovesEvent>(OnOutOfMoves);

            boardController.Initialize(staticDataService, playerStatsService, gameEventsDispatcher);
            countersController.Initialize(gameEventsDispatcher, playerStatsService);
            showMainMenuButton.Initialize(uiFactoryService);
        }

        private void OnDestroy()
        {
            _gameEventsDispatcher.RemoveListener<OutOfMovesEvent>(OnOutOfMoves);
        }

        private void OnOutOfMoves(OutOfMovesEvent @event)
        {
            _uiFactoryService.CreateLowScoreWindow();
        }
    }
}