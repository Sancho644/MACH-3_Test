using UnityEngine;

namespace UI.Windows
{
    public class UIWindowScaler : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float widthPercent = 0.8f;

        [Range(0f, 1f)]
        [SerializeField] private float heightPercent = 0.6f;

        private RectTransform rectTransform;
        private Canvas canvas;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        private void Start()
        {
            UpdateSize();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float targetWidth = canvasRect.rect.width * widthPercent;
            float targetHeight = canvasRect.rect.height * heightPercent;

            float originalWidth = rectTransform.rect.width;
            float originalHeight = rectTransform.rect.height;

            float scaleX = targetWidth / originalWidth;
            float scaleY = targetHeight / originalHeight;

            // Берем меньший масштаб, чтобы окно гарантированно помещалось
            float scale = Mathf.Min(scaleX, scaleY);

            rectTransform.localScale = Vector3.one * scale;
            Debug.Log("UpdateSize");
        }
    }
}