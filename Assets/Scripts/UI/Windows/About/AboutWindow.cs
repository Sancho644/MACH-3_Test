using GameEvents;
using UI.Windows.Records;
using UnityEngine;

namespace UI.Windows.About
{
    public class AboutWindow : AbstractWindow
    {
        [SerializeField] private GoMainMenuButton goMainMenuButton;

        public void Init(IGameEventsDispatcher gameEventsDispatcher)
        {
            goMainMenuButton.Initialize(gameEventsDispatcher);
        }
    }
}