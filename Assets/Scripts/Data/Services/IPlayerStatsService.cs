namespace Data.Services
{
    public interface IPlayerStatsService
    {
        public int Score { get; }
        public int Moves { get; }
        public void AddScore(int amount);
        public void AddMoves(int amount);
        public void SpendMoves(int amount);
        public void LoadDefaultData();
    }
}