using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FitChildrenBounds : MonoBehaviour
    {
        private void Start()
        {
            UpdateBounds();
        }

        [ContextMenu("Update Bounds")]
        public void UpdateBounds()
        {
            RectTransform parent = (RectTransform)transform;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (RectTransform child in transform)
            {
                foreach (RectTransform newChild in child)
                {
                    Vector2 pos = newChild.anchoredPosition;
                    Vector2 size = Vector2.Scale(newChild.rect.size, newChild.localScale);

                    float left = pos.x - size.x * newChild.pivot.x;
                    float right = pos.x + size.x * (1f - newChild.pivot.x);
                    float bottom = pos.y - size.y * newChild.pivot.y;
                    float top = pos.y + size.y * (1f - newChild.pivot.y);

                    minX = Mathf.Min(minX, left);
                    minY = Mathf.Min(minY, bottom);
                    maxX = Mathf.Max(maxX, right);
                    maxY = Mathf.Max(maxY, top);
                }
            }

            float width = maxX - minX;
            float height = maxY - minY;

            parent.sizeDelta = new Vector2(width, height);
        }
    }
}