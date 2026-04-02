using System;
using UI.Services.Factory;
using UI.Windows.MainMenu.Services;

namespace UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactoryService _iuiFactoryService;
        private readonly IMainMenuWindowService _mainMenuWindowService;

        public WindowService(IUIFactoryService iuiFactoryService)
        {
            _iuiFactoryService = iuiFactoryService;
        }

        public void Open(WindowType type)
        {
            switch (type)
            {
                case WindowType.MainMenu:
                    _iuiFactoryService.CreateMainMenu();
                    break;
                case WindowType.Gameplay:
                    _iuiFactoryService.CreateGamePlayWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}