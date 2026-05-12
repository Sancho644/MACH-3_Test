using System.Collections.Generic;
using Core.Yandex.Services;
using GameEvents;
using UnityEngine;

namespace UI.Windows.Records
{
    public class RecordsWindow : AbstractWindow
    {
        [SerializeField] private RectTransform recordsRoot;
        [SerializeField] private RecordItem recordItemPrefab;
        [SerializeField] private GoMainMenuButton goMainMenuButton;

        private ILeaderboardService _leaderboardService;

        public void Init(IGameEventsDispatcher gameEventsDispatcher , ILeaderboardService  leaderboardService)
        {
            _leaderboardService = leaderboardService;
            
            goMainMenuButton.Initialize(gameEventsDispatcher);
            
            Refresh();
        }

        private void Refresh()
        {
            _leaderboardService.LoadTop(OnLoaded);
        }

        private void OnLoaded(List<RecordEntry> recordEntries)
        {
            foreach (Transform child in recordsRoot.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var entry in recordEntries)
            {
                var recordItem = Instantiate(recordItemPrefab, recordsRoot);
                recordItem.Setup(entry);
            }
        }
    }
}