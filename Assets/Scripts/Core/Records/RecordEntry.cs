using System;

namespace Core.Records
{
    [Serializable]
    public class RecordEntry
    {
        public DateTime Date { get; }
        public int Score { get; }
        public bool NewRecord { get; private set; }

        public RecordEntry(DateTime date, int score, bool newRecord = false)
        {
            Date = date.Date;
            Score = score;
            NewRecord = newRecord;
        }

        public void SetStatus(bool value)
        {
            NewRecord = value;
        }
    }
}
