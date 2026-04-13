using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;
using UnityEngine;

namespace Game.Services
{
    public class ExitActionStrategy : AbstractGameActionStrategy
    {
        public ExitActionStrategy(ISceneLoaderService sceneLoaderService, IWindowService windowService,
            IUIFactoryService uiFactoryService) : base(sceneLoaderService, windowService, uiFactoryService)
        {
        }

        public override void Execute()
        {
            WindowService.Open(WindowType.ConfirmExit);
        }
    }
}