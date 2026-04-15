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
                    _iuiFactoryService.CreateMainMenuWindow();
                    break;
                case WindowType.Gameplay:
                    _iuiFactoryService.CreateGameplayWindow();
                    break;
                case WindowType.Records:
                    _iuiFactoryService.CreateRecordsWindow();
                    break;
                case WindowType.ConfirmExit:
                    _iuiFactoryService.CreateConfirmExitWindow();
                    break;
                case WindowType.About:
                    _iuiFactoryService.CreateAboutWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}