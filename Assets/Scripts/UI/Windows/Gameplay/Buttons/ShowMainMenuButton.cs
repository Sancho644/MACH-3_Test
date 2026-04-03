using UI.Services.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Gameplay.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ShowMainMenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private IUIFactoryService _uiFactoryService;

        public void Initialize(IUIFactoryService uiFactoryService)
        {
            _uiFactoryService = uiFactoryService;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _uiFactoryService.CreateShowMainMenuWindow();
        }
    }
}