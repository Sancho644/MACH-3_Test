using System.Collections.Generic;
using GameEvents;
using UI.Windows.MainMenu.GameEvents;
using UnityEngine;

namespace UI.Windows.MainMenu
{
    public class MainMenuWindow : AbstractWindow
    {
        [SerializeField] private List<MainMenuButton> buttons;

        private IGameEventsDispatcher _gameEventsDispatcher;

        public void Init(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;

            foreach (var button in buttons)
            {
                button.OnClick += OnButtonClick;
            }
        }

        private void OnDestroy()
        {
            foreach (var button in buttons)
            {
                button.OnClick -= OnButtonClick;
            }
        }

        private void OnButtonClick(MainMenuButtonType buttonType)
        {
            _gameEventsDispatcher.Dispatch(new MainMenuActionEvent(buttonType));
        }
    }
}