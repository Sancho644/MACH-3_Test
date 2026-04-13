using Game.Services.GameEvents;
using GameEvents;
using UI.Windows.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Records
{
    [RequireComponent(typeof(Button))]
    public class GoMainMenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private IGameEventsDispatcher _gameEventsDispatcher;

        public void Initialize(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _gameEventsDispatcher.Dispatch(new GameActionEvent(GameActionType.MainMenu));
        }
    }
}