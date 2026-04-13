using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.ConfirmExit
{
    public class ConfirmExitWindow : AbstractWindow
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button stayButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(OnExit);
            stayButton.onClick.AddListener(OnStay);
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveListener(OnExit);
            stayButton.onClick.RemoveListener(OnStay);
        }

        private void OnStay()
        {
            Destroy(gameObject);
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}