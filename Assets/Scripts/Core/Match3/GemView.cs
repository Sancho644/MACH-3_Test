using DG.Tweening;
using UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Match3
{
    [RequireComponent(typeof(MoveTween), typeof(ExplosionTween), typeof(HintTween))]
    [RequireComponent(typeof(Image))]
    public class GemView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private MoveTween moveTween;
        [SerializeField] private ExplosionTween explosionTween;
        [SerializeField] private HintTween hintTween;

        private Vector3 _initialScale;
        private Color _initialColor;

        public Gem Gem { get; private set; }

        private void Awake()
        {
            _initialScale = transform.localScale;
            _initialColor = image.color;
        }

        public void SetGem(Gem gem)
        {
            Gem = gem;
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public Tween GetMoveTween(Vector3 target)
        {
            return moveTween.GetTween(target);
        }

        public Tween GetMoveTween(Vector3 target, float duration, Ease ease)
        {
            return moveTween.GetTween(target, duration, ease);
        }

        public Tween GetExplosionTween()
        {
            return explosionTween.GetTween();
        }

        public Sequence GetHintTween(Vector2 direction, Image targetCell)
        {
            return hintTween.Play(direction, targetCell);
        }

        public void ResetVisuals()
        {
            hintTween.ResetState();
            transform.DOKill();
            transform.localScale = _initialScale;

            if (image != null)
                image.color = _initialColor;
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}