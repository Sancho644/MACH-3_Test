using Data.Services;
using Game.Match3.GameEvents;
using GameEvents;
using TMPro;
using UnityEngine;

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
            var score = _playerStatsService.Score.ToString();
            var moves = _playerStatsService.Moves.ToString();

            scoreText.text = score;
            movesText.text = moves;
        }

        private void OnPlayerStatsChanged(PlayerStatsChangedEvent @event)
        {
            Refresh();
        }
    }
}