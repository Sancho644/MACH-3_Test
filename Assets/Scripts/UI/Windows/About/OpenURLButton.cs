using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.About
{
    public class OpenURLButton : MonoBehaviour
    {
        [SerializeField] private string link;
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OpenURL);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OpenURL);
        }

        private void OpenURL()
        {
            if (!string.IsNullOrEmpty(link))
            {
                Application.OpenURL(link);
            }
        }
    }
}