using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StaticData;
using UnityEngine;

namespace Core.Records
{
    public class RecordsService : IRecordsService
    {
        private const string DefaultFileName = "record_scores.csv";

        private readonly IRecordsCsvParser _recordsCsvParser;
        private readonly IStaticDataService _staticDataService;
        private readonly string _sourcePath;
        private readonly string _persistentPath;

        private List<RecordEntry> _records;

        public IReadOnlyList<RecordEntry> Records => _records;

        public RecordsService(IStaticDataService staticDataService, string fileName = DefaultFileName)
        {
            _staticDataService = staticDataService;
            _recordsCsvParser = new RecordsCsvParser();

            _sourcePath = ResolveSourcePath(fileName);
            _persistentPath = Path.Combine(Application.persistentDataPath, fileName);
            _records = new List<RecordEntry>();

            Reload();
        }

        public bool IsEnoughScore(int score)
        {
            if (score <= 0)
                return false;

            var maxRecords = _staticDataService.GetBoardConfig().MaxRecords;
            if (_records.Count <= 0 || _records.Count < maxRecords)
                return true;

            return _records[^1].Score < score;
        }

        public bool TryAddRecord(int score, DateTime? date = null)
        {
            if (!IsEnoughScore(score))
            {
                return false;
            }

            _records.Add(new RecordEntry((date ?? DateTime.Now).Date, score, true));
            _records = Normalize(_records);
            Save();

            return true;
        }

        public void Reload()
        {
            EnsurePersistentFileExists();

            var csvContent = File.Exists(_persistentPath)
                ? File.ReadAllText(_persistentPath)
                : string.Empty;

            _records = Normalize(_recordsCsvParser.Parse(csvContent));

            if (!File.Exists(_persistentPath) || !HasSameContent(csvContent, _records))
                Save();
        }

        public void ResetRecordsStatus()
        {
            foreach (var entry in _records)
            {
                entry.SetStatus(false);
            }
        }

        private void EnsurePersistentFileExists()
        {
            if (File.Exists(_persistentPath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(_persistentPath) ?? Application.persistentDataPath);

            if (File.Exists(_sourcePath))
            {
                File.Copy(_sourcePath, _persistentPath, false);
                return;
            }

            Save();
        }

        private void Save()
        {
            var csvContent = _recordsCsvParser.Serialize(_records);
            File.WriteAllText(_persistentPath, csvContent);
        }

        private List<RecordEntry> Normalize(IEnumerable<RecordEntry> records)
        {
            var maxRecords = _staticDataService.GetBoardConfig().MaxRecords;

            return records
                .OrderByDescending(record => record.Score)
                .ThenByDescending(record => record.Date)
                .Take(maxRecords)
                .ToList();
        }

        private string ResolveSourcePath(string fileName)
        {
            var streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(streamingAssetsPath))
            {
                return streamingAssetsPath;
            }

            throw new Exception($"Could not find streaming assets at {streamingAssetsPath}");
        }

        private bool HasSameContent(string currentContent, IReadOnlyList<RecordEntry> records)
        {
            var normalizedContent = _recordsCsvParser.Serialize(records);
            return string.Equals(currentContent?.Trim(), normalizedContent.Trim(), StringComparison.Ordinal);
        }
    }
}