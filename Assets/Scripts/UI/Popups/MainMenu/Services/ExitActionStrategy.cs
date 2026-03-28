using Scenes;
using UnityEngine;

namespace UI.Popups.MainMenu.Services
{
    public class ExitActionStrategy : AbstractMainMenuActionStrategy
    {
        public ExitActionStrategy(ISceneLoaderService sceneLoaderService) : base(sceneLoaderService)
        {
        }

        public override void Execute()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}