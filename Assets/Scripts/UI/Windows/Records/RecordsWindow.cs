using Core.Records;
using GameEvents;
using UnityEngine;

namespace UI.Windows.Records
{
    public class RecordsWindow : AbstractWindow
    {
        [SerializeField] private RectTransform recordsRoot;
        [SerializeField] private RecordItem recordItemPrefab;
        [SerializeField] private GoMainMenuButton goMainMenuButton;

        private IRecordsService _recordsService;

        public void Init(IRecordsService recordsService, IGameEventsDispatcher gameEventsDispatcher)
        {
            _recordsService = recordsService;
            
            goMainMenuButton.Initialize(gameEventsDispatcher);
            
            Refresh();
        }

        private void Refresh()
        {
            var records = _recordsService.Records;
            foreach (Transform child in recordsRoot.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var entry in records)
            {
                var recordItem = Instantiate(recordItemPrefab, recordsRoot);
                recordItem.Setup(entry);
            }

            _recordsService.ResetRecordsStatus();
        }
    }
}