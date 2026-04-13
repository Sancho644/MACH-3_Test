using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Records
{
    public class RecordsCsvParser : IRecordsCsvParser
    {
        private const string Header = "date,score";
        private const string DateFormat = "yyyy-MM-dd";

        public IReadOnlyList<RecordEntry> Parse(string csvContent)
        {
            List<RecordEntry> records = new();

            if (string.IsNullOrWhiteSpace(csvContent))
                return records;

            var lines = csvContent
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (i == 0 && line.Equals(Header, StringComparison.OrdinalIgnoreCase))
                    continue;

                var columns = line.Split(',');
                if (columns.Length != 2)
                    continue;

                if (!DateTime.TryParseExact(columns[0].Trim(), DateFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime date))
                {
                    continue;
                }

                if (!int.TryParse(columns[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int score))
                    continue;

                records.Add(new RecordEntry(date, score));
            }

            return records;
        }

        public string Serialize(IReadOnlyList<RecordEntry> records)
        {
            StringBuilder builder = new();
            builder.AppendLine(Header);

            foreach (var record in records)
            {
                builder.Append(record.Date.ToString(DateFormat, CultureInfo.InvariantCulture));
                builder.Append(',');
                builder.Append(record.Score.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine();
            }

            return builder.ToString().TrimEnd('\r', '\n');
        }
    }
}
