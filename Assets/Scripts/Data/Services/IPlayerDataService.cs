namespace Data.Services
{
    public interface IPlayerDataService
    {
        public PlayerData Data { get; }
        public bool IsDirty { get; }
        public void SavePlayerData();
        public void LoadPlayerData();
        public bool HasPlayerData { get; }
        public void LoadDefaultPlayerData();
    }
}