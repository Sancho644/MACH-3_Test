using System.IO;
using System.Runtime.Serialization;
using Core.Match3;
using StaticData;
using UnityEngine;

namespace Data.Services
{
    public class PlayerDataService : IPlayerDataService
    {
        private const string PlayerDataPrefsKey = "PlayerDataSave";

        public PlayerData Data { get; private set; }
        public bool IsDirty { get; private set; }
        public bool HasPlayerData => PlayerPrefs.HasKey(PlayerDataPrefsKey);

        private readonly BoardStaticData _boardConfig;

        public PlayerDataService(IStaticDataService staticDataService)
        {
            _boardConfig = staticDataService.GetBoardConfig();
        }
        
        public void SavePlayerData()
        {
            try
            {
                var jsonData = JsonUtility.ToJson(Data, true);
                PlayerPrefs.SetString(PlayerDataPrefsKey, jsonData);
                PlayerPrefs.Save();
                IsDirty = false;
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError(e);
            }
        }

        public void LoadPlayerData()
        {
            Data = new PlayerData();
            try
            {
                if (PlayerPrefs.HasKey(PlayerDataPrefsKey))
                {
                    var jsonData = PlayerPrefs.GetString(PlayerDataPrefsKey);
                    JsonUtility.FromJsonOverwrite(jsonData, Data);
                }
                else
                {
                    LoadDefaultPlayerData();
                }
            }
            catch (SerializationException e)
            {
                Debug.LogError(e);
            }
        }

        public void LoadDefaultPlayerData()
        {
            Data = CreateDefaultPlayerData();
            SavePlayerData();
        }

        private PlayerData CreateDefaultPlayerData()
        {
            var playerData = new PlayerData
            {
                Moves = _boardConfig.InitialMoves
            };

            return playerData;
        }
    }
}