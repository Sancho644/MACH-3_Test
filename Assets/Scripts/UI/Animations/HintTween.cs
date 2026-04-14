using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Animations
{
    public class HintTween : MonoBehaviour
    {
        [SerializeField] private float duration = 0.45f;
        [SerializeField] private float scaleMultiplier = 1.12f;
        [SerializeField] private float moveDistance = 20f;
        [SerializeField] private Ease ease = Ease.InOutSine;
        [SerializeField] private Color gemHintColor = new(1f, 1f, 1f, 0.8f);
        [SerializeField] private Color cellHintColor = new(1f, 0.92f, 0.45f, 1f);
        [SerializeField] private Image image;
        [SerializeField] private RectTransform rectTransform;

        private Sequence _sequence;
        private Vector3 _initialScale;
        private Vector2 _initialAnchoredPosition;
        private Color _initialGemColor;
        private Image _targetCell;
        private Color _initialCellColor;
        private bool _isHintActive;

        private void Awake()
        {
            _initialScale = transform.localScale;
            if (image != null)
                _initialGemColor = image.color;
        }

        public Sequence Play(Vector2 direction, Image targetCell)
        {
            Kill();

            if (rectTransform != null)
                _initialAnchoredPosition = rectTransform.anchoredPosition;

            _targetCell = targetCell;
            if (_targetCell != null)
                _initialCellColor = _targetCell.color;

            if (direction.sqrMagnitude > 0.0001f)
                direction = direction.normalized * moveDistance;
            else
                direction = Vector3.zero;

            _sequence = DOTween.Sequence();
            _sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            _sequence.SetLoops(-1, LoopType.Yoyo);

            if (image != null)
            {
                _sequence.Join(transform.DOScale(_initialScale * scaleMultiplier, duration)
                    .SetEase(ease));
                _sequence.Join(image.DOColor(gemHintColor, duration)
                    .SetEase(ease));
            }

            if (rectTransform != null)
            {
                _sequence.Join(rectTransform.DOAnchorPos(_initialAnchoredPosition + direction, duration)
                    .SetEase(ease));
            }

            if (_targetCell != null)
            {
                _sequence.Join(_targetCell.DOColor(cellHintColor, duration)
                    .SetEase(ease));
            }

            _isHintActive = true;
            return _sequence;
        }

        public void ResetState()
        {
            Kill();

            if (_isHintActive)
            {
                transform.localScale = _initialScale;

                if (rectTransform != null)
                    rectTransform.anchoredPosition = _initialAnchoredPosition;

                if (image != null)
                    image.color = _initialGemColor;

                if (_targetCell != null)
                    _targetCell.color = _initialCellColor;

                _targetCell = null;
                _isHintActive = false;
            }
        }

        private void Kill()
        {
            _sequence?.Kill();
            _sequence = null;

            transform.DOKill();
            image?.DOKill();
            _targetCell?.DOKill();
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void OnDestroy()
        {
            Kill();
        }
    }
}
