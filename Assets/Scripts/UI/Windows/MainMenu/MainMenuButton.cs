using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class MainMenuButton : MonoBehaviour
    {
        [SerializeField] private MainMenuButtonType mainMenuButtonType;
        [SerializeField] private Button button;

        public event Action<MainMenuButtonType> OnClick;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            OnClick?.Invoke(mainMenuButtonType);
        }
    }
}