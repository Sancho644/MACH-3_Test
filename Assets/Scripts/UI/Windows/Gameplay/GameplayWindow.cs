using Data.Services;
using Game.Match3;
using GameEvents;
using StaticData;
using UnityEngine;

namespace UI.Windows.Gameplay
{
    [RequireComponent(typeof(BoardController), typeof(CountersController))]
    public class GameplayWindow : AbstractWindow
    {
        [SerializeField] private BoardController boardController;
        [SerializeField] private CountersController countersController;

        public void Init(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher)
        {
            boardController.Initialize(staticDataService, playerStatsService, gameEventsDispatcher);
            countersController.Initialize(gameEventsDispatcher, playerStatsService);
        }
    }
}