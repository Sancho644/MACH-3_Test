using System;
using System.Collections.Generic;

namespace Core.Records
{
    public interface IRecordsService
    {
        public IReadOnlyList<RecordEntry> Records { get; }
        public bool IsEnoughScore(int score);
        public bool TryAddRecord(int score, DateTime? date = null);
        public void Reload();
        public void ResetRecordsStatus();
    }
}
