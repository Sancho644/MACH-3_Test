using Game.Match3;
using StaticData;
using UnityEngine;

namespace UI.Windows.Gameplay
{
    [RequireComponent(typeof(BoardController))]
    public class GameplayWindow : AbstractWindow
    {
        [SerializeField] private BoardController boardController;

        public void Init(IStaticDataService staticDataService)
        {
            boardController.Initialize(staticDataService);
        }
    }
}