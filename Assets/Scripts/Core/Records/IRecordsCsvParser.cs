using System.Collections.Generic;

namespace Core.Records
{
    public interface IRecordsCsvParser
    {
        public IReadOnlyList<RecordEntry> Parse(string csvContent);
        public string Serialize(IReadOnlyList<RecordEntry> records);
    }
}
