using GameEvents;
using UI.Windows.MainMenu;

namespace Game.Services.GameEvents
{
    public class GameActionEvent : IGameEvent
    {
        public GameActionType ActionType { get; private set; }

        public GameActionEvent(GameActionType actionType)
        {
            ActionType = actionType;
        }
    }
}