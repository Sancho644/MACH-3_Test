using System.Collections.Generic;
using Core.Services;
using UI.Popups.MainMenu.Services;
using UnityEngine;

namespace UI.Popups.MainMenu
{
    public class MainMenuPopup : MonoBehaviour
    {
        [SerializeField] private List<MainMenuButton> buttons;
        
        private IMainMenuPopupService _mainMenuService;

        private void Awake()
        {
            _mainMenuService = AllServices.Get<IMainMenuPopupService>();
            
            foreach (var button in buttons)
            {
                button.OnClick += OnButtonClick;
            }
        }

        private void OnDestroy()
        {
            foreach (var button in buttons)
            {
                button.OnClick -= OnButtonClick;
            }
        }

        private void OnButtonClick(MainMenuButtonType buttonType)
        {
            _mainMenuService.DoMainMenuAction(buttonType);
        }
    }
}