using UnityEngine;

namespace Data.Services
{
    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly IPlayerDataService _playerDataService;

        public int Score => _playerDataService.Data.Score;
        public int Moves => _playerDataService.Data.Moves;

        public PlayerStatsService(IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
        }

        public void AddScore(int amount)
        {
            if (amount <= 0)
                return;
            
            _playerDataService.Data.Score += amount;
        }

        public void AddMoves(int amount)
        {
            if (amount <= 0)
                return;
            
            _playerDataService.Data.Moves += amount;
        }

        public void SpendMoves(int amount)
        {
            var movesMax = Mathf.Max(0, Moves - amount);
            SetMoves(movesMax);
        }

        public void LoadDefaultData()
        {
            _playerDataService.LoadDefaultPlayerData();
        }

        private void SetMoves(int amount)
        {
            _playerDataService.Data.Moves = amount;
        }
    }
}