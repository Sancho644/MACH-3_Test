using Game.Services.GameEvents;
using GameEvents;
using UI.Windows.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LowScore
{
    public class LowScoreWindow : AbstractWindow
    {
        [SerializeField] private Button button;

        private IGameEventsDispatcher _gameEventsDispatcher;

        public void Init(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
            
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _gameEventsDispatcher.Dispatch(new GameActionEvent(GameActionType.MainMenu));
        }
    }
}