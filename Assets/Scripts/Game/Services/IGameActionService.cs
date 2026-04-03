using UI.Windows.MainMenu;

namespace Game.Services
{
    public interface IGameActionService
    {
        public void DoGameAction(GameActionType actionType);
    }
}