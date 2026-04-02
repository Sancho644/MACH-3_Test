using GameEvents;

namespace UI.Windows.MainMenu.GameEvents
{
    public class MainMenuActionEvent : IGameEvent
    {
        public MainMenuButtonType ButtonType { get; private set; }

        public MainMenuActionEvent(MainMenuButtonType buttonType)
        {
            ButtonType = buttonType;
        }
    }
}