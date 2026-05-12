using System;
using System.Collections.Generic;
using StaticData;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Core.Yandex.Services
{
    public class YandexLeaderboardService : ILeaderboardService
    {
        private readonly IStaticDataService _staticDataService;

        private Action<List<RecordEntry>> _callback;

        public YandexLeaderboardService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SubmitScore(int score);

    [DllImport("__Internal")]
    private static extern void GetLeaderboardEntries();

#endif

        public void WriteScore(int score)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        SubmitScore(score);
#else
            Debug.Log($"Submit score: {score}");
#endif
        }

        public void LoadTop(Action<List<RecordEntry>> callback)
        {
            _callback = callback;

#if UNITY_WEBGL && !UNITY_EDITOR
        GetLeaderboardEntries();
#else
            callback?.Invoke(CreateMock());
#endif
        }

        public bool IsEnoughScore(int score, List<RecordEntry> recordEntries)
        {
            if (score <= 0)
                return false;

            var maxRecords = _staticDataService.GetBoardConfig().MaxRecords;
            if (recordEntries.Count <= 0 || recordEntries.Count < maxRecords)
                return true;

            return recordEntries[^1].score < score;
        }

        public void OnLeaderboardLoaded(string json)
        {
            Debug.Log(json);

            var wrapped = "{ \"entries\": " + json + "}";
            var data = JsonUtility.FromJson<RecordEntryWrapper>(wrapped);

            _callback?.Invoke(data.entries);
        }

        private List<RecordEntry> CreateMock()
        {
            return new List<RecordEntry>()
            {
                new() { name = "Player1", rank = 1, score = 1000 },
                new() { name = "Player2", rank = 2, score = 800 },
                new() { name = "Player3", rank = 3, score = 600 }
            };
        }
    }
}