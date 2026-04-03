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

        public void Initialize(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;

            goMenuButton.onClick.AddListener(OnGoMenu);
            stayButton.onClick.AddListener(OnStay);
        }

        private void OnDestroy()
        {
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