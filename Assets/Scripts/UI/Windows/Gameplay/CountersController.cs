using Core.Match3.GameEvents;
using Data.Services;
using GameEvents;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.Windows.Gameplay
{
    public class CountersController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI movesText;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private IPlayerStatsService _playerStatsService;

        public void Initialize(IGameEventsDispatcher gameEventsDispatcher, IPlayerStatsService playerStatsService)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
            _playerStatsService = playerStatsService;

            _gameEventsDispatcher.AddListener<PlayerStatsChangedEvent>(OnPlayerStatsChanged);

            Refresh();
        }

        private void OnDestroy()
        {
            _gameEventsDispatcher.RemoveListener<PlayerStatsChangedEvent>(OnPlayerStatsChanged);
        }

        private void Refresh()
        {
            var formattedScore = NumberFormatter.Format(_playerStatsService.Score);
            var formattedMoves = NumberFormatter.Format(_playerStatsService.Moves);

            scoreText.text = formattedScore;
            movesText.text = formattedMoves;
        }

        private void OnPlayerStatsChanged(PlayerStatsChangedEvent @event)
        {
            Refresh();
        }
    }
}