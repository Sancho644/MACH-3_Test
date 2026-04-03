using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;
using UnityEngine;

namespace Game.Services
{
    public class ExitActionStrategy : AbstractGameActionStrategy
    {
        public ExitActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService, IUIFactoryService uiFactoryService) : base(sceneLoaderService, windowService, uiFactoryService)
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