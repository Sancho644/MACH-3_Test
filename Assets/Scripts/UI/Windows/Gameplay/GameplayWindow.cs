using Core.Match3;
using Core.Match3.GameEvents;
using Core.Records;
using Data.Services;
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
        [SerializeField] private InputController inputController;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private IUIFactoryService _uiFactoryService;

        public void Init(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher, IUIFactoryService uiFactoryService, IRecordsService recordsService)
        {
            _uiFactoryService = uiFactoryService;
            _gameEventsDispatcher = gameEventsDispatcher;

            _gameEventsDispatcher.AddListener<OutOfMovesEvent>(OnOutOfMoves);

            boardController.Initialize(staticDataService, playerStatsService, gameEventsDispatcher, recordsService);
            countersController.Initialize(gameEventsDispatcher, playerStatsService);
            showMainMenuButton.Initialize(uiFactoryService);
            inputController.Initialize(gameEventsDispatcher);
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