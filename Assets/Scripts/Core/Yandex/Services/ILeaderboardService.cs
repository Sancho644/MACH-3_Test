using System;
using System.Collections.Generic;

namespace Core.Yandex.Services
{
    public interface ILeaderboardService
    {
        public void WriteScore(int score);
        public void LoadTop(Action<List<RecordEntry>> callback);
        public bool IsEnoughScore(int score, List<RecordEntry> recordEntries);
    }
}