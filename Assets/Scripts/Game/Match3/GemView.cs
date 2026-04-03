using DG.Tweening;
using UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Match3
{
    [RequireComponent(typeof(Image), typeof(MoveTween), typeof(ExplosionTween))]
    public class GemView : MonoBehaviour
    {
        [SerializeField] private MoveTween moveTween;
        [SerializeField] private ExplosionTween explosionTween;

        private Image _image;
        private Vector3 _initialScale;

        public Gem Gem { get; private set; }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _initialScale = transform.localScale;
        }

        public void SetGem(Gem gem)
        {
            Gem = gem;
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
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

        public void ResetVisuals()
        {
            transform.DOKill();
            transform.localScale = _initialScale;

            if (_image != null)
            {
                Color color = _image.color;
                color.a = 1f;
                _image.color = color;
            }
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
