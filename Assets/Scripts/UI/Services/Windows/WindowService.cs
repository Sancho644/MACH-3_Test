using System;
using UI.Services.Factory;

namespace UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactoryService _iuiFactoryService;

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