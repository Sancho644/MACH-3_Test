using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class CenterUIObject : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private RectTransform rectTransform;

        private void Start()
        {
            Center();
        }

        private void OnRectTransformDimensionsChange()
        {
            Center();
        }

        [ContextMenu("Center")]
        public void Center()
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            rectTransform.anchoredPosition = offset;
        }
    }
}