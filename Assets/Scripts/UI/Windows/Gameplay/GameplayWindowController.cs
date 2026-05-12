using Core.Match3;
using Core.Match3.Board;
using Core.Match3.Board.GameEvents;
using Core.Match3.GameEvents;
using Core.Match3.Hint;
using Core.Match3.Input;
using Core.Yandex.Services;
using Data.Services;
using GameEvents;
using StaticData;
using UI.Services.Factory;
using UI.Windows.Gameplay.Buttons;
using UnityEngine;

namespace UI.Windows.Gameplay
{
    [RequireComponent(typeof(BoardController), typeof(CountersController))]
    public class GameplayWindowController : AbstractWindow
    {
        [SerializeField] private BoardController boardController;
        [SerializeField] private CountersController countersController;
        [SerializeField] private ShowMainMenuButton showMainMenuButton;
        [SerializeField] private InputController inputController;
        [SerializeField] private HintController hintController;
        [SerializeField] private BoardView boardView;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private IUIFactoryService _uiFactoryService;

        public void Init(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher, IUIFactoryService uiFactoryService, ILeaderboardService leaderboardService)
        {
            _uiFactoryService = uiFactoryService;
            _gameEventsDispatcher = gameEventsDispatcher;

            _gameEventsDispatcher.AddListener<OutOfMovesEvent>(OnOutOfMoves);

            boardController.Initialize(staticDataService, playerStatsService, _gameEventsDispatcher, leaderboardService);
            hintController.Initialize(boardController, _gameEventsDispatcher, boardView);
            countersController.Initialize(_gameEventsDispatcher, playerStatsService);
            showMainMenuButton.Initialize(_uiFactoryService);
            inputController.Initialize(_gameEventsDispatcher);
            
            _gameEventsDispatcher.Dispatch(new WindowInitializationCompleteEvent());
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