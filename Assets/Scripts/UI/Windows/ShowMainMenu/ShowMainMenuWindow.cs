using Core.Match3.GameEvents;
using Game.Services.GameEvents;
using GameEvents;
using UI.Windows.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.ShowMainMenu
{
    public class ShowMainMenuWindow : AbstractWindow
    {
        [SerializeField] private Button goMenuButton;
        [SerializeField] private Button stayButton;

        private IGameEventsDispatcher _gameEventsDispatcher;

        public void Init(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
            _gameEventsDispatcher.Dispatch(new PauseInputEvent(true));

            goMenuButton.onClick.AddListener(OnGoMenu);
            stayButton.onClick.AddListener(OnStay);
        }

        private void OnDestroy()
        {
            _gameEventsDispatcher.Dispatch(new PauseInputEvent(false));

            goMenuButton.onClick.RemoveListener(OnGoMenu);
            stayButton.onClick.RemoveListener(OnStay);
        }

        private void OnGoMenu()
        {
            _gameEventsDispatcher.Dispatch(new GameActionEvent(GameActionType.MainMenu));
        }

        private void OnStay()
        {
            Destroy(gameObject);
        }
    }
}